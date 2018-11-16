using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

namespace BeatRider
{
	public class GemLogic : MonoBehaviour
	{
        public Gem m_manualySetGem;
		CraftingManager m_manager;
		GemTypeRandomizer m_randomizer;

		
		// Use this for initialization
		void Start()
		{
			m_manager = FindObjectOfType<CraftingManager>();
			m_randomizer = GetComponentInChildren<GemTypeRandomizer>();
		}

		private void OnEnable()
		{
			if (!m_manager)
				m_manager = FindObjectOfType<CraftingManager>();
			if (!m_randomizer	)
				m_randomizer = GetComponentInChildren<GemTypeRandomizer>();

			if (!m_manualySetGem && !SongController.m_freeFlow)
				gameObject.SetActive(false);
			
			if (m_manualySetGem) 
				m_randomizer.SetGemManualy (m_manualySetGem);
		}

		public void PickupGem()
		{
			m_manager.PickupGem(m_randomizer.GetGem());
		}
	}
}