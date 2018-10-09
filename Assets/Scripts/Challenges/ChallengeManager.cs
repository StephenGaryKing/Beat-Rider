using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatRider
{
	public class ChallengeManager : MonoBehaviour
	{
		public string m_saveFileName = "Challenges";

		public Gem[] m_UnlocksToStartWith;
		public Gem[] m_allChallenges;
		public Image[] m_pickupImageLocations;
		[HideInInspector] public List<Gem> m_collectedGems;
		[HideInInspector] public List<int> m_challengesCompleated;
		public int m_challengeToFocusOn;

		private void Start()
		{
			foreach(var unlock in m_UnlocksToStartWith)
				UnlockChallenge(FindGemIndex(unlock));
			DisplayPickupList();
		}

		public void SaveChallenges()
		{
			SaveFile saveFile = new SaveFile();
			saveFile.AddList(m_challengesCompleated);
			saveFile.Save(m_saveFileName);
		}

		public void LoadChallenges()
		{
			SaveFile saveFile = new SaveFile();
			saveFile.Load(m_saveFileName);

			m_challengesCompleated = saveFile.m_numbers[0].list;
		}

		public void PickChallenge(Gem challenge)
		{
			m_challengeToFocusOn = FindGemIndex(challenge);
			RefreshPickupList();
		}

		int FindGemIndex(Gem challenge)
		{
			int gemIndex = -1;
			for (int i = 0; i < m_allChallenges.Length; i++)
				if (m_allChallenges[i] == challenge)
					gemIndex = i;

			if (gemIndex == -1)
				Debug.LogError("Challenge (" + challenge + ") was not found in the challenge manager's list.\nDid not unlock. Please add this challenge to the list");

			return gemIndex;
		}

		public void PickupGem(Gem gem)
		{
			m_collectedGems.Add(gem);
			RefreshPickupList();
			DisplayPickupList();
			if (CheckPickupCombination())
				UnlockChallenge(m_challengeToFocusOn);
		}

		void UnlockChallenge (int gemIndex)
		{
			if (!m_challengesCompleated.Contains(gemIndex))
				m_challengesCompleated.Add(gemIndex);
			SaveChallenges();
		}

		bool CheckPickupCombination()
		{
			if (m_collectedGems.Count != m_allChallenges[m_challengeToFocusOn].m_recipe.GemsToPickup.Length)
				return false;

			for(int i = 0; i < m_collectedGems.Count; i ++)
			{
				if (m_collectedGems[i] != m_allChallenges[m_challengeToFocusOn].m_recipe.GemsToPickup[i])
					return false;
			}

			// everything matches up
			return true;
		}

		void DisplayPickupList()
		{
			if (m_pickupImageLocations.Length < m_collectedGems.Count)
				Debug.LogError("Not enough image locations for ChallengeManager!\nRequired " + m_collectedGems.Count + " image locations.");
			for (int i = 0; i < m_pickupImageLocations.Length; i++)
			{
				if (i < m_collectedGems.Count)
				{
					m_pickupImageLocations[i].sprite = m_collectedGems[i].m_unlockable.m_icon;
					UnlockableColour uc = m_collectedGems[i].m_unlockable as UnlockableColour;
					if (uc)
						m_pickupImageLocations[i].color = uc.m_colour;
					m_pickupImageLocations[i].gameObject.SetActive(true);
				}
				else
					m_pickupImageLocations[i].gameObject.SetActive(false);
			}
		}

		void RefreshPickupList()
		{
			// while there are more gems in the list than the focused recipies require, pop the oldest elements of the list
			while (m_collectedGems.Count > m_allChallenges[m_challengeToFocusOn].m_recipe.GemsToPickup.Length)
				m_collectedGems.RemoveAt(0);
		}
	}
}