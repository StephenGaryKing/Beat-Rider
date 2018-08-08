using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MusicalGameplayMechanics;

namespace BeatRider
{
	public class ScoreBoardLogic : MonoBehaviour
	{

		[HideInInspector] public int m_score = 0;

		public int m_scorePerSecond = 1;
		public Text m_scoreText = null;
		SongController m_songController;

		float m_scoreToAdd = 0;

		private void Start()
		{
			m_songController = FindObjectOfType<SongController>();
		}

		public void Update()
		{
			m_scoreText.text = m_score.ToString();
		}

		private void FixedUpdate()
		{
			if (m_songController.m_songIsBeingPlayed)
			{
				m_scoreToAdd += Time.deltaTime;
				if (m_scoreToAdd > 1)
				{
					m_score += m_scorePerSecond;
					m_scoreToAdd = 0;
				}
			}
		}

		public void ResetScores()
		{
			m_score = 0;
		}
	}
}