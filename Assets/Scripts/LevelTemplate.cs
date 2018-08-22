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
	public struct SceneryElement
	{
		public float spawnChanceWeight;
		public Vector2 randomScaleVariance;
		[Header("Info")]
		public float spawnChancePercentage;
	}

	[System.Serializable]
	public struct TrackElement
	{
		public float spawnChanceWeight;
		[Header("Info")]
		public float spawnChancePercentage;
	}

	[CreateAssetMenu(fileName = "LevelTemplate", menuName = "Beat Rider/LevelTemplate", order = 1)]
	public class LevelTemplate : ScriptableObject
	{
		public LevelType m_levelGenerationType;
		public float m_unitSize;
		public int m_numOfSceneryLayers;
		public List<SceneryElement> m_sceneryElements = new List<SceneryElement>();
		public List<TrackElement> m_trackElements = new List<TrackElement>();
		[Tooltip("Bumber of units in front of the player to spawn the level")]
		public int m_unitsAwayToSpawn = 10;
		[Tooltip("The time taken for the level to reach the player in seconds")]
		public float m_travelTime = 2;
	}
}