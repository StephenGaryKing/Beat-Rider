using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class ShipWobbleLogic : MonoBehaviour
	{
		public float m_frequency = 1;
		public float m_amplitude = 1;
		float m_height = 0;
		float m_zRot = 0;

		void FixedUpdate()
		{
			m_height = Mathf.Sin(Time.time * m_frequency) * m_amplitude;
			Vector3 pos = transform.localPosition;
			pos.y = m_height;
			transform.localPosition = pos;

			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.right * (m_height * 100 + 0.2f) ), 0.05f);
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.forward * m_zRot), 0.02f);

			if (Random.Range(0, 100) < 10)
				m_zRot = Random.Range(-10, 10);
		}
	}
}