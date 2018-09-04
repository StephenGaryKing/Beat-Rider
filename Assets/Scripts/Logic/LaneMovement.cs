﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

namespace BeatRider
{
	public class LaneMovement : MonoBehaviour
	{
		public float m_unitsToMovePerSecond = 1;	// How fast to move
		public float m_zValueToDie;					// z depth to die at
		public LevelGenerator m_levelGen;           // level generator, used to return to pool when killed

		private void FixedUpdate()
		{
			// move
			transform.position += Vector3.back * m_unitsToMovePerSecond * Time.deltaTime;
			
			// return to the level gen pool of objects
			if (m_levelGen)
			{
				if (transform.position.z + m_levelGen.transform.position.z < m_zValueToDie)
					m_levelGen.RemoveLayerOfLevel(transform);
			}
			// if created without a level generator, just disable it
			else
			{
				if (transform.position.z < m_zValueToDie)
					gameObject.SetActive(false);
			}
		}
	}
}