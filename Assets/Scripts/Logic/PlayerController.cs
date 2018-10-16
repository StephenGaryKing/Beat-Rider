using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

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

		PlayerInput m_inputManager;
		float m_amountToMove = 0;
		float m_halfTrackWidth;

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
			m_amountToMove = m_inputManager.GatherInput() * m_moveSpeed;
		}

		private void FixedUpdate()
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