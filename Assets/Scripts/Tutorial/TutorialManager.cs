using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	public GameObject m_tutorialPrefab;
	GameObject m_currentTutorial;

	public void StartTutorial()
	{
		if (m_currentTutorial)
			Destroy(m_currentTutorial);
		m_currentTutorial = Instantiate(m_tutorialPrefab);
	}
	public void EndTutorial()
	{
		if (m_currentTutorial)
			Destroy(m_currentTutorial);
	}
}
