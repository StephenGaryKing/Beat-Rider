using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

namespace BeatRider
{
	public class GemLogic : MonoBehaviour
	{
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
			if (!SongController.m_freeFlow)
				gameObject.SetActive(false);
		}

		public void PickupGem()
		{
			m_manager.PickupGem(m_randomizer.GetGem());
		}
	}
}