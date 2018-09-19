using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MusicalGameplayMechanics;

namespace BeatRider
{
	/// <summary>
	/// Manages playing cutscenes
	/// </summary>
	public class CutsceneManager : MonoBehaviour
	{
		public Sound m_backgroundMusic;
		public Image m_imageBox;
		public Text m_nameBox;              // text box for the name of whoever is talking
		public Text m_conversationBox;      // text box for the substance of what someone is saying
		public Transform m_defaultCamerPosition;

		[HideInInspector] public AudioSource m_backgroundMusicSource;
		SoundManager m_soundManager;
		SongController m_songController;    // used to play the specified song after the cutscene is over (if any)
		int m_speachBubbleNumber = 0;       // the current line to write
		LevelGenerator m_levelgen;

		private void Start()
		{
			m_levelgen = FindObjectOfType<LevelGenerator>();
			m_soundManager = FindObjectOfType<SoundManager>();
			m_songController = FindObjectOfType<SongController>();
		}

		/// <summary>
		/// Plays the cutscene specified
		/// </summary>
		/// <param name="cutscene">
		/// Cutscene to play
		/// </param>
		public void PlayCutscene(Cutscene cutscene)
		{
			m_backgroundMusicSource = m_soundManager.PlaySound(m_backgroundMusic);
			MoveCamera(cutscene, true);
			m_speachBubbleNumber = 0;
			StartCoroutine(StartCutscene(cutscene));
		}

		public void MoveCamera(Cutscene cutscene, bool moveIn)
		{
			CameraMover mover = new CameraMover();
			mover.Start();

			GameObject cutsceneCam = GameObject.FindGameObjectWithTag(cutscene.m_cameraTagToUse);
			if (cutsceneCam)
			{
				if (moveIn)
				{
					mover.m_targetTransform = cutsceneCam.transform;
					mover.m_cinematicMovementEnabled = false;
				}
				else
				{
					mover.m_targetTransform = m_defaultCamerPosition;
					mover.m_cinematicMovementEnabled = true;
				}
				mover.MoveToPos();
			}
			else
				Debug.LogError("There is no camera with the tag " + cutscene.m_cameraTagToUse);
		}

		/// <summary>
		/// Stops the cutscene from playing
		/// </summary>
		public void StopCutscene()
		{
			m_speachBubbleNumber = 9999;
		}

		public IEnumerator StartCutscene(Cutscene cs)
		{
			// if there is a song to be loaded, do it at the start
			if (cs.m_levelToPlay.m_song)
				m_songController.OpenSoundFile(cs.m_levelToPlay.m_song);

			// make the conversation visible
			m_conversationBox.transform.parent.parent.gameObject.SetActive(true);
			// while the conversation is still going
			while (m_speachBubbleNumber < cs.m_conversation.Length)
			{
				SpeachBubble sb = cs.m_conversation[m_speachBubbleNumber];
				if (sb.Audio.soundToPlay)
					m_soundManager.PlaySound(sb.Audio);
				if (sb.Image)
					m_imageBox.sprite = sb.Image;
				m_nameBox.text = sb.Name + ":";
				m_conversationBox.text = sb.Content;
				// if no wait time is specified, wait till an external force continues the conversation
				if (cs.m_conversation[m_speachBubbleNumber].waitTime == 0)
					yield return null;
				else
				{
					// wait for the specified amount of time in realtime to negate any pausing that may occur
					yield return new WaitForSecondsRealtime(cs.m_conversation[m_speachBubbleNumber].waitTime);
					// go to the next speach bubble
					m_speachBubbleNumber++;
				}
			}
			// if there is a song to play
			if (cs.m_levelToPlay.m_song)
			{
				// if the song is not loading or analysing still
				while (m_songController.m_bgThread != null)
					yield return null;
				// play it
				m_songController.PlayAudio();
				// change the level gen
				m_levelgen.m_levelTemplate = cs.m_levelToPlay.m_levelTemplate;
				// update cutscene
				m_songController.m_cutsceneToPlayAtEnd = cs.m_levelToPlay.m_endCutscene;
			}
			// hide the conversation
			m_conversationBox.transform.parent.parent.gameObject.SetActive(false);
			// un pause the game (if it was not paused in the first place this wont matter)
			Time.timeScale = 1;
			MoveCamera(cs, false);
			if (m_backgroundMusicSource)
				if (m_backgroundMusicSource.isPlaying)
					Destroy(m_backgroundMusicSource);
		}
	}
}