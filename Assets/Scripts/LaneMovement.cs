using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

namespace BeatRider
{
	public class LaneMovement : MonoBehaviour
	{
		public float m_unitsToMovePerSecond = 1;
		public float m_zValueToDie;
		public LevelGenerator m_levelGen;

		private void FixedUpdate()
		{
			transform.position += Vector3.back * m_unitsToMovePerSecond * Time.deltaTime;
			if (m_levelGen)
			{
				if (transform.position.z + m_levelGen.transform.position.z < m_zValueToDie)
					m_levelGen.RemoveLayerOfLevel(transform);
			}
			else
			{
				if (transform.position.z < m_zValueToDie)
					Destroy(gameObject);
			}
		}
	}
}