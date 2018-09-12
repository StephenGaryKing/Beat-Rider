using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	[CreateAssetMenu(fileName = "Level", menuName = "Beat Rider/Level", order = 1)]
	public class Level : ScriptableObject
	{
		public LevelTemplate m_levelTemplate;
		public AudioClip m_song;
		public Cutscene m_endCutscene;
	}
}