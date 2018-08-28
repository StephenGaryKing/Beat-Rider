using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPausePoint : MonoBehaviour {

	public Cutscene m_cutsceneToShow;
	public KeyCode[] m_continueKeys;

	CutsceneManager m_cutsceneManager;

	private void OnTriggerEnter(Collider other)
	{
		Time.timeScale = 0;
		m_cutsceneManager.PlayCutscene(m_cutsceneToShow);

		float timeToWait = 0;
		foreach (SpeachBubble sb in m_cutsceneToShow.m_conversation)
			timeToWait += sb.waitTime;
		if (timeToWait > 0)
			Invoke("ContinueTutorial", timeToWait);
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
	void Start () {
		m_cutsceneManager = FindObjectOfType<CutsceneManager>();
	}
}
