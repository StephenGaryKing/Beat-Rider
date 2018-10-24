using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public enum LevelType
	{
		GRID,
		RANDOM,
		CENTERED
	}

	[System.Serializable]
	public class SceneryElement
	{
		public GameObject m_prefab;
		public float spawnChanceWeight;
		public Vector2 randomScaleVariance = Vector2.one;
	}

	[CreateAssetMenu(fileName = "LevelTemplate", menuName = "Beat Rider/LevelTemplate", order = 1)]
	public class LevelTemplate : ScriptableObject
	{
		public LevelType m_levelGenerationType;
		public GameObject m_groundPrefab;
		public float m_spawnHeightOffset;
		public float m_trackWidth;
		public float m_unitSize = 1;
		public int m_numOfSceneryLayers;
		public List<SceneryElement> m_sceneryElements = new List<SceneryElement>();
		[Tooltip("The time taken for the level to reach the player in seconds")]
		public float m_travelTime = 2;
		public Material m_customPostProcess;
	}
}