using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class FloatingCameraLogic : MonoBehaviour {

		public PlayerController m_target;							// The player (used for reacting to speed)
		public float m_movementSmoothing = 0.1f;					// How smooth the camera should move
		public Vector3 m_movementMask = new Vector3(1, 1, 0);		// Mask to deny/multiply movement on cirtain axis'

		public float m_shakeMagnitude = 1;			// Magnitude to shake
		public float m_shakeFrequency = 1;			// Frequency to shake

		float m_defaultMag;			// used as a refference when hitting objects
		float m_defaultFreq;        // used as a refference when hitting objects

		Vector3 m_shake;
		Vector3 m_newShake = new Vector3();
		Vector3 m_oldShake = new Vector3();
		Vector3 m_offset;
		float m_shakeModifier = 0;
		float m_shakeTimer = 0;

		private void Start()
		{
			// set default/starting positions for values
			m_defaultMag = m_shakeMagnitude;
			m_defaultFreq = m_shakeFrequency;
			m_offset = transform.position - m_target.transform.position;
		}

		void FixedUpdate() {
			//follow Movement
			Vector3 newPos = Vector3.Lerp(transform.position, m_target.transform.position + m_offset, m_movementSmoothing);

			// mask out the correct axis'
			newPos.x *= m_movementMask.x;
			newPos.y *= m_movementMask.y;
			newPos.z *= m_movementMask.z;
			
			// correct the position on axis' that are masked out
			newPos.x += transform.position.x * (1 - m_movementMask.x);
			newPos.y += transform.position.y * (1 - m_movementMask.y);
			newPos.z += transform.position.z * (1 - m_movementMask.z);

			// add the shake effect
			newPos += m_shake;

			// move the camera
			transform.position = newPos;

			//shake
			m_shakeTimer += Time.deltaTime;
			if (m_shakeTimer >= m_shakeFrequency)
			{
				//randomise the location to shake towards
				m_oldShake = m_newShake;
				m_newShake = new Vector3(
					Random.Range(-m_shakeMagnitude * 1000, m_shakeMagnitude * 1000) / 1000f,
					Random.Range(-m_shakeMagnitude * 1000, m_shakeMagnitude * 1000) / 1000f,
					Random.Range(-m_shakeMagnitude * 1000, m_shakeMagnitude * 1000) / 1000f);
				m_shakeTimer = 0;

				// keep from shaking in the wrong axis
				m_newShake.x *= m_movementMask.x;
				m_newShake.y *= m_movementMask.y;
				m_newShake.z *= m_movementMask.z;
			}

			// move to the new shake location liniarly
			m_shake = Vector3.Lerp(m_oldShake * (m_target.m_FOVAmount + m_shakeModifier), m_newShake * (m_target.m_FOVAmount + m_shakeModifier), m_shakeTimer / m_shakeFrequency);
			//fix vector if it is NAN
			if (float.IsNaN(m_shake.x))
				m_shake = Vector3.zero;

			// increase the FOV based on the speed of the player
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, m_target.m_targetFOV, 0.1f);
			m_target.m_FOVAmount = (m_target.m_targetFOV - m_target.m_minFOV) / (m_target.m_maxFOV - m_target.m_minFOV);
			// correct the camera position based of the speed of the player (FOV increases can cause too much visibility in the vertical axis)
			Vector3 camPos = Vector3.Lerp(Camera.main.transform.position, m_target.m_startingCameraPos + Camera.main.transform.forward * (m_target.m_FOVAmount * m_target.m_maxZoomAmount), 0.1f);
			Camera.main.transform.position = camPos;
		}

		/// <summary>
		/// shake the camera based on outside effects such as hitting something
		/// </summary>
		/// <param name="mag">Magnitude of the shaking</param>
		/// <param name="freq">Frequency of the shaking</param>
		/// <param name="waitTime">How long to shake for</param>
		/// <param name="relaxTime">Length of the cooldown period</param>
		/// <returns></returns>
		public IEnumerator Shake(float mag, float freq, float waitTime, float relaxTime)
		{
			m_shakeModifier = 1;

			// set shake values
			m_shakeMagnitude = mag;
			m_shakeFrequency = freq;

			// shake for waitTime seconds
			yield return new WaitForSeconds(waitTime);

			// linearly slow down the shaking over relax time seconds
			while (m_shakeModifier > 0)
			{ 
				m_shakeModifier -= Time.deltaTime / relaxTime;
				m_shakeMagnitude = Mathf.Lerp(mag, m_shakeFrequency+m_defaultMag, 1 - m_shakeModifier);
				m_shakeFrequency = Mathf.Lerp(freq, m_defaultFreq, 1 - m_shakeModifier);
			}
			m_shakeModifier = 0;
			// reset values
			m_shakeMagnitude = m_defaultMag;
			m_shakeFrequency = m_defaultFreq;
		}
	}
}