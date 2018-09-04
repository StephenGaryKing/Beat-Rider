using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MusicalGameplayMechanics;

/// <summary>
/// Manages playing cutscenes
/// </summary>
public class CutsceneManager : MonoBehaviour {

	public Text m_nameBox;				// text box for the name of whoever is talking
	public Text m_conversationBox;		// text box for the substance of what someone is saying

	SongController m_songController;	// used to play the specified song after the cutscene is over (if any)
	int m_speachBubbleNumber = 0;		// the current line to write

	private void Start()
	{
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
		m_speachBubbleNumber = 0;
		StartCoroutine(StartCutscene(cutscene));
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
		if (cs.m_songToPlay != "")
			m_songController.OpenSoundFileByName(cs.m_songToPlay);
		
		// make the conversation visible
		m_conversationBox.transform.parent.parent.gameObject.SetActive(true);
		// while the conversation is still going
		while (m_speachBubbleNumber < cs.m_conversation.Length)
		{
			m_nameBox.text = cs.m_conversation[m_speachBubbleNumber].Name + ":";
			m_conversationBox.text = cs.m_conversation[m_speachBubbleNumber].Content;
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
		if (cs.m_songToPlay != "")
		{
			// if the song is not loading or analysing still
			while (m_songController.m_bgThread != null)
				yield return null;
			// play it
			m_songController.PlayAudio();
		}
		// hide the conversation
		m_conversationBox.transform.parent.parent.gameObject.SetActive(false);
		// un pause the game (if it was not paused in the first place this wont matter)
		Time.timeScale = 1;
	}
}
