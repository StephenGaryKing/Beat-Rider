using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	class RouletteWheel
	{
		List<KeyValuePair<SceneryElement, Vector2>> m_wheel = new List<KeyValuePair<SceneryElement, Vector2>>();
		float m_sumOfWheel;

		public void Init(List<SceneryElement> elements)
		{
			foreach (var el in elements)
				AddToWheel(el);
		}

		public void AddToWheel(SceneryElement el)
		{
			Vector2 val = new Vector2(m_sumOfWheel, m_sumOfWheel += el.spawnChanceWeight);
			m_wheel.Add(new KeyValuePair<SceneryElement, Vector2>(el, val));
			m_sumOfWheel++;
		}

		public SceneryElement Spin()
		{
			float ran = Random.Range(0, m_sumOfWheel-1);
			foreach(var slice in m_wheel)
			{
				if (slice.Value.x <= ran && slice.Value.y >= ran)
					return slice.Key;
			}

			Debug.LogError("Roulette Wheel failed when selecting an item. Tell Steve ASAP");
			return m_wheel[0].Key;
		}
	}

	public class LevelGenerator : MonoBehaviour
	{
		[Tooltip("A scriptable object that determines the layout of a level")]
		[SerializeField] LevelTemplate m_levelTemplate;

		List<GameObject> m_activeSceneryElements = new List<GameObject>();
		List<GameObject> m_inactiveSceneryElements = new List<GameObject>();

		GameObject m_activeSceneryContainer;
		GameObject m_inactiveSceneryContainer;

		PickupSpawner m_PickupSpawner;
		int m_numberOfSceneryElements;
		int m_numberOfTrackElements;
		float m_spawnInterval = 0;

		float m_timer = 0;

		private void OnDrawGizmos()
		{

			float unitSize = m_levelTemplate.m_unitSize;
			float halfTrackWidth = m_levelTemplate.m_trackWidth / 2;

			// if the level is a grid
			if (m_levelTemplate.m_levelGenerationType == LevelType.GRID)
			{
				m_numberOfTrackElements = (int)Mathf.Max((transform.position.z / (halfTrackWidth * 2)) + 2, 0);
				m_numberOfSceneryElements = (int)Mathf.Max(((transform.position.z / unitSize) + 2 * m_levelTemplate.m_numOfSceneryLayers) * 2, 0);

				for (int i = 1; i <= m_levelTemplate.m_numOfSceneryLayers; i++)
				{
					// draw the line of squares for the layer on either side of the track
					Vector3 depth = transform.position;
					while (depth.z >= -unitSize)
					{
						// draw the scenery squares
						Gizmos.color = Color.red;
						Gizmos.DrawWireCube(depth + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.right * halfTrackWidth - Vector3.right * unitSize / 2 + Vector3.right * unitSize * i, new Vector3(unitSize, 0, unitSize));
						Gizmos.DrawWireCube(depth + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.left * halfTrackWidth - Vector3.left * unitSize / 2 + Vector3.left * unitSize * i, new Vector3(unitSize, 0, unitSize));


						depth.z -= m_levelTemplate.m_unitSize;
					}
					depth = transform.position;
					while (depth.z >= -halfTrackWidth*2)
					{
						// draw the track squares
						Gizmos.color = Color.green;
						Gizmos.DrawWireCube(depth, new Vector3(halfTrackWidth * 2, 0, halfTrackWidth * 2));

						depth.z -= halfTrackWidth * 2;
					}

				}
				GizmosUtils.DrawText(GUI.skin, m_numberOfSceneryElements.ToString(), transform.position + Vector3.forward * unitSize / 2 + Vector3.up * 50, Color.red, 20, 0.5f);
				GizmosUtils.DrawText(GUI.skin, m_numberOfTrackElements.ToString(), transform.position + Vector3.forward * unitSize / 2 + Vector3.up * 100, Color.green, 20, 0.5f);
			}

			//if the level is random
			if (m_levelTemplate.m_levelGenerationType == LevelType.RANDOM)
			{
				m_numberOfTrackElements = (int)Mathf.Max((transform.position.z / (halfTrackWidth * 2)) + 2, 0);

				// draw the line for the random spawning either side of the track
				Gizmos.color = Color.red;
				Gizmos.DrawWireCube(transform.position + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.right * unitSize/2, new Vector3(unitSize, 0, 0));
				Gizmos.DrawWireCube(transform.position + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.left * unitSize/2, new Vector3(unitSize, 0, 0));

				Vector3 depth = transform.position;
				while (depth.z >= -(halfTrackWidth * 2))
				{
					// draw the track squares
					Gizmos.color = Color.green;
					Gizmos.DrawWireCube(depth, new Vector3(halfTrackWidth * 2, 0, halfTrackWidth * 2));

					depth.z -= halfTrackWidth * 2;
				}
				GizmosUtils.DrawText(GUI.skin, m_numberOfSceneryElements.ToString(), transform.position + Vector3.forward * unitSize / 2 + Vector3.up * 50, Color.red, 20, 0.5f);
				GizmosUtils.DrawText(GUI.skin, m_numberOfTrackElements.ToString(), transform.position + Vector3.forward * halfTrackWidth + Vector3.up * 100, Color.green, 20, 0.5f);
			}
		}

		// Use this for initialization
		void Start()
		{
			float sceneSpeed = transform.position.z / m_levelTemplate.m_travelTime;
			float unitSize = m_levelTemplate.m_unitSize;

			switch (m_levelTemplate.m_levelGenerationType)
			{
				case (LevelType.GRID):
					m_spawnInterval = unitSize / sceneSpeed;
					break;
				case (LevelType.RANDOM):
					m_spawnInterval = 0.1f;
					break;
			}

			m_activeSceneryContainer = new GameObject("Active Scenery");
			m_inactiveSceneryContainer = new GameObject("Inactive Scenery");

			m_numberOfTrackElements = (int)Mathf.Max((transform.position.z / unitSize) + 2, 0);
			m_numberOfSceneryElements = Mathf.Max((m_numberOfTrackElements * m_levelTemplate.m_numOfSceneryLayers) * 2, 0);

			m_PickupSpawner = FindObjectOfType<PickupSpawner>();
			CreatePool();

			for (int i = 0; i < m_numberOfSceneryElements / (m_levelTemplate.m_numOfSceneryLayers * 2); i ++)
				CreateLayerOfLevel(i);
		}

		void CreatePool()
		{
			//add the values together then pick a random value via roulette wheels selection
			RouletteWheel wheel = new RouletteWheel();
			wheel.Init(m_levelTemplate.m_sceneryElements);

			float elementsMultiplier = 0;
			switch (m_levelTemplate.m_levelGenerationType)
			{
				case (LevelType.GRID) :
					elementsMultiplier = 4;
					break;

				case (LevelType.RANDOM):
					elementsMultiplier = 10;
					break;
			}

			// Make a pool of objects to make each layer out of
			for (int i = 0; i < m_numberOfSceneryElements * elementsMultiplier; i ++)
			{
				SceneryElement element = wheel.Spin();
				GameObject go = Instantiate(element.m_prefab, null);
				go.name = element.m_prefab.name + " : " + i;
				go.SetActive(false);
				go.transform.parent = m_inactiveSceneryContainer.transform;
				go.transform.localScale = Vector3.one * m_levelTemplate.m_unitSize * Random.Range(element.randomScaleVariance.x, element.randomScaleVariance.y);
				go.transform.position = Vector3.zero;
				m_inactiveSceneryElements.Add(go);
			}
		}

		public void RemoveLayerOfLevel(Transform container)
		{
			// Unpack container and add parts back to the pool
			while (container.childCount > 0)
			{
				Transform child = container.GetChild(container.childCount - 1);
				child.parent = null;
				child.gameObject.SetActive(false);
				child.position = Vector3.zero;
				m_inactiveSceneryElements.Add(child.gameObject);
			}
			Destroy(container.parent.gameObject);
		}

		void CreateLayerOfLevel(int layerNum)
		{
			// Make a layer of the level using the objects in the pools
			float unitSize = m_levelTemplate.m_unitSize;
			float halfTrackWidth = m_levelTemplate.m_trackWidth / 2;

			GameObject layerContainer = new GameObject("Layer Container : " + Time.time + " : " + layerNum);
			GameObject sceneryContainer = new GameObject("Scenery Container");

			switch (m_levelTemplate.m_levelGenerationType)
			{
				case (LevelType.GRID):

					//create Track piece
					//foreach(var track in m_levelTemplate.m_trackElements)
					//{


					//}
					//for each scenery layer, create a piece of scenery

					for (int i = 1; i <= m_levelTemplate.m_numOfSceneryLayers; i ++)
					{
						int ran;
						GameObject go;

						// right side
						ran = Random.Range(0, m_inactiveSceneryElements.Count);
						go = m_inactiveSceneryElements[ran];
						m_activeSceneryElements.Add(go);
						m_inactiveSceneryElements.Remove(go);
						go.transform.position = transform.position + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * unitSize) + Vector3.right * halfTrackWidth - Vector3.right * unitSize / 2 + Vector3.right * unitSize * i;
						go.transform.parent = sceneryContainer.transform;
						go.SetActive(true);

						// left side
						ran = Random.Range(0, m_inactiveSceneryElements.Count);
						go = m_inactiveSceneryElements[ran];
						m_activeSceneryElements.Add(go);
						m_inactiveSceneryElements.Remove(go);
						go.transform.position = transform.position + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * unitSize) + Vector3.left * halfTrackWidth - Vector3.left * unitSize / 2 + Vector3.left * unitSize * i;
						go.transform.parent = sceneryContainer.transform;
						go.SetActive(true);
					}

					sceneryContainer.transform.parent = layerContainer.transform;
					layerContainer.transform.parent = m_activeSceneryContainer.transform;

					break;
				case (LevelType.RANDOM):

					//create Track piece
					//foreach(var track in m_levelTemplate.m_trackElements)
					//{


					//}
					//for each scenery layer, create a piece of scenery

					for (int i = 1; i <= m_levelTemplate.m_numOfSceneryLayers; i++)
					{
						int ran;
						GameObject go;

						// right side
						ran = Random.Range(0, m_inactiveSceneryElements.Count);
						go = m_inactiveSceneryElements[ran];
						m_activeSceneryElements.Add(go);
						m_inactiveSceneryElements.Remove(go);
						go.transform.position = transform.position + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * unitSize) + Vector3.right * halfTrackWidth - Vector3.right * Random.Range(0, unitSize);
						go.transform.parent = sceneryContainer.transform;
						go.SetActive(true);

						// left side
						ran = Random.Range(0, m_inactiveSceneryElements.Count);
						go = m_inactiveSceneryElements[ran];
						m_activeSceneryElements.Add(go);
						m_inactiveSceneryElements.Remove(go);
						go.transform.position = transform.position + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * unitSize) + Vector3.left * halfTrackWidth - Vector3.left * Random.Range(0, unitSize);
						go.transform.parent = sceneryContainer.transform;
						go.SetActive(true);
					}

					sceneryContainer.transform.parent = layerContainer.transform;
					layerContainer.transform.parent = m_activeSceneryContainer.transform;

					break;
			}

			LaneMovement lm = sceneryContainer.AddComponent<LaneMovement>();
			const float dieAtZ = -100;
			//do some math to find the speed the scene must move
			lm.m_unitsToMovePerSecond = transform.position.z / m_levelTemplate.m_travelTime;
			lm.m_zValueToDie = dieAtZ;
			lm.m_levelGen = this;
		}

		private void FixedUpdate()
		{
			m_timer += Time.deltaTime;
			if (m_timer >= m_spawnInterval)
			{
				m_timer = 0;
				CreateLayerOfLevel(0);
			}
		}
	}
}