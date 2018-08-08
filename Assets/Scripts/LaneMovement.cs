using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

namespace BeatRider
{
	public class LaneMovement : MonoBehaviour
	{
		[SerializeField] float m_unitsToMovePerSecond = 1;

		private void FixedUpdate()
		{
			transform.position += -Vector3.forward * m_unitsToMovePerSecond * Time.deltaTime;
			if (transform.position.z < -10)
				Destroy(gameObject);
		}
	}
}