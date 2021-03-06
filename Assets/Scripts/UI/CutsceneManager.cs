﻿using System.Collections;
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
		public Button m_choiceButtonTemplate;
		public Transform m_defaultCamerPosition;

		[HideInInspector] public AudioSource m_backgroundMusicSource;
		SoundManager m_soundManager;
		SongController m_songController;    // used to play the specified song after the cutscene is over (if any)
		int m_speachBubbleNumber = 0;       // the current line to write
		LevelGenerator m_levelgen;
		StoryModeManager m_storyModeManager;

		List<GameObject> m_choices = new List<GameObject>();


        // Controller navigation variables
        [SerializeField] private UIButtonManager m_buttonManager = null;
        private List<UIButton> m_callboxButtons = new List<UIButton>();

        private void Start()
		{
			if (!m_choiceButtonTemplate)
				Debug.LogError("Add a template for the choice button (used in CutsceneManager)");
			else
				m_choiceButtonTemplate.gameObject.SetActive(false);
			m_levelgen = FindObjectOfType<LevelGenerator>();
			m_soundManager = FindObjectOfType<SoundManager>();
			m_songController = FindObjectOfType<SongController>();
			m_storyModeManager = FindObjectOfType<StoryModeManager>();

            // This part is used to show error dialog messages if there are parts of the script that have not been initialised properly
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                if (!m_buttonManager)
                {
                    UnityEditor.EditorUtility.DisplayDialog("Error", "Cutscene Manager script does not have a button manager", "Exit");
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            }

#endif
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
			if (!CheckChoices(cutscene.m_preCutsceneChoice, cutscene))
				StartCoroutine(StartCutscene(cutscene));
		}

		void StartCutsceneCaller(Cutscene cs)
		{
			//remove choice buttons
			foreach (GameObject choice in m_choices)
				Destroy(choice);
			m_choices.Clear();
            m_buttonManager.buttons.Clear();

            // Clears callbox button list
            m_callboxButtons.Clear();

			m_speachBubbleNumber = 0;
			StartCoroutine(StartCutscene(cs));
		}

		public bool CheckChoices(Choice[] choiceArray, Cutscene cs)
		{
			// if there are pre choices to be made let the player do so
			if (choiceArray.Length == 0)
				return false;

			// display choices
			foreach (Choice c in choiceArray)
			{
				GameObject go = Instantiate(m_choiceButtonTemplate.gameObject, m_choiceButtonTemplate.transform.parent);
				Button btn = go.GetComponent<Button>();

                // Adds a UI button for each callbox button
                UIButton uiButton = null;
                uiButton = go.AddComponent<UIButton>();

                // Adds a reference to the new button
                m_callboxButtons.Add(uiButton);

				btn.image.sprite = c.Image;

				if (c.CutsceneToPlay != null)
					btn.onClick.AddListener(() => { StartCutsceneCaller(c.CutsceneToPlay);});
				else
					btn.onClick.AddListener(() => { StartCutsceneCaller(cs);});

				Debug.Log(c.CutsceneToPlay);

				go.SetActive(true);
				m_choices.Add(go);

                //Adds UI button to UI button manager
                m_buttonManager.buttons.Add(uiButton);
            }

            // Adds navigation to callbox buttons once they are all instantiated
            for (int i = 0; i < m_callboxButtons.Count; i++)
            {
                if (i - 1 >= 0)
                    m_callboxButtons[i].leftBtn = m_callboxButtons[i - 1];
                if (i + 1 < m_callboxButtons.Count)
                    m_callboxButtons[i].rightBtn = m_callboxButtons[i + 1];
            }

            m_buttonManager.SelectButton(m_buttonManager.buttons[0]);

            return true;
		}

		public void MoveCamera(Cutscene cutscene, bool moveIn)
		{
			if (cutscene.m_cameraTagToUse.Length > 0)
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
			//Debug.LogError("Playing cutscene " + cs.name);
			if (cs.m_endGameConditionToAdd != EndGameCondition.NONE)
				m_storyModeManager.AddCondition(cs.m_endGameConditionToAdd);

			// if this cutscene exists in any node, it will be unlocked
			m_storyModeManager.UnlockNode(cs);

			// if there is a song to be loaded, do it at the start
			if (cs.m_levelToPlay)
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
					yield return  new WaitForVoiceLineEnd(cs.m_conversation[m_speachBubbleNumber].waitTime, KeyCode.Space);
					// go to the next speach bubble
					m_speachBubbleNumber++;
				}
			}
			// make the conversation invisible
			m_conversationBox.transform.parent.parent.gameObject.SetActive(false);

			if (!CheckChoices(cs.m_postCutsceneChoice, cs))
				StartCoroutine(StartSong(cs));
		}

		IEnumerator StartSong(Cutscene cs)
		{ 
			// if there is a song to play
			if (cs.m_levelToPlay && cs.m_levelToPlay.m_song)
			{
				// change the level gen
				m_levelgen.ChangeLevel(cs.m_levelToPlay);
				// if the song is not loading or analysing still
				while (m_songController.m_bgThread != null)
					yield return null;
				// play it
				m_songController.PlayAudio();
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

			foreach (GameObject go in m_choices)
				Destroy(go);
			m_choices.Clear();
		}


	}
}