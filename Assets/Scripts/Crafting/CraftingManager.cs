using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatRider
{
	public class CraftingManager : MonoBehaviour
	{
		public string m_saveFileName = "Challenges";

		public Gem[] m_ReipesToStartWith;
		public Gem[] m_allRecipes;
		public Image[] m_pickedupGemImageLocations;
		[HideInInspector] public List<Gem> m_collectedGems;
		[HideInInspector] public List<int> m_RecipesCompleated;
		public int m_recipeToFocusOn;

		private void Start()
		{
			List<Gem> newAllRecipies = new List<Gem>(m_allRecipes);

			foreach (Gem recipe in m_ReipesToStartWith)
				if (FindGemIndex(recipe) == -1)
					newAllRecipies.Add(recipe);

			m_allRecipes = newAllRecipies.ToArray();

			foreach(var unlock in m_ReipesToStartWith)
				UnlockChallenge(FindGemIndex(unlock));

			DisplayPickupList();
		}

		public void SaveChallenges()
		{
			SaveFile saveFile = new SaveFile();
			saveFile.AddList(m_RecipesCompleated);
			saveFile.Save(m_saveFileName);
		}

		public void LoadChallenges()
		{
			SaveFile saveFile = new SaveFile();
			saveFile.Load(m_saveFileName);

			m_RecipesCompleated = saveFile.m_numbers[0].list;
		}

		public void PickChallenge(Gem challenge)
		{
			m_recipeToFocusOn = FindGemIndex(challenge);
			RefreshPickupList();
		}

		public int FindGemIndex(Gem Recipie)
		{
			int gemIndex = -1;
			for (int i = 0; i < m_allRecipes.Length; i++)
				if (m_allRecipes[i] == Recipie)
					gemIndex = i;

			if (gemIndex == -1)
				Debug.LogError("Recipie (" + Recipie + ") was not found in the Crafting Manager's \"All Recipies\" list.\nDid not unlock. Please add this challenge to the list");

			return gemIndex;
		}

		public void PickupGem(Gem gem)
		{
			m_collectedGems.Add(gem);
			RefreshPickupList();
			DisplayPickupList();
			if (CheckPickupCombination())
				UnlockChallenge(m_recipeToFocusOn);
		}

		void UnlockChallenge (int gemIndex)
		{
			if (!m_RecipesCompleated.Contains(gemIndex))
				m_RecipesCompleated.Add(gemIndex);
			SaveChallenges();
		}

		bool CheckPickupCombination()
		{
			if (m_collectedGems.Count != m_allRecipes[m_recipeToFocusOn].m_recipe.GemsToPickup.Length)
				return false;

			for(int i = 0; i < m_collectedGems.Count; i ++)
			{
				if (m_collectedGems[i] != m_allRecipes[m_recipeToFocusOn].m_recipe.GemsToPickup[i])
					return false;
			}

			// everything matches up
			return true;
		}

		void DisplayPickupList()
		{
			if (m_pickedupGemImageLocations.Length < m_collectedGems.Count)
				Debug.LogError("Not enough image locations for ChallengeManager!\nRequired " + m_collectedGems.Count + " image locations.");
			for (int i = 0; i < m_pickedupGemImageLocations.Length; i++)
			{
				if (i < m_collectedGems.Count)
				{
					m_pickedupGemImageLocations[i].sprite = m_collectedGems[i].m_unlockable.m_icon;
					UnlockableColour uc = m_collectedGems[i].m_unlockable as UnlockableColour;
					if (uc)
						m_pickedupGemImageLocations[i].color = uc.m_colour;
					m_pickedupGemImageLocations[i].gameObject.SetActive(true);
				}
				else
					m_pickedupGemImageLocations[i].gameObject.SetActive(false);
			}
		}

		void RefreshPickupList()
		{
			// while there are more gems in the list than the focused recipies require, pop the oldest elements of the list
			while (m_collectedGems.Count > m_allRecipes[m_recipeToFocusOn].m_recipe.GemsToPickup.Length)
				m_collectedGems.RemoveAt(0);
		}
	}
}