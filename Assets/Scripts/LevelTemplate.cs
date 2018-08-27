using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public enum LevelType
	{
		GRID,
		RANDOM
	}

	[System.Serializable]
	public class SceneryElement
	{
		public GameObject m_prefab;
		public float spawnChanceWeight;
		public Vector2 randomScaleVariance = Vector2.one;
		//[Header("Info")]
		//public float spawnChancePercentage;
	}

	[System.Serializable]
	public class TrackElement
	{
		public GameObject m_prefab;
		public float spawnChanceWeight;
		//[Header("Info")]
		//public float spawnChancePercentage;
	}

	[CreateAssetMenu(fileName = "LevelTemplate", menuName = "Beat Rider/LevelTemplate", order = 1)]
	public class LevelTemplate : ScriptableObject
	{
		public LevelType m_levelGenerationType;
		public float m_spawnHeightOffset;
		public float m_trackWidth;
		public float m_unitSize;
		public int m_numOfSceneryLayers;
		public List<SceneryElement> m_sceneryElements = new List<SceneryElement>();
		public List<TrackElement> m_trackElements = new List<TrackElement>();
		[Tooltip("The time taken for the level to reach the player in seconds")]
		public float m_travelTime = 2;
	}
}