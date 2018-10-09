using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	/// <summary>
	/// Used for weighted slelection based on the roulett wheel algorithm
	/// </summary>
	class RouletteWheel
	{
		List<KeyValuePair<SceneryElement, Vector2>> m_wheel = new List<KeyValuePair<SceneryElement, Vector2>>();	// The "wheel" to spin
		float m_sumOfWheel;																							// used to find distribution of the wheel

		/// <summary>
		/// Add a list of elements to the wheel
		/// </summary>
		/// <param name="elements">List of elements to add to the wheel</param>
		public void Init(List<SceneryElement> elements)
		{
			foreach (var el in elements)
				AddToWheel(el);
		}

		/// <summary>
		/// add an element to the wheel
		/// </summary>
		/// <param name="el">element to add</param>
		public void AddToWheel(SceneryElement el)
		{
			// set the start and end values to look for on this element of the wheel (the final sum of wheel / this range will give the percentage chance to spawn)
			Vector2 val = new Vector2(m_sumOfWheel, m_sumOfWheel += el.spawnChanceWeight);
			m_wheel.Add(new KeyValuePair<SceneryElement, Vector2>(el, val));
			// add a bit to stop overlap
			//m_sumOfWheel++;
		}

		public SceneryElement Spin()
		{
			// perform roulett wheel selection, correcting for the addition beyond true sum of wheel
			float ran = Random.Range(0, m_sumOfWheel-1);
			// find where the wheel landed
			foreach(var slice in m_wheel)
			{
				if (slice.Value.x <= ran && slice.Value.y >= ran)
					return slice.Key;
			}

			Debug.LogError("Roulette Wheel failed when selecting an item. Tell Steve ASAP\n" + "Item Number Was " + ran);
			return m_wheel[0].Key;
		}
	}

	/// <summary>
	/// Generator for the level, uses a tamplate to create a level
	/// </summary>
	public class LevelGenerator : MonoBehaviour
	{
		[Tooltip("A scriptable object that determines the layout of a level")]
		public LevelTemplate m_levelTemplate;
		
		public ObjectSpawner[] m_objectSpawners;

		List<GameObject> m_activeSceneryElements = new List<GameObject>();
		List<GameObject> m_inactiveSceneryElements = new List<GameObject>();

		GameObject m_activeSceneryContainer;
		GameObject m_inactiveSceneryContainer;
		GameObject m_ground;

		int m_numberOfSceneryElements;
		float m_spawnInterval = 0;

		float m_timer = 0;

		private void OnDrawGizmos()
		{
			if (m_levelTemplate.m_trackWidth > 0 && m_levelTemplate.m_unitSize > 0)
			{
				float unitSize = m_levelTemplate.m_unitSize;
				float halfTrackWidth = m_levelTemplate.m_trackWidth / 2;
				Vector3 depth;

				switch (m_levelTemplate.m_levelGenerationType)
				{
					// if the level is a grid
					case (LevelType.GRID):
						// find the correct number of elements of each type
						m_numberOfSceneryElements = (int)Mathf.Max(((transform.position.z / unitSize) + 2 * m_levelTemplate.m_numOfSceneryLayers) * 2, 0);

						for (int i = 1; i <= m_levelTemplate.m_numOfSceneryLayers; i++)
						{
							// draw the line of squares for the layer on either side of the track
							depth = transform.position;
							while (depth.z >= -unitSize)
							{
								// draw the scenery squares
								Gizmos.color = Color.red;
								Gizmos.DrawWireCube(depth + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.right * halfTrackWidth - Vector3.right * unitSize / 2 + Vector3.right * unitSize * i, new Vector3(unitSize, 0, unitSize));
								Gizmos.DrawWireCube(depth + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.left * halfTrackWidth - Vector3.left * unitSize / 2 + Vector3.left * unitSize * i, new Vector3(unitSize, 0, unitSize));

								depth.z -= m_levelTemplate.m_unitSize;
							}

							// draw the track square
							Gizmos.color = Color.green;
							Gizmos.DrawWireCube(Vector3.up * m_levelTemplate.m_spawnHeightOffset + transform.position / 2, new Vector3(halfTrackWidth * 2, 0, transform.position.z));
						}
						// draw the rough number of track pieces and scenery elemts to be spawned at any one point in time
						GizmosUtils.DrawText(GUI.skin, m_numberOfSceneryElements.ToString(), transform.position + Vector3.forward * unitSize / 2 + Vector3.up * 50, Color.red, 20, 0.5f);
						break;

					//if the level is random
					case (LevelType.RANDOM):
						// draw the line for the random spawning either side of the track
						Gizmos.color = Color.red;
						Gizmos.DrawWireCube(transform.position + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.right * unitSize / 2 + Vector3.right * halfTrackWidth, new Vector3(unitSize, 0, 0));
						Gizmos.DrawWireCube(transform.position + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.left * unitSize / 2 + Vector3.left * halfTrackWidth, new Vector3(unitSize, 0, 0));

						// draw the track square
						Gizmos.color = Color.green;
						Gizmos.DrawWireCube(Vector3.up * m_levelTemplate.m_spawnHeightOffset + transform.position / 2, new Vector3(halfTrackWidth * 2, 0, transform.position.z));

						// draw the rough number of track pieces and scenery elemts to be spawned at any one point in time
						GizmosUtils.DrawText(GUI.skin, m_numberOfSceneryElements.ToString(), transform.position + Vector3.forward * unitSize / 2 + Vector3.up * 50, Color.red, 20, 0.5f);
						//GizmosUtils.DrawText(GUI.skin, m_numberOfTrackElements.ToString(), transform.position + Vector3.forward * halfTrackWidth + Vector3.up * 100, Color.green, 20, 0.5f);
						break;
					
					// If the level is Centered
					case (LevelType.CENTERED):
						// find the correct number of elements of each type
						m_numberOfSceneryElements = (int)Mathf.Max(((transform.position.z / unitSize) + 2 * m_levelTemplate.m_numOfSceneryLayers), 0);

						// draw the line of squares for the layer on either side of the track
						depth = transform.position;
						while (depth.z >= -unitSize)
						{
							// draw the scenery squares
							Gizmos.color = Color.red;
							Gizmos.DrawWireCube(depth + Vector3.up * m_levelTemplate.m_spawnHeightOffset, new Vector3(unitSize, 0, unitSize));

							depth.z -= m_levelTemplate.m_unitSize;
						}

						// draw the track square
						Gizmos.color = Color.green;
						Gizmos.DrawWireCube(Vector3.up * m_levelTemplate.m_spawnHeightOffset + transform.position / 2, new Vector3(halfTrackWidth * 2, 0, transform.position.z));

						// draw the rough number of track pieces and scenery elemts to be spawned at any one point in time
						GizmosUtils.DrawText(GUI.skin, m_numberOfSceneryElements.ToString(), transform.position + Vector3.forward * unitSize / 2 + Vector3.up * 50, Color.red, 20, 0.5f);
						break;
				}
			}
		}

		// Use this for initialization
		void Start()
		{
			SetupLevel();
		}

		public void ChangeLevel(LevelTemplate newTemplate)
		{
			Debug.Log("new Level");
			m_levelTemplate = newTemplate;
			ClearOldLevel();
			SetupLevel();
		}

		void SetupLevel()
		{
			// place the ground plane
			if (m_levelTemplate.m_groundPrefab)
			{
				m_ground = Instantiate(m_levelTemplate.m_groundPrefab);
				m_ground.transform.position = Vector3.zero + Vector3.up * m_levelTemplate.m_spawnHeightOffset;
			}

			// find the speed of the ingame elements based on the distance to cover and the time to take
			float sceneSpeed = transform.position.z / m_levelTemplate.m_travelTime;
			float unitSize = m_levelTemplate.m_unitSize;
			// update object spawners
			foreach (var spawner in m_objectSpawners)
			{
				// update position
				for (int i = 0; i < spawner.m_spawningAreas.Length; i++)
					spawner.m_spawningAreas[i].m_centerPosition.z = transform.position.z;
				// Update object speeds
				spawner.SetObjectSpeed(sceneSpeed, m_levelTemplate.m_travelTime);
			}

			// correct the spawning interval
			// find the correct number of elements of each type
			switch (m_levelTemplate.m_levelGenerationType)
			{
				case (LevelType.GRID):
					m_numberOfSceneryElements = Mathf.Max(((int)Mathf.Max((transform.position.z / unitSize) + 2, 0) * m_levelTemplate.m_numOfSceneryLayers) * 2, 0);
					m_spawnInterval = unitSize / sceneSpeed;
					break;
				case (LevelType.RANDOM):
					m_numberOfSceneryElements = Mathf.Max(((int)Mathf.Max((transform.position.z / unitSize) + 2, 0) * m_levelTemplate.m_numOfSceneryLayers), 0);
					m_spawnInterval = 0.1f / m_levelTemplate.m_numOfSceneryLayers;
					break;
				case (LevelType.CENTERED):
					m_numberOfSceneryElements = Mathf.Max((int)Mathf.Max((transform.position.z / unitSize) + 2, 0), 0);
					m_spawnInterval = unitSize / sceneSpeed;
					break;
			}

			// create a container for scenery elements
			m_activeSceneryContainer = new GameObject("Active Scenery");
			m_inactiveSceneryContainer = new GameObject("Inactive Scenery");


			// create a pool of objects
			CreatePool();

			// spawn the whole level at once to avoid the cascade of scenery elements
			switch (m_levelTemplate.m_levelGenerationType)
			{
				case (LevelType.GRID):
					for (int i = 0; i < m_numberOfSceneryElements / (m_levelTemplate.m_numOfSceneryLayers); i++)
						CreateLayerOfLevel(i);
					break;
				case (LevelType.RANDOM):
					for (int i = 0; i < m_numberOfSceneryElements / (m_levelTemplate.m_numOfSceneryLayers); i++)
						CreateLayerOfLevel(i);
					break;
				case (LevelType.CENTERED):
					for (int i = 0; i < m_numberOfSceneryElements / (m_levelTemplate.m_numOfSceneryLayers); i++)
						CreateLayerOfLevel(i);
					break;
			}
		}

		void ClearOldLevel()
		{
			WipeObjects();
			WipeScenery();
			Destroy(m_ground);
			Destroy(m_inactiveSceneryContainer);
			Destroy(m_activeSceneryContainer);
			m_activeSceneryElements.Clear();
			m_inactiveSceneryElements.Clear();
		}

		/// <summary>
		/// Creates a pool of obects to be used in level generation. Stops constant Instantiation
		/// </summary>
		void CreatePool()
		{
			//add the values together then pick a random value via roulette wheels selection
			RouletteWheel wheel = new RouletteWheel();
			wheel.Init(m_levelTemplate.m_sceneryElements);

			float elementsMultiplier = 0;
			// Give some wiggle room for spawning objects
			switch (m_levelTemplate.m_levelGenerationType)
			{
				case (LevelType.GRID) :
					elementsMultiplier = 4;
					break;

				case (LevelType.RANDOM):
					elementsMultiplier = 100;
					break;

				case (LevelType.CENTERED):
					elementsMultiplier = 2;
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
				if (m_levelTemplate.m_levelGenerationType == LevelType.RANDOM)
					go.transform.localScale = Vector3.one * Random.Range(element.randomScaleVariance.x, element.randomScaleVariance.y);
				else
					go.transform.localScale = Vector3.one * m_levelTemplate.m_unitSize * Random.Range(element.randomScaleVariance.x, element.randomScaleVariance.y);
				go.transform.position = Vector3.zero;
				SceneryModifier[] sm = go.GetComponents<SceneryModifier>();
				if (sm.Length != 0)
				{
					foreach (var mod in sm)
						mod.CreatePool();
				}
				m_inactiveSceneryElements.Add(go);
			}
		}

		public void WipeObjects()
		{
			foreach (ObjectSpawner spawner in m_objectSpawners)
				spawner.WipeObjects();
		}

		public void WipeScenery()
		{
			for(int i = 0; i < m_activeSceneryContainer.transform.childCount; i ++)
				RemoveLayerOfLevel(m_activeSceneryContainer.transform.GetChild(0));
		}

		/// <summary>
		/// remove a layer of the level to be used later
		/// </summary>
		/// <param name="container"></param>
		public void RemoveLayerOfLevel(Transform container)
		{
			// Unpack container and add parts back to the pool
			while (container.childCount > 0)
			{
				Transform child = container.GetChild(container.childCount - 1);
				child.parent = m_inactiveSceneryContainer.transform;
				child.gameObject.SetActive(false);
				child.position = Vector3.zero;
				m_inactiveSceneryElements.Add(child.gameObject);
			}
			Destroy(container.parent.gameObject);
		}

		/// <summary>
		/// create a layer of the level from the pooled objects
		/// </summary>
		/// <param name="layerNum">Layer to create the materials at</param>
		void CreateLayerOfLevel(int layerNum)
		{
			if (m_inactiveSceneryContainer)
			{
				if (m_levelTemplate.m_trackWidth > 0)
				{
					// Make a layer of the level using the objects in the pools
					float unitSize = m_levelTemplate.m_unitSize;
					float halfTrackWidth = m_levelTemplate.m_trackWidth / 2;

					GameObject layerContainer = new GameObject("Layer Container : " + Time.time + " : " + layerNum);
					GameObject sceneryContainer = new GameObject("Scenery Container");

					int ran;
					GameObject go;

					switch (m_levelTemplate.m_levelGenerationType)
					{
						case (LevelType.GRID):
							//for each scenery layer, create a piece of scenery
							if (m_inactiveSceneryElements.Count == 0)
								Debug.LogError("Pool of scenery objects is empty! Tell Steve ASAP!");
							for (int i = 1; i <= m_levelTemplate.m_numOfSceneryLayers; i++)
							{
								// right side
								ran = Random.Range(0, m_inactiveSceneryElements.Count);
								go = m_inactiveSceneryElements[ran];
								m_activeSceneryElements.Add(go);
								m_inactiveSceneryElements.RemoveAt(ran);
								go.transform.position = transform.position + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * unitSize) + Vector3.right * halfTrackWidth - Vector3.right * unitSize / 2 + Vector3.right * unitSize * i;
								go.transform.parent = sceneryContainer.transform;
								SceneryModifier[] smR = go.GetComponents<SceneryModifier>();
								if (smR.Length != 0)
								{
									foreach (var mod in smR)
									{
										mod.ResetScenery();
										mod.ModifyScenery();
									}
								}
								go.SetActive(true);

								// left side
								ran = Random.Range(0, m_inactiveSceneryElements.Count);
								go = m_inactiveSceneryElements[ran];
								m_activeSceneryElements.Add(go);
								m_inactiveSceneryElements.RemoveAt(ran);
								go.transform.position = transform.position + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * unitSize) + Vector3.left * halfTrackWidth - Vector3.left * unitSize / 2 + Vector3.left * unitSize * i;
								go.transform.parent = sceneryContainer.transform;
								SceneryModifier[] smL = go.GetComponents<SceneryModifier>();
								if (smL.Length != 0)
								{
									foreach (var mod in smL)
									{
										mod.ResetScenery();
										mod.ModifyScenery();
									}
								}
								go.SetActive(true);
							}

							sceneryContainer.transform.parent = layerContainer.transform;
							layerContainer.transform.parent = m_activeSceneryContainer.transform;

							break;
						case (LevelType.RANDOM):
							if (m_inactiveSceneryElements.Count == 0)
								Debug.LogError("Pool of scenery objects is empty! Tell Steve ASAP!");
							for (int i = 1; i <= m_levelTemplate.m_numOfSceneryLayers; i++)
							{
								// right side
								ran = Random.Range(0, m_inactiveSceneryElements.Count);
								go = m_inactiveSceneryElements[ran];
								m_activeSceneryElements.Add(go);
								m_inactiveSceneryElements.RemoveAt(ran);
								go.transform.position = transform.position + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * unitSize) + Vector3.right * halfTrackWidth + Vector3.right * Random.Range(0, unitSize);
								go.transform.parent = sceneryContainer.transform;
								SceneryModifier[] smR = go.GetComponents<SceneryModifier>();
								if (smR.Length != 0)
								{
									foreach (var mod in smR)
									{
										mod.ResetScenery();
										mod.ModifyScenery();
									}
								}
								go.SetActive(true);

								// left side
								ran = Random.Range(0, m_inactiveSceneryElements.Count);
								go = m_inactiveSceneryElements[ran];
								m_activeSceneryElements.Add(go);
								m_inactiveSceneryElements.RemoveAt(ran);
								go.transform.position = transform.position + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * unitSize) + Vector3.left * halfTrackWidth + Vector3.left * Random.Range(0, unitSize);
								go.transform.parent = sceneryContainer.transform;
								SceneryModifier[] smL = go.GetComponents<SceneryModifier>();
								if (smL.Length != 0)
								{
									foreach (var mod in smL)
									{
										mod.ResetScenery();
										mod.ModifyScenery();
									}
								}
								go.SetActive(true);
							}

							sceneryContainer.transform.parent = layerContainer.transform;
							layerContainer.transform.parent = m_activeSceneryContainer.transform;

							break;

						case (LevelType.CENTERED):
							//for each scenery layer, create a piece of scenery
							if (m_inactiveSceneryElements.Count == 0)
								Debug.LogError("Pool of scenery objects is empty! Tell Steve ASAP!");

							// middle
							ran = Random.Range(0, m_inactiveSceneryElements.Count);
							go = m_inactiveSceneryElements[ran];
							m_activeSceneryElements.Add(go);
							m_inactiveSceneryElements.RemoveAt(ran);
							go.transform.position = transform.position + Vector3.up * m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * unitSize);
							go.transform.parent = sceneryContainer.transform;
							SceneryModifier[] sm = go.GetComponents<SceneryModifier>();
							if (sm.Length != 0)
							{
								foreach (var mod in sm)
								{
									mod.ResetScenery();
									mod.ModifyScenery();
								}
							}
							go.SetActive(true);

							sceneryContainer.transform.parent = layerContainer.transform;
							layerContainer.transform.parent = m_activeSceneryContainer.transform;

							break;
					}

					// add the lane movement component to the lane and set its variables (moving one object is cheeper than mooving all individualy)
					LaneMovement lm = sceneryContainer.AddComponent<LaneMovement>();
					const float dieAtZ = -1000;
					//do some math to find the speed the scene must move
					lm.m_unitsToMovePerSecond = transform.position.z / m_levelTemplate.m_travelTime;
					lm.m_zValueToDie = dieAtZ;
					lm.m_levelGen = this;
				}
			}
		}

		private void FixedUpdate()
		{
			m_timer += Time.deltaTime;
			// spawn a layer of the level
			if (m_timer >= m_spawnInterval)
			{
				m_timer = 0;
				CreateLayerOfLevel(0);
			}
		}
	}
}