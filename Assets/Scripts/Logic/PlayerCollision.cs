using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

namespace BeatRider
{
	[RequireComponent(typeof(PlayerSoundEffects))]
	public class PlayerCollision : MonoBehaviour
	{
		[SerializeField] float m_maxScoreMultiplier = 30;
		public float m_maxZoomAmount = 6.43f;
		[SerializeField] float m_FOVSmoothing = 0.1f;
		public float m_minFOV = 50;
		public float m_maxFOV = 110;
		[HideInInspector] public float m_targetFOV;
		[HideInInspector] public float m_FOVAmount;
		FloatingCameraLogic m_floatingCamera;
		[SerializeField] float m_ambientSpeedDegradation = 0.1f;
		[HideInInspector] public Vector3 m_startingCameraPos;

		PlayerSoundEffects m_playerSoundEffects;
		QuickTimeInput m_quickTime;

		SongController m_songController;

		// Use this for initialization
		void Start()
		{
			m_quickTime = GetComponent<QuickTimeInput>();
			m_songController = FindObjectOfType<SongController>();
			m_targetFOV = m_minFOV;
			m_startingCameraPos = Camera.main.transform.position;
			m_floatingCamera = FindObjectOfType<FloatingCameraLogic>();
			m_playerSoundEffects = GetComponent<PlayerSoundEffects>();
		}

		private void FixedUpdate()
		{
			if (m_targetFOV > m_minFOV)
				m_targetFOV -= m_ambientSpeedDegradation;
		}
		
		void Die()
		{

		}

		public void Revive()
		{

		}

		public void ResetSpeed()
		{
			m_targetFOV = m_minFOV;
			m_floatingCamera.SnapFOV();
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Note"))
			{
				// get the number off the gem
				int val = other.GetComponent<QuickTimeNote>().Val;
				m_quickTime.StartCoroutine(m_quickTime.LookForKeyPress(val, other.gameObject));
				
				return;
			}

			if (other.CompareTag("Boost"))
				if (m_targetFOV < m_maxFOV)
				{
					m_targetFOV += other.GetComponent<BoostPadLogic>().m_boostAmount;
					other.GetComponent<ParticleCreationLogic>().SpawnParticle();
					if (m_playerSoundEffects.m_boost.soundToPlay)
						m_playerSoundEffects.m_soundManager.PlaySound(m_playerSoundEffects.m_boost);
					AchievementManager.OnTallyPickups(other.tag);
					return;
				}

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
					Die();
					m_songController.ActualStopSong(StopSongConditions.PlayerDead);
				}
				if (m_playerSoundEffects.m_hitObstical.soundToPlay)
					m_playerSoundEffects.m_soundManager.PlaySound(m_playerSoundEffects.m_hitObstical);
				other.gameObject.SetActive(false);

				AchievementManager.OnTallyPickups(other.tag);
				return;
			}

			if (other.CompareTag("Gem"))
			{
				GemLogic gl = other.GetComponent<GemLogic>();
				if (gl)
					gl.PickupGem();
				other.gameObject.SetActive(false);
				AchievementManager.OnTallyPickups(other.tag);
			}
		}
	}
}