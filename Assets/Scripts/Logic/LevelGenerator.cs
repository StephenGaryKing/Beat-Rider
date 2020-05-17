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
		public Level m_currentLevel;
		
		public ObjectSpawner[] m_objectSpawners;

		List<GameObject> m_activeSceneryElements = new List<GameObject>();
		List<GameObject> m_inactiveSceneryElements = new List<GameObject>();

		GameObject m_activeSceneryContainer;
		GameObject m_inactiveSceneryContainer;
		GameObject m_tempActiveSceneryContainer;
		GameObject m_tempInactiveSceneryContainer;

		GameObject m_ground;

		int m_numberOfSceneryElements;
		float m_spawnInterval = 0;

		float m_timer = 0;

		private void OnDrawGizmos()
		{
			if (m_currentLevel.m_levelTemplate.m_trackWidth > 0 && m_currentLevel.m_levelTemplate.m_unitSize > 0)
			{
				float unitSize = m_currentLevel.m_levelTemplate.m_unitSize;
				float halfTrackWidth = m_currentLevel.m_levelTemplate.m_trackWidth / 2;
				Vector3 depth;

				switch (m_currentLevel.m_levelTemplate.m_levelGenerationType)
				{
					// if the level is a grid
					case (LevelType.GRID):
						// find the correct number of elements of each type
						m_numberOfSceneryElements = (int)Mathf.Max(((transform.position.z / unitSize) + 2 * m_currentLevel.m_levelTemplate.m_numOfSceneryLayers) * 2, 0);

						for (int i = 1; i <= m_currentLevel.m_levelTemplate.m_numOfSceneryLayers; i++)
						{
							// draw the line of squares for the layer on either side of the track
							depth = transform.position;
							while (depth.z >= -unitSize)
							{
								// draw the scenery squares
								Gizmos.color = Color.red;
								Gizmos.DrawWireCube(depth + Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset + Vector3.right * halfTrackWidth - Vector3.right * unitSize / 2 + Vector3.right * unitSize * i, new Vector3(unitSize, 0, unitSize));
								Gizmos.DrawWireCube(depth + Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset + Vector3.left * halfTrackWidth - Vector3.left * unitSize / 2 + Vector3.left * unitSize * i, new Vector3(unitSize, 0, unitSize));

								depth.z -= m_currentLevel.m_levelTemplate.m_unitSize;
							}

							// draw the track square
							Gizmos.color = Color.green;
							Gizmos.DrawWireCube(Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset + transform.position / 2, new Vector3(halfTrackWidth * 2, 0, transform.position.z));
						}
						// draw the rough number of track pieces and scenery elemts to be spawned at any one point in time
						GizmosUtils.DrawText(GUI.skin, m_numberOfSceneryElements.ToString(), transform.position + Vector3.forward * unitSize / 2 + Vector3.up * 50, Color.red, 20, 0.5f);
						break;

					//if the level is random
					case (LevelType.RANDOM):
						// draw the line for the random spawning either side of the track
						Gizmos.color = Color.red;
						Gizmos.DrawWireCube(transform.position + Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset + Vector3.right * unitSize / 2 + Vector3.right * halfTrackWidth, new Vector3(unitSize, 0, 0));
						Gizmos.DrawWireCube(transform.position + Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset + Vector3.left * unitSize / 2 + Vector3.left * halfTrackWidth, new Vector3(unitSize, 0, 0));

						// draw the track square
						Gizmos.color = Color.green;
						Gizmos.DrawWireCube(Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset + transform.position / 2, new Vector3(halfTrackWidth * 2, 0, transform.position.z));

						// draw the rough number of track pieces and scenery elemts to be spawned at any one point in time
						GizmosUtils.DrawText(GUI.skin, m_numberOfSceneryElements.ToString(), transform.position + Vector3.forward * unitSize / 2 + Vector3.up * 50, Color.red, 20, 0.5f);
						//GizmosUtils.DrawText(GUI.skin, m_numberOfTrackElements.ToString(), transform.position + Vector3.forward * halfTrackWidth + Vector3.up * 100, Color.green, 20, 0.5f);
						break;
					
					// If the level is Centered
					case (LevelType.CENTERED):
						// find the correct number of elements of each type
						m_numberOfSceneryElements = (int)Mathf.Max(((transform.position.z / unitSize) + 2 * m_currentLevel.m_levelTemplate.m_numOfSceneryLayers), 0);

						// draw the line of squares for the layer on either side of the track
						depth = transform.position;
						while (depth.z >= -unitSize)
						{
							// draw the scenery squares
							Gizmos.color = Color.red;
							Gizmos.DrawWireCube(depth + Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset, new Vector3(unitSize, 0, unitSize));

							depth.z -= m_currentLevel.m_levelTemplate.m_unitSize;
						}

						// draw the track square
						Gizmos.color = Color.green;
						Gizmos.DrawWireCube(Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset + transform.position / 2, new Vector3(halfTrackWidth * 2, 0, transform.position.z));

						// draw the rough number of track pieces and scenery elemts to be spawned at any one point in time
						GizmosUtils.DrawText(GUI.skin, m_numberOfSceneryElements.ToString(), transform.position + Vector3.forward * unitSize / 2 + Vector3.up * 50, Color.red, 20, 0.5f);
						break;
				}
			}
		}

		// Use this for initialization
		void Start()
		{
			StartCoroutine(SetupLevelAsync());
		}

		private void Update()
		{
			if (s_levelHidden && (m_activeSceneryContainer.activeInHierarchy || m_inactiveSceneryContainer.activeInHierarchy))
			{
				m_activeSceneryContainer.SetActive(false);
				m_inactiveSceneryContainer.SetActive(false);
			}
			if (!s_levelHidden && (!m_activeSceneryContainer.activeInHierarchy || !m_inactiveSceneryContainer.activeInHierarchy))
			{
				m_activeSceneryContainer.SetActive(true);
				m_inactiveSceneryContainer.SetActive(true);
			}
		}

		static bool s_levelHidden = false;
		public static void HideLevel()
		{
			s_levelHidden = true;
		}

		public static void ShowLevel()
		{
			s_levelHidden = false;
		}

		LevelTemplate m_oldlevelTemplate;
		private void LateUpdate()
		{
            if (!m_oldlevelTemplate)
                m_oldlevelTemplate = m_currentLevel.m_levelTemplate;
            if (m_oldlevelTemplate != m_currentLevel.m_levelTemplate)
            {
                ChangeLevel(m_currentLevel);
                m_oldlevelTemplate = m_currentLevel.m_levelTemplate;
            }
		}

		public void ChangeLevel(Level newlevel)
		{
            StartCoroutine(ChangeLevelAsync(newlevel));
		}

		IEnumerator ChangeLevelAsync(Level newLevel)
		{
			Debug.Log("Change level Async");
			m_currentLevel = newLevel;
			StartCoroutine(ClearOldLevelAsync());
			// wait till clear, setup new level
			while (!c_levelClear)
				yield return null;
			StartCoroutine(SetupLevelAsync());
			Destroy(m_tempActiveSceneryContainer);
			Destroy(m_tempInactiveSceneryContainer);
		}

		private void OnApplicationQuit()
		{
			// Disable custom post effect
			if (m_currentLevel.m_levelTemplate.m_customPostProcess)
				m_currentLevel.m_levelTemplate.m_customPostProcess.SetFloat("_IsEnabled", 0);
		}

		IEnumerator SetupLevelAsync()
		{
			Debug.Log("Setting up level Async");
			// Apply Fog
			RenderSettings.fogColor = m_currentLevel.m_levelTemplate.m_fogColour;
			RenderSettings.fogStartDistance = m_currentLevel.m_levelTemplate.m_fogStart;
			RenderSettings.fogEndDistance = m_currentLevel.m_levelTemplate.m_fogEnd;

			// Enable custom post effect
			if (m_currentLevel.m_levelTemplate.m_customPostProcess)
				m_currentLevel.m_levelTemplate.m_customPostProcess.SetFloat("_IsEnabled", 1);

			// place the ground plane
			if (m_currentLevel.m_levelTemplate.m_groundPrefab)
			{
				m_ground = Instantiate(m_currentLevel.m_levelTemplate.m_groundPrefab);
				m_ground.transform.position = Vector3.zero + Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset;
			}

			// find the speed of the ingame elements based on the distance to cover and the time to take
			float sceneSpeed = transform.position.z / m_currentLevel.m_levelTemplate.m_travelTime;
			float unitSize = m_currentLevel.m_levelTemplate.m_unitSize;
			// update object spawners
			foreach (var spawner in m_objectSpawners)
			{
				// update position
				for (int i = 0; i < spawner.m_spawningAreas.Length; i++)
					spawner.m_spawningAreas[i].m_centerPosition.z = transform.position.z;
				// Update object speeds
				spawner.SetObjectSpeed(sceneSpeed, m_currentLevel.m_levelTemplate.m_travelTime);
			}

			// correct the spawning interval
			// find the correct number of elements of each type
			switch (m_currentLevel.m_levelTemplate.m_levelGenerationType)
			{
				case (LevelType.GRID):
					m_numberOfSceneryElements = Mathf.Max(((int)Mathf.Max((transform.position.z / unitSize) + 2, 0) * m_currentLevel.m_levelTemplate.m_numOfSceneryLayers) * 2, 0);
					m_spawnInterval = unitSize / sceneSpeed;
					break;
				case (LevelType.RANDOM):
					m_spawnInterval = 1;
					unitSize = m_spawnInterval * sceneSpeed;
					m_numberOfSceneryElements = Mathf.Max((int)Mathf.Max((transform.position.z / sceneSpeed) + 2, 0), 0);
					break;
				case (LevelType.CENTERED):
					m_numberOfSceneryElements = Mathf.Max((int)Mathf.Max((transform.position.z / unitSize) + 2, 0), 0);
					m_spawnInterval = unitSize / sceneSpeed;
					break;
			}

			// create a container for scenery elements
			m_activeSceneryContainer = new GameObject("Active Scenery");
			m_inactiveSceneryContainer = new GameObject("Inactive Scenery");


			// create a pool of objects A-sync
			StartCoroutine(CreatePoolAsync());
			while (!c_poolCreated)
				yield return null;

			// spawn the whole level at once to avoid the cascade of scenery elements
			switch (m_currentLevel.m_levelTemplate.m_levelGenerationType)
			{
				case (LevelType.GRID):
					for (int i = 0; i < m_numberOfSceneryElements / (m_currentLevel.m_levelTemplate.m_numOfSceneryLayers); i++)
					{
						CreateLayerOfLevel(i);
						yield return null;
					}
					break;
				case (LevelType.RANDOM):
					for (int i = 0; i < m_numberOfSceneryElements; i++)
					{ 
						CreateLayerOfLevel(i);
						yield return null;
					}
					break;
				case (LevelType.CENTERED):
					for (int i = 0; i < m_numberOfSceneryElements / (m_currentLevel.m_levelTemplate.m_numOfSceneryLayers); i++)
					{ 
						CreateLayerOfLevel(i);
						yield return null;
					}
					break;
			}
		}

		bool c_levelClear;
		IEnumerator ClearOldLevelAsync()
		{
			Debug.Log("Clearing level Async");
			c_levelClear = false;
			// Dissable custom post effect
			if (m_oldlevelTemplate.m_customPostProcess)
				m_oldlevelTemplate.m_customPostProcess.SetFloat("_IsEnabled", 0);

			WipeObjects();
			StartCoroutine(WipeSceneryAsync());
			while (!c_sceneryClear)
				yield return null;
			Destroy(m_ground);
			m_tempActiveSceneryContainer = m_activeSceneryContainer;
			m_tempInactiveSceneryContainer = m_inactiveSceneryContainer;
			Destroy(m_inactiveSceneryContainer);
			Destroy(m_activeSceneryContainer);
			m_activeSceneryElements.Clear();
			m_inactiveSceneryElements.Clear();
			c_levelClear = true;
		}

		bool c_poolCreated;
		/// <summary>
		/// Creates a pool of obects to be used in level generation. Stops constant Instantiation
		/// </summary>
		IEnumerator CreatePoolAsync()
		{
			c_poolCreated = false;
			//add the values together then pick a random value via roulette wheels selection
			RouletteWheel wheel = new RouletteWheel();
			wheel.Init(m_currentLevel.m_levelTemplate.m_sceneryElements);

			float elementsMultiplier = 0;
			// Give some wiggle room for spawning objects
			switch (m_currentLevel.m_levelTemplate.m_levelGenerationType)
			{
				case (LevelType.GRID):
					elementsMultiplier = 16;
					break;

				case (LevelType.RANDOM):
					elementsMultiplier = 2000;
					break;

				case (LevelType.CENTERED):
					elementsMultiplier = 8;
					break;
			}

			Debug.Log("creating pool of size " + (m_numberOfSceneryElements * elementsMultiplier));

			// Make a pool of objects to make each layer out of
			for (int i = 0; i < m_numberOfSceneryElements * elementsMultiplier; i++)
			{
				SceneryElement element = wheel.Spin();
				GameObject go = Instantiate(element.m_prefab, null);
				go.name = element.m_prefab.name + " : " + i;
				go.SetActive(false);
				go.transform.parent = m_inactiveSceneryContainer.transform;
				if (m_currentLevel.m_levelTemplate.m_levelGenerationType == LevelType.RANDOM)
					go.transform.localScale = Vector3.one * Random.Range(element.randomScaleVariance.x, element.randomScaleVariance.y);
				else
					go.transform.localScale = Vector3.one * m_currentLevel.m_levelTemplate.m_unitSize * Random.Range(element.randomScaleVariance.x, element.randomScaleVariance.y);
				go.transform.position = Vector3.zero;
                if (element.randomRotation)
                {
                    float yValue = Random.Range(element.randomRotationVariance.x, element.randomRotationVariance.y);
                    go.transform.Rotate(0, yValue, 0);
                }
                SceneryModifier[] sm = go.GetComponents<SceneryModifier>();
				if (sm.Length != 0)
				{
					foreach (var mod in sm)
						mod.CreatePool();
				}
				m_inactiveSceneryElements.Add(go);
				//yield return null;
			}

			Debug.Log("finished creating pool");
			yield return null;
			c_poolCreated = true;
		}

		public void WipeObjects()
		{
			foreach (ObjectSpawner spawner in m_objectSpawners)
				spawner.WipeObjects();
		}

		bool c_sceneryClear;
		IEnumerator WipeSceneryAsync()
		{
			Debug.Log("Wipe Scenery Async");
			c_sceneryClear = false;
			for (int i = 0; i < m_activeSceneryContainer.transform.childCount; i++)
			{
				StartCoroutine(RemoveLayerOfLevelAsync(m_activeSceneryContainer.transform.GetChild(0)));
				while (!c_layerRemoved)
					yield return null;
			}
			c_sceneryClear = true;
		}

		public void RemoveLayerOfLevel(Transform container)
		{
			c_layerRemoved = false;
			// Unpack container and add parts back to the pool
			while (container.childCount > 0)
			{
				Transform child = container.GetChild(container.childCount - 1);
				child.SetParent(m_inactiveSceneryContainer.transform);
				child.gameObject.SetActive(false);
				child.position = Vector3.zero;
				m_inactiveSceneryElements.Add(child.gameObject);
			}
			c_layerRemoved = true;
		}

		bool c_layerRemoved;
		IEnumerator RemoveLayerOfLevelAsync(Transform container)
		{
			c_layerRemoved = false;
			// Unpack container and add parts back to the pool
			while (container && container.childCount > 0)
			{
				Transform child = container.GetChild(container.childCount - 1);
				child.SetParent(m_inactiveSceneryContainer.transform);
				child.gameObject.SetActive(false);
				child.position = Vector3.zero;
				m_inactiveSceneryElements.Add(child.gameObject);
				yield return null;
			}
			c_layerRemoved = true;
		}

		/// <summary>
		/// create a layer of the level from the pooled objects
		/// </summary>
		/// <param name="layerNum">Layer to create the materials at</param>
		void CreateLayerOfLevel(int layerNum)
		{
			if (m_inactiveSceneryContainer)
			{
				if (m_currentLevel.m_levelTemplate.m_trackWidth > 0)
				{
					// Make a layer of the level using the objects in the pools
					float unitSize = m_currentLevel.m_levelTemplate.m_unitSize;
					float halfTrackWidth = m_currentLevel.m_levelTemplate.m_trackWidth / 2;

					GameObject layerContainer = new GameObject("Layer Container : " + Time.time + " : " + layerNum);
					GameObject sceneryContainer = new GameObject("Scenery Container");
					sceneryContainer.transform.position = transform.position + Vector3.back * (layerNum * unitSize);

					int ran;
					GameObject go;

					switch (m_currentLevel.m_levelTemplate.m_levelGenerationType)
					{
						case (LevelType.GRID):
							//for each scenery layer, create a piece of scenery
							if (m_inactiveSceneryElements.Count == 0)
								Debug.LogError("Pool of grid scenery objects is empty! Tell Steve ASAP!");
							for (int i = 1; i <= m_currentLevel.m_levelTemplate.m_numOfSceneryLayers; i++)
							{
								// right side
								ran = Random.Range(0, m_inactiveSceneryElements.Count);
								go = m_inactiveSceneryElements[ran];
								m_activeSceneryElements.Add(go);
								m_inactiveSceneryElements.RemoveAt(ran);
								go.transform.parent = sceneryContainer.transform;
								go.transform.position = transform.position + Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * unitSize) + Vector3.right * halfTrackWidth - Vector3.right * unitSize / 2 + Vector3.right * unitSize * i;
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
								go.transform.parent = sceneryContainer.transform;
								go.transform.position = transform.position + Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * unitSize) + Vector3.left * halfTrackWidth - Vector3.left * unitSize / 2 + Vector3.left * unitSize * i;
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
							//spawn a block of object that will cover the next second that will

							float spawnInterval = 0.1f / m_currentLevel.m_levelTemplate.m_numOfSceneryLayers;
							float sceneSpeed = transform.position.z / m_currentLevel.m_levelTemplate.m_travelTime;
							float newUnitSize = m_spawnInterval * sceneSpeed;
							sceneryContainer.transform.position = transform.position + Vector3.back * (layerNum * newUnitSize);

							for (float f = 0; f < 1; f += spawnInterval)
							{
								if (m_inactiveSceneryElements.Count == 0)
									Debug.LogError("Pool of random scenery objects is empty! Tell Steve ASAP!");
								for (int i = 1; i <= m_currentLevel.m_levelTemplate.m_numOfSceneryLayers; i++)
								{
									// right side
									ran = Random.Range(0, m_inactiveSceneryElements.Count);
									go = m_inactiveSceneryElements[ran];
									m_activeSceneryElements.Add(go);
									m_inactiveSceneryElements.RemoveAt(ran);
									go.transform.parent = sceneryContainer.transform;
									go.transform.position = transform.position + Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * newUnitSize) + Vector3.forward * (f * newUnitSize) + Vector3.right * halfTrackWidth + Vector3.right * Random.Range(0, unitSize);
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
									go.transform.parent = sceneryContainer.transform;
									go.transform.position = transform.position + Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * newUnitSize) + Vector3.forward * (f * newUnitSize) + Vector3.left * halfTrackWidth + Vector3.left * Random.Range(0, unitSize);
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
							}

							sceneryContainer.transform.parent = layerContainer.transform;
							layerContainer.transform.parent = m_activeSceneryContainer.transform;

							break;

						case (LevelType.CENTERED):
							//for each scenery layer, create a piece of scenery
							if (m_inactiveSceneryElements.Count == 0)
								Debug.LogError("Pool of centered scenery objects is empty! Tell Steve ASAP!");

							// middle
							ran = Random.Range(0, m_inactiveSceneryElements.Count);
							go = m_inactiveSceneryElements[ran];
							m_activeSceneryElements.Add(go);
							m_inactiveSceneryElements.RemoveAt(ran);
							go.transform.parent = sceneryContainer.transform;
							go.transform.position = transform.position + Vector3.up * m_currentLevel.m_levelTemplate.m_spawnHeightOffset + Vector3.back * (layerNum * unitSize);
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
					//do some math to find the speed the scene must move
					lm.m_unitsToMovePerSecond = transform.position.z / m_currentLevel.m_levelTemplate.m_travelTime;
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
				if (c_poolCreated)
					CreateLayerOfLevel(0);
			}
		}
	}
}