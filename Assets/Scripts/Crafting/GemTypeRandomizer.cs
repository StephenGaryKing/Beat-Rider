using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class GemTypeRandomizer : MonoBehaviour
	{
		CraftingManager m_challengeManager;
		[HideInInspector] public int m_gemIndexNumber;
		Renderer m_ren;

		// Use this for initialization
		void Awake()
		{
			m_challengeManager = FindObjectOfType<CraftingManager>();
			m_ren = GetComponent<Renderer>();
		}

		public Gem GetGem()
		{
			return m_challengeManager.m_allRecipes[m_gemIndexNumber];
		}

		private void OnEnable()
		{
			Randomize();
			ApplyMaterial();
		}

		void Randomize()
		{
			if (m_challengeManager.m_RecipesCompleated.Count > 0)
			{
				//NOTE: maybe implement a roulett wheel to allow for rarity
				//NOTE: only randomize from pool currently used for the filtered recipes
				int rand = Random.Range(0, m_challengeManager.m_RecipesCompleated.Count);
				m_gemIndexNumber = m_challengeManager.m_RecipesCompleated[rand];
			}
		}

		void ApplyMaterial()
		{
			if (m_challengeManager.m_RecipesCompleated.Count > 0)
				m_ren.material = m_challengeManager.m_allRecipes[m_gemIndexNumber].m_material;
		}

	}
}