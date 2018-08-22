using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	[UnityEditor.InitializeOnLoad]
	class EditorUpdator
	{
		static EditorUpdator()
		{
			UnityEditor.EditorApplication.update += Update;
		}

		static void Update()
		{
			//update percentages of spawn rates
		}
	}

	public class LevelGenerator : MonoBehaviour
	{
		[Tooltip("A scriptable object that determines the layout of a level")]
		LevelTemplate m_levelTemplate;

		PickupSpawner m_PickupSpawner;

		// Use this for initialization
		void Start()
		{
			m_PickupSpawner = FindObjectOfType<PickupSpawner>();
		}

		void CreatePool()
		{
			// Make a pool of objects to make each layer out of
		}

		void CreateLayerOfLevel()
		{
			// Make a layer of the level using the objects in the pools
			Vector3 m_spawnLocation = Vector3.forward * m_levelTemplate.m_unitsAwayToSpawn * m_levelTemplate.m_unitSize;

			switch (m_levelTemplate.m_levelGenerationType)
			{
				case (LevelType.GRID):
					//create Track piece
					foreach(var track in m_levelTemplate.m_trackElements)
					{
						//add the values together then pick a random value via roulette wheels selection

					}
					//for each scenery layer, create a piece of scenery


					break;
				case (LevelType.RANDOM):

					break;
			}
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}