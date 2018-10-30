using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	public GameObject m_tutorialGameObject;

	GameObject[] m_notes;       // must be replaced when reinitilized

	private void Start()
	{
		m_tutorialGameObject.SetActive(false);
	}

	void Initialize()
	{
		m_tutorialGameObject.SetActive(true);
		m_tutorialGameObject.transform.position = Vector3.zero;
		foreach (GameObject n in m_notes)
			n.SetActive(true);
	}

	public void StartTutorial()
	{
		Initialize();
	}
	public void EndTutorial()
	{
		m_tutorialGameObject.SetActive(false);
	}
}
