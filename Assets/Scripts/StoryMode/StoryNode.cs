using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeatRider
{
	[RequireComponent(typeof(Image))]
	public class StoryNode : MonoBehaviour {

		Image m_image;
		CutsceneManager m_cutsceneManager;
		StoryModeManager m_storyModeManager;
		public Sprite m_activeImage;
		public Sprite m_inactiveImage;
		public StoryNode m_parent;
		public Cutscene m_cutsceneToPlay;
		public EndGameCondition m_EndGameCondition;
		public bool m_unlocked = false;
		bool m_unlockedLastFrame = false;

		public virtual void Select()
		{
			m_cutsceneManager.PlayCutscene(m_cutsceneToPlay);
			m_storyModeManager.InitializeConditions(this);
		}

		public void Awake()
		{
			m_cutsceneManager = FindObjectOfType<CutsceneManager>();
			m_image = GetComponent<Image>();
			m_image.sprite = m_inactiveImage;
			if (m_parent == null)
				Unlock();
			else
				m_image.sprite = m_inactiveImage;
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
			UpdateUI();
			m_storyModeManager.AddCondition(m_EndGameCondition);
			if (m_parent)
				m_parent.Unlock();
		}

		public void UpdateUI()
		{
			m_image.sprite = m_activeImage;
		}
	}
}