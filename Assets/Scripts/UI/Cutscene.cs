using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	[CreateAssetMenu(fileName = "Cutscene", menuName = "Beat Rider/Cutscene", order = 1)]

	public class Cutscene : ScriptableObject
	{

		public string m_cameraTagToUse;         // camera to use for the cutscene
		public SpeachBubble[] m_conversation;   // a conversation with n elements
		public Level m_levelToPlay;             // the song to play (if any) after the cutscene is over
	}
}