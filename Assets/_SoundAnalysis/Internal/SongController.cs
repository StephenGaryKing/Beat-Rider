using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using System.Numerics;
using DSPLib;
using System.IO;
using UnityEngine.UI;
using BeatRider;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MusicalGameplayMechanics
{
	public enum StopSongConditions
	{
		EndOfSong,
		RestartSong,
		PlayerDead,
		ReturnToMenu,
        AutoWin
	}

	[System.Serializable]
	public struct Pass
	{
		public string name; // identifier for this pass (eg. "LowPass")
		public float beatThreshold;
		[Range(0, 512)]
		public int minFreq; // inclusive
		[Range(1, 513)]
		public int maxFreq; // exclusive
	};

	/// <summary>
	/// This event is used on a frame by frame basis to expose the list of 
	/// SpectralFluxInfo's that have been previously calculated as well as 
	/// the current index the song is at during this point in time
	/// </summary>
	[Serializable]
	public class SpectrumEvent : UnityEvent<List<SavedPass>, int> { }

	/// <summary>
	/// The song controller is what handles the playing of the chosen song.
	/// it also handles the delegation of events related to the preprocessed
	/// audio spectrums.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class SongController : MonoBehaviour
	{
		AudioSource _audioSource;                           // The audio source attached to this Gameobject
		[HideInInspector] public static float[] m_curSpectrum;
		string m_spectralFluxFilePath;						// The file path used when saving and loading spectral analysis files

		public static float m_timeToLookAhead = 0f;				// Used to preemptively expose the spectrum (for use when creating things prior to a beat happening)
		int m_indexToAddToIndexer = 0;                      // Time to look ahead converted to an amount of idexes									

		public static SpectrumEvent m_onFrontBufferDump = new SpectrumEvent();			// the event called at the start of the song to populate the game field
		public static SpectrumEvent m_onMusicIsPlaying = new SpectrumEvent();          // the event called every frame in order to expose the spectrum on that frame											
		public static SpectrumEvent m_onEarlyMusicIsPlaying = new SpectrumEvent();     // the event called 'm_timeToLookAhead' seconds before this point in time in order to expose the spectrum on that frame
		[HideInInspector] public bool m_paused = false;

		// UI
		[Header("UI:")]
		public Button m_selectSongButton;
		public Button m_playSongButton;
		public Slider m_songProgressSlider;
		public Slider m_spectralAnalisisSlider;
		float m_spectralAnalisisSliderValue;
		public Transform m_saveIcon;
		public Transform m_loadIcon;
		public MenuTransition m_freeFlowMenuTransition;
		public MenuTransition m_playerDeathMenuTransition;
		public MenuTransition m_LevelFailedMenuTransition;

		int m_numChannels;									// Amount of channels in the current song
		int m_numTotalSamples;								// Total amount of samples in this song
		int m_sampleRate;									// Frequency of this song
		float m_clipLength;									// Length of this song
		float[] m_multiChannelSamples;						// Samples of the song stored as alternating channels eg. [L,R,L,R,L,R]
		[HideInInspector] public bool m_songIsBeingPlayed = false;
		[HideInInspector] public Thread m_bgThread = null;

		SpectralFluxSaver m_spectralFluxSaver = new SpectralFluxSaver();    // Serializable class used to save and load spectral analysis files
		public List<Pass> m_passes;                         // The passes to better analyze specific sections of the spectrum
		MusicalityAnalyzer m_preProcessedMusicalityAnalyzer;                // Class used to analyse the BPM, Pitch Variance and Beat Density of the spectrums of a song prior to playing it

		//beat rider
		PlayerCollision m_player;
		ScoreBoardLogic m_scoreBoard;
		LevelGenerator m_levelGen;
		[HideInInspector] public Cutscene m_cutsceneToPlayAtEnd;
		CutsceneManager m_cutsceneManager;
		MenuMusicManager m_menuMusicManager;
		StoryModeManager m_storyModeManager;
		CraftingManager m_craftingManager;

		public static bool m_freeFlow = false;

		void Start()
		{
			m_craftingManager = FindObjectOfType<CraftingManager>();
			m_storyModeManager = FindObjectOfType<StoryModeManager>();
			m_player = FindObjectOfType<PlayerCollision>();
			m_scoreBoard = FindObjectOfType<ScoreBoardLogic>();
			m_levelGen = FindObjectOfType<LevelGenerator>();
			m_cutsceneManager = FindObjectOfType<CutsceneManager>();
			m_preProcessedMusicalityAnalyzer = new MusicalityAnalyzer();
			if (m_playSongButton)
				m_playSongButton.interactable = false;						// Since no song is loaded the play song button should not be accessable
			_audioSource = GetComponent<AudioSource>();
			_audioSource.minDistance = 0.3f;
			m_menuMusicManager = FindObjectOfType<MenuMusicManager>();
		}

		void Update()
		{
			if (s_returnToMenu)
			{
				s_returnToMenu = false;
				ReturnToMenu();
			}
            if (m_songIsBeingPlayed && Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.End))
                ActualStopSong(StopSongConditions.AutoWin);

			if (m_loadIcon != null)
			{
				if (m_spectralFluxSaver.m_loading)
					m_loadIcon.gameObject.SetActive(true);		// Show the loading icon if a file is being loaded
				else
					m_loadIcon.gameObject.SetActive(false);     // Hide the loading icon if a file is not being loaded
			}

			if (m_saveIcon != null)
			{
				if (m_spectralFluxSaver.m_saving)
					m_saveIcon.gameObject.SetActive(true);      // Show the saving icon if a file is being saved
				else
					m_saveIcon.gameObject.SetActive(false);     // Hide the saving icon if a file is being not saved
			}

			if (m_spectralAnalisisSlider != null && m_spectralAnalisisSliderValue > 0 && m_spectralAnalisisSliderValue < 0.99f)
			{
				m_spectralAnalisisSlider.gameObject.SetActive(true);
				m_spectralAnalisisSlider.value = m_spectralAnalisisSliderValue;				// Update the spectral analysis loading bar based on it's current progress
			}
			else
				m_spectralAnalisisSlider.gameObject.SetActive(false);

			if (_audioSource.clip != null)
			{
				m_songProgressSlider.value = _audioSource.time / _audioSource.clip.length;	// Update the timeline for the song (used to show the player how much time is left in a song)
			}

			if (m_bgThread != null)
			{
				// Disable buttons when a sound file is being loaded, saved or analyzed
				m_selectSongButton.interactable = false;
				m_playSongButton.interactable = false;
			}
			else
			{
				m_selectSongButton.interactable = true;
				if (_audioSource.clip != null && !m_songIsBeingPlayed)
				{
					m_playSongButton.interactable = true;			// Allow the play song button to be interacted with if there is a song that can be played and one is not already playing
				}
			}
		}

		/// <summary>
		/// Change the volume of the AudioSource based on the volume slider's value
		/// </summary>
		public void UpdateVolume(Slider slider)
		{
			_audioSource.volume = slider.value;				// Adjust the volume of the song based on the volume slider (this does not affect the spectral analysis data)
		}

		/// <summary>
		/// Play the song that has been selected
		/// </summary>
		public void PlayAudio()
		{
			if (m_bgThread == null && !m_songIsBeingPlayed)
			{
				m_scoreBoard.FindAllNotes(m_spectralFluxSaver.m_savedPasses);
				//_audioSource.PlayDelayed(m_timeToLookAhead);        // Play the song that has been loaded with the delay specified (this delay must occur in order to correctly preemptively call events)
				_audioSource.Play();
				m_onFrontBufferDump.Invoke(m_spectralFluxSaver.m_savedPasses, 0);
				Debug.Log("playing the song named " + _audioSource.clip.name);
				StartCoroutine(UpdateListeners());
				m_playSongButton.interactable = false;
			}
		}

		[EnumAction(typeof(StopSongConditions))]
		public void StopSong(int num)
		{
			ActualStopSong((StopSongConditions)num);
		}

		static bool s_returnToMenu = false;
		public static void ReturnToMenuStatic()
		{
			s_returnToMenu = true;
		}

		public void ActualStopSong(StopSongConditions condition)
		{
			Debug.Log("stop");
			StopAllCoroutines();
			_audioSource.Stop();
			m_songIsBeingPlayed = false;
			switch (condition)
			{
				case (StopSongConditions.EndOfSong):
					EndOfSong();
					break;
				case (StopSongConditions.PlayerDead):
					PlayerDead();
					break;
				case (StopSongConditions.RestartSong):
					RestartSong();
					break;
				case (StopSongConditions.ReturnToMenu):
					ReturnToMenu();
					break;
                case (StopSongConditions.AutoWin):
                    WinSong();
                    break;
			}
			ResetValues();
		}

		void EndOfSong()
		{
			m_craftingManager.CompletePendingCrafts();
			m_levelGen.WipeObjects();
			AchievementManager.OnTallyPickups("Final");
            AchievementManager.OnLevelPercent(m_scoreBoard.FindPercentageOfNotes() + ":" + m_levelGen.m_currentLevel.name + ":" + GameController.GetDifficulty().ToString());


            if (PassedThisDifficulty(m_levelGen.m_currentLevel))
			{
				if (m_cutsceneToPlayAtEnd)
					m_cutsceneManager.PlayCutscene(m_cutsceneToPlayAtEnd);
				else
					ReturnToMenu();
			}
			else
			{
				m_LevelFailedMenuTransition.PlayTransitions();
			}
		}

        void WinSong()
        {
            m_craftingManager.CompletePendingCrafts();
            m_levelGen.WipeObjects();
            AchievementManager.OnTallyPickups("Final");
            AchievementManager.OnLevelPercent(m_scoreBoard.FindPercentageOfNotes() + ":" + m_levelGen.m_currentLevel.name + ":" + GameController.GetDifficulty().ToString());

            if (m_cutsceneToPlayAtEnd)
                m_cutsceneManager.PlayCutscene(m_cutsceneToPlayAtEnd);
            else
                ReturnToMenu();
        }

		bool PassedThisDifficulty(Level level)
		{
			//easy
			if (GameController.GetDifficulty() == Difficulty.EASY)
			{
				if (m_scoreBoard.FindPercentageOfNotes() > level.m_easyPercantage)
					return true;
				else
					return false;
			}
			//medium
			if (GameController.GetDifficulty() == Difficulty.MEDUIM)
			{
				if (m_scoreBoard.FindPercentageOfNotes() > level.m_mediumPercantage)
					return true;
				else
					return false;
			}
			//hard
			if (GameController.GetDifficulty() == Difficulty.HARD)
			{
				if (m_scoreBoard.FindPercentageOfNotes() > level.m_hardPercantage)
					return true;
				else
					return false;
			}
			return false;
		}

		void PlayerDead()
		{
			m_craftingManager.ClearPendingCrafts();
			m_levelGen.WipeObjects();
			m_playerDeathMenuTransition.PlayTransitions();
		}

		void RestartSong()
		{
			m_craftingManager.ClearPendingCrafts();
			m_levelGen.WipeObjects();
			PlayAudio();
			m_player.Revive();
		}

		void ReturnToMenu()
		{
			m_freeFlow = false;
			m_craftingManager.ClearPendingCrafts();
			m_storyModeManager.ClearConditions();
			m_levelGen.WipeObjects();
			m_menuMusicManager.PlayRandomSong();
			m_freeFlowMenuTransition.PlayTransitions();
		}

		/// <summary>
		/// Update all the event listeners with beats and access to the spectrum
		/// </summary>
		/// <returns>
		/// Waits until the next frame
		/// </returns>
		IEnumerator UpdateListeners()
		{
			//float frontBufferTime = -m_timeToLookAhead;		// This front buffer time is used to correctly simulate the time before a song starts
			
			float lastTimeChecked = 0;                      // This variable is used to avoid spamming listeners with updates every frame if the index is the same
			float lastEarlyTimeChecked = 0;
			while (true)
			{

				m_songIsBeingPlayed = false;

				if (_audioSource.isPlaying)
				{
					m_songIsBeingPlayed = true;
				}
#if UNITY_EDITOR
				else if (EditorApplication.isPaused)
				{
					m_songIsBeingPlayed = true;
				}
#endif

				if (!m_songIsBeingPlayed)
				{
					Debug.Log("The song named" + _audioSource.clip.name + " is over");
					break;
				}

				int currentTime = Mathf.RoundToInt(_audioSource.time * 10f);

				// int earlyTime = Mathf.RoundToInt((frontBufferTime + _audioSource.time + m_timeToLookAhead) * 10f);
				int earlyTime = Mathf.RoundToInt((_audioSource.time + m_timeToLookAhead) * 10f);

				if (earlyTime != lastEarlyTimeChecked)
				{
					lastEarlyTimeChecked = earlyTime;
					if (m_spectralFluxSaver.m_savedPasses[0].runtimeData.ContainsKey(earlyTime))
					{
						try
						{
							m_onEarlyMusicIsPlaying.Invoke(m_spectralFluxSaver.m_savedPasses, earlyTime);
						}
						catch(System.Exception exc) {
                            // This error sucks. lets just ignore it for now
							//Debug.LogError (exc.Message + " event size is " + m_onEarlyMusicIsPlaying.GetPersistentEventCount());
						}
					}
				}
				if (lastTimeChecked != currentTime)
				{
					lastTimeChecked = currentTime;
					m_curSpectrum = new float[1024];
					_audioSource.GetSpectrumData(m_curSpectrum, 0, FFTWindow.BlackmanHarris);
					if (m_spectralFluxSaver.m_savedPasses[0].runtimeData.ContainsKey(currentTime))
					{
						m_onMusicIsPlaying.Invoke(m_spectralFluxSaver.m_savedPasses, currentTime);
					}
				}

				yield return new WaitForFixedUpdate();
			}

			ActualStopSong(StopSongConditions.EndOfSong);
		}

		/// <summary>
		/// Find the index of the sample at the specified time.
		/// </summary>
		/// <param name="curTime"> 
		/// Time in seconds from the start of the song.
		/// </param>
		/// <returns>
		/// Index of the sample.
		/// </returns>
		public int GetIndexFromTime(float curTime)
		{
			float lengthPerSample = m_clipLength / (float)m_numTotalSamples;

			return Mathf.FloorToInt(curTime / lengthPerSample);
		}

		/// <summary>
		/// Find the time from the start of the song, given the specified index of a sample in the song.
		/// </summary>
		/// <param name="index">
		/// Index of the sample in question.
		/// </param>
		/// <returns>
		/// Time in seconds.
		/// </returns>
		public float GetTimeFromIndex(int index)
		{
			return ((1f / m_sampleRate) * index);
		}

		/// <summary>
		/// Open a sound file using my ingame method
		/// </summary>
		public void OpenSoundFile(string filePath)
		{
			StartCoroutine(LoadFile(filePath));
		}

		/// <summary>
		/// Open a sound file using a clip
		/// </summary>
		public void OpenSoundFile(AudioClip clip)
		{
			if (clip != null)
			{
				_audioSource.clip = clip;
				_audioSource.clip.name = clip.name;     // Isolate its name;
				GetData();
			}
		}

		public void OpenSoundFileByName(string filePath)
		{
			string file = Application.dataPath + "/Songs/" + filePath + ".wav";	
			StartCoroutine(LoadFile(file));
		}

		public void Pause()
		{
			if (_audioSource.isPlaying)
			{
				m_paused = true;
				_audioSource.pitch = 0;
			}
		}

		public void UnPause()
		{
			m_paused = false;
			_audioSource.pitch = 1;
		}

		void GetData()
		{
			if (_audioSource.clip != null)
			{
				m_spectralFluxFilePath = Path.Combine(Application.streamingAssetsPath, _audioSource.clip.name + ".json");       // The location of the save file for this song

				// Initialize important values about the song
				m_multiChannelSamples = new float[_audioSource.clip.samples * _audioSource.clip.channels];
				m_numChannels = _audioSource.clip.channels;
				m_numTotalSamples = _audioSource.clip.samples;
				m_clipLength = _audioSource.clip.length;

				// We are not evaluating the audio as it is being played by Unity, so we need the clip's sampling rate
				// This will be applied later on a per pass basis
				m_sampleRate = _audioSource.clip.frequency;

				// If in stereo, samples will return with left and right channels interweaved
				_audioSource.clip.GetData(m_multiChannelSamples, 0);

				// Calculate the offset of the early event calls
				m_indexToAddToIndexer = GetIndexFromTime(m_timeToLookAhead) / 1024;
				Debug.Log("GetData done");

				if (m_spectralFluxSaver.SongExists(m_spectralFluxFilePath))
				{
					LoadSoundFile();                        // Load the saved spectral analysis info if it exists for this song
					m_playSongButton.interactable = true;
				}
				else
				{
					AnalyzeSoundFile();                     // Perform a spectral analysis of this song
					m_playSongButton.interactable = false;
				}
			}
		}

		IEnumerator LoadFile(string fileLocation)
		{
			m_cutsceneToPlayAtEnd = null;
			if (fileLocation != null)
			{
				string ext = Path.GetExtension(fileLocation);
				if (ext == ".MP3" || ext == ".mp3")
				{
					using (var www = new WWW("file:///" + fileLocation))
					{
						yield return www;
						NAudioPlayer.StartMp3Thread(www.bytes);

						while (NAudioPlayer.threadDone == false)
							yield return null;

						_audioSource.clip = NAudioPlayer.FromMp3Data();
						_audioSource.clip.name = Path.GetFileNameWithoutExtension(fileLocation);     // Isolate its name;
						GetData();
					}
				}
				else
				{
					using (var www = new WWW("file:///" + fileLocation))
					{
						yield return www;
						_audioSource.clip = www.GetAudioClip();
						_audioSource.clip.name = Path.GetFileNameWithoutExtension(fileLocation);     // Isolate its name;
						GetData();
					}
				}
			}
		}

		/// <summary>
		/// Load the spectrum alanysis file at the location denoted by the variable 'm_spectralFluxFilePath'
		/// </summary>
		void LoadSoundFile()
		{
			m_bgThread = new Thread(LoadSpectrumThreaded);
			m_bgThread.Start();
		}

		/// <summary>
		/// Analysis the spectrums contained in the variable 'm_multiChannelSamples'
		/// </summary>
		void AnalyzeSoundFile()
		{
			m_bgThread = new Thread(GetFullSpectrumThreaded);
			m_bgThread.Start();
		}

		/// <summary>
		/// Load the saved spectral analysis info file at the location denoted by the variable 'm_spectralFluxFilePath' on a seperate thread
		/// </summary>
		public void LoadSpectrumThreaded()
		{
			Debug.Log("Starting Background Thread To Load");
			// load the data into the spectral flux saver
			m_spectralFluxSaver.LoadSong(m_spectralFluxFilePath);

			Debug.Log(m_spectralFluxSaver.m_savedPasses.Count + " Passes");
			Debug.Log(m_spectralFluxSaver.m_savedPasses[0].savedData.Count + " Samples");

			Debug.Log("Spectrum Loading done");
			Debug.Log("Background Thread Completed");
			m_bgThread = null;
		}

		/// <summary>
		/// Analyse the song using the Spectral Flux analyzer and the musicality analyzer
		/// </summary>
		public void GetFullSpectrumThreaded()
		{
			try
			{
				Debug.Log("Starting Background Thread To Analyse");

				// We only need to retain the samples for combined channels over the time domain
				float[] preProcessedSamples = new float[m_numTotalSamples];

				int numProcessed = 0;
				float combinedChannelAverage = 0f;
				for (int i = 0; i < m_multiChannelSamples.Length; i++)
				{
					combinedChannelAverage += m_multiChannelSamples[i];

					// Each time we have processed all channels samples for a point in time, we will store the average of the channels combined
					// This is done to convert stereo sound files to mono (much easier to work with)
					// Eg. [L,R,L,R,L,R] -> [mono,mono,mono]
					if ((i + 1) % m_numChannels == 0)
					{
						preProcessedSamples[numProcessed] = combinedChannelAverage / m_numChannels;
						numProcessed++;
						combinedChannelAverage = 0f;
					}
				}

				Debug.Log("Combine Channels done");
				//Debug.Log(preProcessedSamples.Length);

				// Once we have our audio sample data prepared, we can execute an FFT to return the spectrum data over the time domain
				int spectrumSampleSize = 1024;
				int iterations = preProcessedSamples.Length / spectrumSampleSize;

				FFT fft = new FFT();
				fft.Initialize((UInt32)spectrumSampleSize);

				Debug.Log(string.Format("Processing {0} time domain samples for FFT", iterations));
				double[] sampleChunk = new double[spectrumSampleSize];

				List<SpectralFluxAnalyzer> spectralFluxAnalyzers = new List<SpectralFluxAnalyzer>();
				// create (number of passes) spectrum analyzers
				for (int i = 0; i < m_passes.Count; i++)
				{
					SpectralFluxAnalyzer sfa = new SpectralFluxAnalyzer();
					sfa.m_sampleRate = m_sampleRate;
					sfa.m_pass = m_passes[i];
					sfa.m_thresholdMultiplier = m_passes[i].beatThreshold;
					spectralFluxAnalyzers.Add(sfa);
				}

				for (int i = 0; i < iterations; i++)
				{
					// Grab the current 1024 chunk of audio sample data
					Array.Copy(preProcessedSamples, i * spectrumSampleSize, sampleChunk, 0, spectrumSampleSize);

					// Apply our chosen FFT Window
					double[] windowCoefs = DSP.Window.Coefficients(DSP.Window.Type.Hanning, (uint)spectrumSampleSize);
					double[] scaledSpectrumChunk = DSP.Math.Multiply(sampleChunk, windowCoefs);
					double scaleFactor = DSP.Window.ScaleFactor.Signal(windowCoefs);

					// Perform the FFT and convert output (complex numbers) to Magnitude
					Complex[] fftSpectrum = fft.Execute(scaledSpectrumChunk);
					double[] scaledFFTSpectrum = DSPLib.DSP.ConvertComplex.ToMagnitude(fftSpectrum);
					scaledFFTSpectrum = DSP.Math.Multiply(scaledFFTSpectrum, scaleFactor);

					// These 1024 magnitude values correspond (roughly) to a single point in the audio timeline
					float curSongTime = GetTimeFromIndex(i) * spectrumSampleSize;

					float[] spectrumArray = Array.ConvertAll(scaledFFTSpectrum, x => (float)x);
					// for each pass use its corresponding analyzer to analyze the spectrum
					for (int j = 0; j < spectralFluxAnalyzers.Count; j++)
						spectralFluxAnalyzers[j].AnalyzeSpectrum(spectrumArray, curSongTime);
					
					// Change the size of the loading bar for spectral analysis
					m_spectralAnalisisSliderValue = (float)i / iterations;
				}

				for (int i = 0; i < spectralFluxAnalyzers.Count; i++)
				{
					// analyze each pass using the same musicality analyzer
					m_preProcessedMusicalityAnalyzer.FindTempo(spectralFluxAnalyzers[i].m_spectralFluxSamples);                   // Send our list of spectrums off to the Musicality Analyzer to be analyzed for tempo
					m_preProcessedMusicalityAnalyzer.FindBeatDensity(ref spectralFluxAnalyzers[i].m_spectralFluxSamples);         // Send our list of spectrums off to the Musicality Analyzer to be analyzed for beat density
					m_preProcessedMusicalityAnalyzer.FindVolumeVariance(ref spectralFluxAnalyzers[i].m_spectralFluxSamples);      // Send our list of spectrums off to the Musicality Analyzer to be analyzed for volume variance
					m_preProcessedMusicalityAnalyzer.FindPitchVariance(ref spectralFluxAnalyzers[i].m_spectralFluxSamples);       // Send our list of spectrums off to the Musicality Analyzer to be analyzed for pitch variance
				}

				// give all passes to the saver as a list to be saved propperly
				m_spectralFluxSaver.SaveSong(spectralFluxAnalyzers, m_spectralFluxFilePath);         // Save our analysis data to a '.json' file to be loaded next time we play this song

				Debug.Log(spectralFluxAnalyzers[0].m_spectralFluxSamples[0].spectrum.Length + " Samples were analyzed");
				Debug.Log("Spectrum Analysis done");
				Debug.Log("Background Thread Completed");

				m_bgThread = null;			// Kill the thread
			}
			catch (Exception e)
			{
				// Catch exceptions here since the background thread won't always surface the exception to the main thread
				Debug.Log(e.ToString());
			}
		}

		public void ResetValues()
		{
			m_player.ResetSpeed();
			m_scoreBoard.ResetScores();
		}
	}
}