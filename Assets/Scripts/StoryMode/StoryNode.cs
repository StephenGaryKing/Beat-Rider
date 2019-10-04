using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeatRider
{
	[RequireComponent(typeof(Image))]
	public class StoryNode : MonoBehaviour {

		Image m_image;
		Button m_button;
		CutsceneManager m_cutsceneManager;
		StoryModeManager m_storyModeManager;
		public Sprite m_activeImage;
		public Sprite m_inactiveImage;
		public StoryNode m_parent;
		public Cutscene m_cutsceneToPlay;
		public EndGameCondition m_EndGameCondition;
		public bool m_unlocked = false;
		public Text m_textBox;
		[Multiline]
		public string m_infoText;
		bool m_unlockedLastFrame = false;

		public virtual void Select()
		{
			if (m_unlocked)
			{
				m_cutsceneManager.PlayCutscene(m_cutsceneToPlay);
				m_storyModeManager.InitializeConditions(this);
			}
		}

		public void UpdateTextBox()
		{
			if (m_unlocked)
				m_textBox.text = m_infoText;
		}

		public void Awake()
		{
			m_button = GetComponent<Button>();
			m_button.interactable = false;
            m_storyModeManager = FindObjectOfType<StoryModeManager>();
            m_cutsceneManager = FindObjectOfType<CutsceneManager>();
			m_image = GetComponent<Image>();
			m_image.sprite = m_inactiveImage;
			if (m_parent == null)
				Unlock();
			else
				m_image.sprite = m_inactiveImage;
            if (m_unlocked && !m_unlockedLastFrame)
            {
                Unlock();
                m_unlockedLastFrame = true;
            }
        }

		private void Update()
		{
			if (m_unlocked && !m_unlockedLastFrame)
			{
				Unlock();
				m_unlockedLastFrame = true;
			}
		}

		public virtual void Unlock()
		{
			m_unlocked = true;
			m_button.interactable = true;
			UpdateUI();
			if (m_storyModeManager)
				m_storyModeManager.AddCondition(m_EndGameCondition);
			if (m_parent)
				m_parent.Unlock();
		}

		public virtual void Lock()
		{
			if (m_parent)
			{
				m_unlocked = false;
				m_button.interactable = false;
				UpdateUI();
				m_parent.Lock();
			}
		}

		public void UpdateUI()
		{
			m_image.sprite = m_activeImage;
		}
	}
}