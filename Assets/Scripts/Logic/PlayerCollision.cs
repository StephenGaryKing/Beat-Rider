using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;
using UnityEngine.UI;

namespace BeatRider
{
	[RequireComponent(typeof(PlayerSoundEffects))]
	public class PlayerCollision : MonoBehaviour
	{
		public Transform m_sheild;
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
        bool m_invincible = false;

		PlayerSoundEffects m_playerSoundEffects;
		QuickTimeInput m_quickTime;

		SongController m_songController;

        public int totalNotes = 0;
        public int runGemDust = 0;
        [SerializeField] private Text m_notesText = null;
        [SerializeField] private Text m_percentageText = null;
        [SerializeField] private Text m_gemDustText = null;
        [SerializeField] private ShopManager m_shopManager = null;


		// Use this for initialization
		void Start()
		{
			m_quickTime = GetComponent<QuickTimeInput>();
			m_songController = FindObjectOfType<SongController>();
			m_targetFOV = m_minFOV;
			m_startingCameraPos = Camera.main.transform.position;
			m_floatingCamera = FindObjectOfType<FloatingCameraLogic>();
			m_playerSoundEffects = GetComponent<PlayerSoundEffects>();
            if (!m_shopManager)
                m_shopManager = FindObjectOfType<ShopManager>();
		}

		private void FixedUpdate()
		{
			if (m_targetFOV > m_minFOV)
				m_targetFOV -= m_ambientSpeedDegradation;
            if (m_targetFOV <= m_minFOV)
                m_sheild.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.KeypadPeriod))
                m_invincible = !m_invincible;
        }

        void Die()
		{

		}

		public void Revive()
		{

		}

        public void TurnOffSheild()
        {
            m_sheild.gameObject.SetActive(false);
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

                totalNotes++;

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
					if (m_sheild)
						m_sheild.gameObject.SetActive(true);
					return;
				}

			if (other.CompareTag("Obstacle"))
			{
				ObstacleLogic ol = other.GetComponent<ObstacleLogic>();

				if (m_sheild)
					m_sheild.gameObject.SetActive(false);

				if (m_targetFOV > m_minFOV)
				{
					m_targetFOV = m_minFOV;
					m_floatingCamera.StartCoroutine(m_floatingCamera.Shake(ol.m_shakeMagnitude, ol.m_shakeFrequency, ol.m_waitTime, ol.m_relaxTime));
				}
				else
				{
					if (!ol.m_tutorialMode && !m_invincible)
					{
						Die();
						m_songController.ActualStopSong(StopSongConditions.PlayerDead);
					}
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
                {
                    Gem gem = gl.PickupGem();
                    runGemDust += gem.dustValue;
                    m_shopManager.m_currentGemDust += gem.dustValue;
                }
				other.gameObject.SetActive(false);
				AchievementManager.OnTallyPickups(other.tag);
                m_playerSoundEffects.m_soundManager.PlaySound(m_playerSoundEffects.m_gemPickup);
                //totalGemDust++;
			}
		}
	}
}