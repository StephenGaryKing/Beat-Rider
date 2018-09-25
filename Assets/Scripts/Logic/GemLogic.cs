using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class GemLogic : MonoBehaviour
	{
		ChallengeManager m_manager;
		GemTypeRandomizer m_randomizer;
		
		// Use this for initialization
		void Start()
		{
			m_manager = FindObjectOfType<ChallengeManager>();
			m_randomizer = GetComponentInChildren<GemTypeRandomizer>();
		}

		public void PickupGem()
		{
			m_manager.PickupGem(m_randomizer.GetGem());
		}
	}
}