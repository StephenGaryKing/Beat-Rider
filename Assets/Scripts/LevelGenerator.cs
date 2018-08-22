using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
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

		}

		void CreateLayerOfLevel()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}