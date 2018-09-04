using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryModeManager : MonoBehaviour {

	public List<Cutscene> m_cutscenes;
	CutsceneManager m_cutsceneManager;


	private void Start()
	{
		m_cutsceneManager = FindObjectOfType<CutsceneManager>();
	}
	public void LoadCutscene(int cutsceneNumber)
	{
		m_cutsceneManager.PlayCutscene(m_cutscenes[cutsceneNumber]);
	}
}
