using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cutscene", menuName = "Beat Rider/Cutscene", order = 1)]
public class Cutscene : ScriptableObject {

	[System.Serializable]
	public struct SpeachBubble
	{
		public string Name;
		public string Content;
		public float waitTime;
	};

	public SpeachBubble[] m_conversation;
	public string m_songToPlay;
}
