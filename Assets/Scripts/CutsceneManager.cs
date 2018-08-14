using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MusicalGameplayMechanics;

public class CutsceneManager : MonoBehaviour {

	public Text m_nameBox;
	public Text m_conversationBox;

	SongController m_songController;
	int m_speachBubbleNumber = 0;

	private void Start()
	{
		m_songController = FindObjectOfType<SongController>();
	}

	public void PlayCutscene(Cutscene cutscene)
	{
		m_speachBubbleNumber = 0;
		StartCoroutine(StartCutscene(cutscene));
	}

	public void StopCutscene(Cutscene cutscene)
	{
		m_speachBubbleNumber = 9999;
	}

	public IEnumerator StartCutscene(Cutscene cs)
	{
		if (cs.m_songToPlay != "")
			m_songController.OpenSoundFileByName(cs.m_songToPlay);
			m_conversationBox.transform.parent.parent.gameObject.SetActive(true);
		while (m_speachBubbleNumber < cs.m_conversation.Length)
		{
			m_nameBox.text = cs.m_conversation[m_speachBubbleNumber].Name + ":";
			m_conversationBox.text = cs.m_conversation[m_speachBubbleNumber].Content;
			if (cs.m_conversation[m_speachBubbleNumber].waitTime == 0)
				yield return null;
			else
			{
				Debug.Log(cs.m_conversation[m_speachBubbleNumber].waitTime);
				yield return new WaitForSecondsRealtime(cs.m_conversation[m_speachBubbleNumber].waitTime);
				m_speachBubbleNumber++;
				Debug.Log("going to next spech bubble");
			}
		}
		if (cs.m_songToPlay != "")
		{
			while (m_songController.m_bgThread != null)
				yield return null;
			m_songController.PlayAudio();
		}
		m_conversationBox.transform.parent.parent.gameObject.SetActive(false);
		Time.timeScale = 1;
	}
}
