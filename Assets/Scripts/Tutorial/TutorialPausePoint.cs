using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BeatRider
{
	[System.Serializable]
	public class PauseEvent : UnityEvent { }

	public class TutorialPausePoint : MonoBehaviour
	{ 
		public Cutscene m_cutsceneToShow;
		public KeyCode[] m_continueKeys;
		public float m_timescale = 0;

		[SerializeField]
		public PauseEvent onPause;

		CutsceneManager m_cutsceneManager;

		private void OnTriggerEnter(Collider other)
		{
			onPause.Invoke();
			if (m_cutsceneToShow)
			{
				Time.timeScale = m_timescale;
				m_cutsceneManager.PlayCutscene(m_cutsceneToShow);

				float timeToWait = 0;
				foreach (SpeachBubble sb in m_cutsceneToShow.m_conversation)
					timeToWait += sb.waitTime;
				if (timeToWait > 0)
					Invoke("ContinueTutorial", timeToWait);
			}
		}

		private void Update()
		{
			foreach (KeyCode key in m_continueKeys)
			{
				if (Input.GetKeyDown(key))
				{
					m_cutsceneManager.StopCutscene();
					Time.timeScale = 1;
				}
			}
		}

		// Use this for initialization
		void Awake()
		{
			m_cutsceneManager = FindObjectOfType<CutsceneManager>();
		}
	}
}