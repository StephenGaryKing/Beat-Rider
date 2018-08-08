using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class FloatingCameraLogic : MonoBehaviour {

	public PlayerController m_target;
	public float m_movementSmoothing = 0.1f;
		public Vector3 m_movementMask = new Vector3(1, 1, 0);

		public float m_shakeMagnitude = 1;
		public float m_shakeFrequency = 1;

		float m_defaultMag;
		float m_defaultFreq;

		Vector3 m_shake;
		Vector3 m_newShake = new Vector3();
		Vector3 m_oldShake = new Vector3();
		Vector3 m_offset;
		float m_shakeModifier = 0;
		float m_shakeTimer = 0;

		private void Start()
		{
			m_defaultMag = m_shakeMagnitude;
			m_defaultFreq = m_shakeFrequency;
			m_offset = transform.position - m_target.transform.position;
		}

		void FixedUpdate() {
			//follow Movement
			Vector3 newPos = Vector3.Lerp(transform.position, m_target.transform.position + m_offset, m_movementSmoothing);

			newPos.x *= m_movementMask.x;
			newPos.y *= m_movementMask.y;
			newPos.z *= m_movementMask.z;

			newPos.x += transform.position.x * (1 - m_movementMask.x);
			newPos.y += transform.position.y * (1 - m_movementMask.y);
			newPos.z += transform.position.z * (1 - m_movementMask.z);

			newPos += m_shake;

			transform.position = newPos;

			//shake
			m_shakeTimer += Time.deltaTime;
			if (m_shakeTimer >= m_shakeFrequency)
			{
				m_oldShake = m_newShake;
				m_newShake = new Vector3(
					Random.Range(-m_shakeMagnitude * 1000, m_shakeMagnitude * 1000) / 1000f,
					Random.Range(-m_shakeMagnitude * 1000, m_shakeMagnitude * 1000) / 1000f,
					Random.Range(-m_shakeMagnitude * 1000, m_shakeMagnitude * 1000) / 1000f);
				m_shakeTimer = 0;

				m_newShake.x *= m_movementMask.x;
				m_newShake.y *= m_movementMask.y;
				m_newShake.z *= m_movementMask.z;
			}

			m_shake = Vector3.Lerp(m_oldShake * (m_target.m_FOVAmount + m_shakeModifier), m_newShake * (m_target.m_FOVAmount + m_shakeModifier), m_shakeTimer / m_shakeFrequency);
			//fix vector if it is NAN
			if (float.IsNaN(m_shake.x))
				m_shake = Vector3.zero;
		}

		public IEnumerator Shake(float mag, float freq, float waitTime, float relaxTime)
		{
			m_shakeModifier = 1;

			m_shakeMagnitude = mag;
			m_shakeFrequency = freq;

			yield return new WaitForSeconds(waitTime);

			while (m_shakeModifier > 0)
			{ 
				m_shakeModifier -= Time.deltaTime / relaxTime;
				m_shakeMagnitude = Mathf.Lerp(mag, m_shakeFrequency+m_defaultMag, 1 - m_shakeModifier);
				m_shakeFrequency = Mathf.Lerp(freq, m_defaultFreq, 1 - m_shakeModifier);
			}
			m_shakeModifier = 0;
			m_shakeMagnitude = m_defaultMag;
			m_shakeFrequency = m_defaultFreq;
		}
	}
}