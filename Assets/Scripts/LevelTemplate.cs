using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	enum LevelType
	{
		GRID,
		RANDOM
	}

	[System.Serializable]
	struct SceneryElement
	{
		public float spawnChanceWeight;
		public Vector2 randomScaleVariance;
	}

	[System.Serializable]
	struct TrackElement
	{
		public float spawnChanceWeight;
	}

	[CreateAssetMenu(fileName = "LevelTemplate", menuName = "Beat Rider/LevelTemplate", order = 1)]
	public class LevelTemplate : MonoBehaviour
	{
		LevelType m_levelGenerationType;
		float m_unitSize;
		int m_numOfSceneryLayers;
		List<SceneryElement> m_sceneryElements = new List<SceneryElement>();
		List<TrackElement> m_trackElements = new List<TrackElement>();
		[Tooltip("Bumber of units in front of the player to spawn the level")]
		int m_unitsAwayToSpawn = 10;
		[Tooltip("The time taken for the level to reach the player in seconds")]
		float m_travelTime = 2;
	}
}