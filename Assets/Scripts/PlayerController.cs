using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

namespace BeatRider
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerController : MonoBehaviour
	{

		[SerializeField] float m_trackWidth = 6;
		[SerializeField] float m_moveSpeed = 160;
		[SerializeField] float m_bounceAmount = 8;
		[SerializeField] float m_tiltAmount = 0.3f;
		[SerializeField] float m_tiltSpeed = 0.2f;
		[SerializeField] float m_ambientSpeedDegradation = 0.1f;

		[SerializeField] float m_maxZoomAmount = 10;
		[SerializeField] float m_FOVSmoothing = 0.1f;
		public float m_minFOV = 50;
		public float m_maxFOV = 100;
		[HideInInspector] public float m_targetFOV;
		[HideInInspector] public float m_FOVAmount;

		[SerializeField] float m_maxScoreMultiplier = 30;

		float m_amountToMove = 0;
		float m_halfTrackWidth;
		Vector3 m_startingCameraPos;
		FloatingCameraLogic m_floatingCamera;

		ScoreBoardLogic _scoreBoardLogic;
		Rigidbody _rigidBody;
		// Use this for initialization
		void Start()
		{
			m_floatingCamera = FindObjectOfType<FloatingCameraLogic>();
			_scoreBoardLogic = FindObjectOfType<ScoreBoardLogic>();
			m_startingCameraPos = Camera.main.transform.position;
			m_targetFOV = m_minFOV;
			m_halfTrackWidth = m_trackWidth / 2;
			_rigidBody = GetComponent<Rigidbody>();
		}

		// Update is called once per frame
		void Update()
		{
			m_amountToMove = Input.GetAxisRaw("Horizontal") * m_moveSpeed;
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.1f);
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

			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, m_targetFOV, 0.1f);
			m_FOVAmount = (m_targetFOV - m_minFOV) / (m_maxFOV - m_minFOV);
			Vector3 camPos = Vector3.Lerp(Camera.main.transform.position, m_startingCameraPos + Camera.main.transform.forward * (m_FOVAmount * m_maxZoomAmount), 0.1f);
			Camera.main.transform.position = camPos;

			if (m_targetFOV > m_minFOV)
				m_targetFOV -= m_ambientSpeedDegradation;

			_rigidBody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.forward * (m_tiltAmount * -m_amountToMove)), m_tiltSpeed));
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Note"))
			{
				_scoreBoardLogic.m_score += Mathf.RoundToInt(1 + (m_maxScoreMultiplier * m_FOVAmount));
				Destroy(other.gameObject);
			}

			if (other.CompareTag("Boost"))
				if (m_targetFOV < m_maxFOV)
					m_targetFOV += other.GetComponent<BoostPadLogic>().m_boostAmount;

			if (other.CompareTag("Obstacle"))
			{
                if (m_targetFOV > m_minFOV)
                {
                    m_targetFOV = m_minFOV;
                    ObstacleLogic ol = other.GetComponent<ObstacleLogic>();
                    m_floatingCamera.StartCoroutine(m_floatingCamera.Shake(ol.m_shakeMagnitude, ol.m_shakeFrequency, ol.m_waitTime, ol.m_relaxTime));
                }
                else
                {
                    FindObjectOfType<SongController>().KillPlayer();
                }
			}
		}
	}
}