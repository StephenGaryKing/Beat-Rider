using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;
using UnityEngine.UI;

namespace BeatRider
{
	[RequireComponent(typeof(Rigidbody), typeof(PlayerCollision))]
	public class PlayerController : MonoBehaviour
	{

		[SerializeField] float m_trackWidth = 6;
		[SerializeField] float m_moveSpeed = 160;
		[SerializeField] float m_bounceAmount = 8;
		[SerializeField] float m_tiltAmount = 0.3f;
		[SerializeField] float m_tiltSpeed = 0.2f;
		[SerializeField] bool m_fixedLaneMovement = false; 

		PlayerInput m_inputManager;
		float m_amountToMove = 0;
		float m_halfTrackWidth;
		bool m_moving;              // used with fixed lane movement
		int m_currentLane = 1;

		Rigidbody _rigidBody;
		// Use this for initialization
		void Start()
		{

#if UNITY_STANDALONE || UNITY_EDITOR
			m_inputManager = new ComputerInput();
#endif

#if UNITY_IOS || UNITY_ANDROID
			m_inputManager = new MobileInput();
#endif

			m_halfTrackWidth = m_trackWidth / 2;
			_rigidBody = GetComponent<Rigidbody>();
		}

		// Update is called once per frame
		void Update()
		{
			if (m_fixedLaneMovement && !m_moving)
			{
				float dir = m_inputManager.GatherInput();
				int oldLane = m_currentLane;

				if (dir > 0 && m_currentLane < 2)
					m_currentLane ++;
				if (dir < 0 && m_currentLane > 0)
					m_currentLane--;
				if (oldLane != m_currentLane)
				{
					m_moving = true;
					Invoke("ResetMoving", 0.15f);
				}
			}
			else
			{
				m_amountToMove = m_inputManager.GatherInput() * m_moveSpeed;
			}
		}


		private void ResetMoving()
		{
			m_moving = false;
		}

		public void UpdateSpeed(Slider slider)
		{
			m_moveSpeed = slider.value;
		}

		public void UpdateMovementType(Toggle toggle)
		{
			m_fixedLaneMovement = toggle.isOn;
		}

		private void FixedUpdate()
		{
			if (m_fixedLaneMovement)
			{
				Vector3 targetPosition = Vector3.zero;
				switch(m_currentLane)
				{
					case (0):
						targetPosition = Vector3.left * 2;
						break;

					case (1):
						targetPosition = Vector3.zero;
						break;

					case (2):
						targetPosition = Vector3.right * 2;
						break;
				}
				transform.position = Vector3.Lerp(transform.position, targetPosition, 0.001f * m_moveSpeed);
				m_amountToMove = (targetPosition.x - transform.position.x);
				_rigidBody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.forward * (m_tiltAmount * -m_amountToMove)), m_tiltSpeed));
			}
			else
			{
				_rigidBody.AddForce(Vector3.right * m_amountToMove);
				if (transform.position.x < -m_halfTrackWidth)
				{
					_rigidBody.velocity = Vector3.zero;
					_rigidBody.AddForce(Vector3.right * m_bounceAmount, ForceMode.Impulse);
				}
				if (transform.position.x > m_halfTrackWidth)
				{
					_rigidBody.velocity = Vector3.zero;
					_rigidBody.AddForce(Vector3.left * m_bounceAmount, ForceMode.Impulse);
				}

				_rigidBody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.forward * (m_tiltAmount * -m_amountToMove)), m_tiltSpeed));
			}
		}
	}
}