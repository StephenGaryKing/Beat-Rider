using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

namespace BeatRider
{
	[RequireComponent(typeof(PlayerSoundEffects))]
	public class PlayerCollision : MonoBehaviour
	{

		ScoreBoardLogic m_scoreBoardLogic;
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

		SongController m_songController;

		// Use this for initialization
		void Start()
		{
			m_songController = FindObjectOfType<SongController>();
			m_targetFOV = m_minFOV;
			m_startingCameraPos = Camera.main.transform.position;
			m_floatingCamera = FindObjectOfType<FloatingCameraLogic>();
			m_scoreBoardLogic = FindObjectOfType<ScoreBoardLogic>();
			m_playerSoundEffects = GetComponent<PlayerSoundEffects>();
		}

		private void FixedUpdate()
		{
			if (m_targetFOV > m_minFOV)
				m_targetFOV -= m_ambientSpeedDegradation;
		}
		
		public void ResetSpeed()
		{
			m_targetFOV = m_minFOV;
			m_floatingCamera.SnapFOV();
		}

		public void Die()
		{

		}

		public void Revive()
		{

		}

		void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Note"))
			{
				m_scoreBoardLogic.m_score += Mathf.RoundToInt(1 + (m_maxScoreMultiplier * m_FOVAmount));
				other.gameObject.SetActive(false);
				other.GetComponent<ParticleCreationLogic>().SpawnParticle();
				if (m_playerSoundEffects.m_pickupNote.soundToPlay)
					m_playerSoundEffects.m_soundManager.PlaySound(m_playerSoundEffects.m_pickupNote);
				AchievementManager.OnTallyPickups("Obsticle");
				return;
			}

			if (other.CompareTag("Boost"))
				if (m_targetFOV < m_maxFOV)
				{
					m_targetFOV += other.GetComponent<BoostPadLogic>().m_boostAmount;
					other.GetComponent<ParticleCreationLogic>().SpawnParticle();
					if (m_playerSoundEffects.m_boost.soundToPlay)
						m_playerSoundEffects.m_soundManager.PlaySound(m_playerSoundEffects.m_boost);
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
					FindObjectOfType<SongController>().StopSong(true);
				}
				if (m_playerSoundEffects.m_hitObstical.soundToPlay)
					m_playerSoundEffects.m_soundManager.PlaySound(m_playerSoundEffects.m_hitObstical);
				other.gameObject.SetActive(false);

				AchievementManager.OnTallyPickups("Obsticle");
				return;
			}

			if (other.CompareTag("Gem"))
			{
				GemLogic gl = other.GetComponent<GemLogic>();
				if (gl)
					gl.PickupGem();
				other.gameObject.SetActive(false);
				AchievementManager.OnTallyPickups("Gem");
			}
		}
	}
}