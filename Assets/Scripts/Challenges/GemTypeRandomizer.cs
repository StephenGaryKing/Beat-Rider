using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class GemTypeRandomizer : MonoBehaviour
	{
		ChallengeManager m_challengeManager;
		[HideInInspector] public int m_gemIndexNumber;
		Renderer m_ren;

		// Use this for initialization
		void Awake()
		{
			m_challengeManager = FindObjectOfType<ChallengeManager>();
			m_ren = GetComponent<Renderer>();
		}

		public Gem GetGem()
		{
			return m_challengeManager.m_allChallenges[m_gemIndexNumber];
		}

		private void OnEnable()
		{
			Randomize();
			ApplyMaterial();
		}

		void Randomize()
		{
			if (m_challengeManager.m_challengesCompleated.Count > 0)
			{
				//NOTE: maybe implement a roulett wheel to allow for rarity
				int rand = Random.Range(0, m_challengeManager.m_challengesCompleated.Count);
				m_gemIndexNumber = m_challengeManager.m_challengesCompleated[rand];
			}
		}

		void ApplyMaterial()
		{
			if (m_challengeManager.m_challengesCompleated.Count > 0)
				m_ren.material = m_challengeManager.m_allChallenges[m_gemIndexNumber].m_material;
		}

	}
}