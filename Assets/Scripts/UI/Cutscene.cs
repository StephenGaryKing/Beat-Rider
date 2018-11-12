using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EndGameCondition
{
	NONE,
	DeathTrigger,
	AnswerDenyPolice,
	AidPolice,
	NoGroup
}

namespace BeatRider
{
	/// <summary>
	/// used in cutscenes to display text on the screen
	/// </summary>
	[System.Serializable]
	public struct SpeachBubble
	{
		public Sprite Image;
		public Sound Audio;
		public string Name;
		public string Content;
		public float waitTime;
	};

	[System.Serializable]
	public struct Choice
	{
		public Sprite Image;
		public Cutscene CutsceneToPlay;
	}

	[CreateAssetMenu(fileName = "Cutscene", menuName = "Beat Rider/Cutscene", order = 1)]

	public class Cutscene : ScriptableObject
	{

		public string m_cameraTagToUse;         // camera to use for the cutscene
		public Choice[] m_preCutsceneChoice;		// played before cutscenes
		public SpeachBubble[] m_conversation;   // a conversation with n elements
		public Choice[] m_postCutsceneChoice;      // played before cutscenes
		public Level m_levelToPlay;             // the song to play (if any) after the cutscene is over
		public int m_nodeNumberToUnlock = 0;
	}
}