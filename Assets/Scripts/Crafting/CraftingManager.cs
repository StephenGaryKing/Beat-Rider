using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatRider
{
	public class CraftingManager : MonoBehaviour
	{
		public string m_saveFileName = "Crafting";

		public Gem[] m_ReipesToStartWith;
		public Gem[] m_allRecipes;
		public Image[] m_pickedupGemImageLocations;
		public Notification m_CraftingNotification;
		[HideInInspector] public List<Gem> m_collectedGems;
		[HideInInspector] public List<int> m_RecipesCompleated;
		public int m_filterNumber = 2;
		UnlockableManager m_UnlockableManager;
		List<Gem> m_filteredRecipes = new List<Gem>();
		List<Gem> m_recipesPendingcompletion = new List<Gem>();

		private void Start()
		{
			m_UnlockableManager = FindObjectOfType<UnlockableManager>();
			List<Gem> newAllRecipies = new List<Gem>(m_allRecipes);

			foreach (Gem recipe in m_ReipesToStartWith)
				if (FindGemIndex(recipe) == -1)
					newAllRecipies.Add(recipe);

			m_allRecipes = newAllRecipies.ToArray();

			LoadChallenges();
			DisplayPickupList();
		}

		void SaveChallenges()
		{
			SaveFile saveFile = new SaveFile();
			saveFile.AddList(m_RecipesCompleated);
			saveFile.Save(m_saveFileName);
		}

		void LoadChallenges()
		{
			SaveFile loadFile = new SaveFile();
			if (loadFile.Load(m_saveFileName))
			{
				List<int> recipesCompleated = loadFile.m_numbers[0].list;
				foreach (var recipe in recipesCompleated)
					UnlockChallenge(m_allRecipes[recipe]);
				CompletePendingCrafts();
			}
			else
			{
				Debug.LogError("generating file now");
				foreach (var recipe in m_ReipesToStartWith)
					UnlockChallenge(recipe);
				CompletePendingCrafts();
				SaveChallenges();
			}
		}

		public void Filter(int filter)
		{
			m_filterNumber = filter;
			RefreshFilteredRecipes();
			RefreshPickupList();
		}

		public int FindGemIndex(Gem Recipie)
		{
			int gemIndex = -1;
			for (int i = 0; i < m_allRecipes.Length; i++)
				if (m_allRecipes[i] == Recipie)
					gemIndex = i;

			if (gemIndex == -1)
				Debug.LogError("Recipie (" + Recipie + ") was not found in the Crafting Manager's \"All Recipies\" list.\nDid not unlock. Please add this recipe to the list");

			return gemIndex;
		}

		public void PickupGem(Gem gem)
		{
			m_collectedGems.Add(gem);
			RefreshPickupList();
			DisplayPickupList();
			Gem gemToUnlock = CheckPickupCombination();
			if (gemToUnlock)
				UnlockChallenge(gemToUnlock);
		}

		public void UnlockChallenge (Gem gem)
		{
			Sprite icon = gem.m_unlockable.m_icon;
			string notificationText = gem.name + " Discovered!";

			if (m_CraftingNotification)
				m_CraftingNotification.Notify((gem.m_unlockable as UnlockableColour).m_colour, icon, notificationText);

			m_recipesPendingcompletion.Add(gem);
		}

		public void CompletePendingCrafts()
		{
			foreach (Gem gem in m_recipesPendingcompletion)
			{
				int index = FindGemIndex(gem);
				if (!m_RecipesCompleated.Contains(index))
				{
					m_RecipesCompleated.Add(index);
					m_UnlockableManager.UnlockUnlockable(gem.m_unlockable);
				}
				AchievementManager.OnCraft("");
			}
			SaveChallenges();
		}

		public void ClearPendingCrafts()
		{
			m_recipesPendingcompletion.Clear();
		}

		Gem CheckPickupCombination()
		{
			if (m_collectedGems.Count != m_filterNumber)
				return null;

			// check to see if the collecte gems match any recipies that are being focused on
			// loop through the list of all recipes, 
			foreach(Gem gem in m_allRecipes)
			{
				// for all the recipes with the correct recipe size, 
				if (gem.m_recipe.GemsToPickup.Length == m_filterNumber)
				{
					// if it has not been unlocked already
					if (!m_RecipesCompleated.Contains(FindGemIndex(gem)))
					{
						bool gemIsAMatch = true;
						// check if the current list of picked up gems is the same
						for (int i = 0; i < m_collectedGems.Count; i++)
						{
							if (m_collectedGems[i] != gem.m_recipe.GemsToPickup[i])
								gemIsAMatch = false;
						}
						if (gemIsAMatch)
							return gem;
					}
				}
			}
			// everything matches up (return the gem)
			return null;
		}

		void DisplayPickupList()
		{
			if (m_pickedupGemImageLocations.Length < m_collectedGems.Count)
				Debug.LogError("Not enough image locations for CraftingManager!\nRequired " + m_collectedGems.Count + " image locations.");
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

		void RefreshFilteredRecipes()
		{
			m_filteredRecipes.Clear();
			// exclude any recipes that are finished
			// exclude recipes of the wrong size
			for(int i = 0; i < m_allRecipes.Length; i ++)
			{
				if (m_allRecipes[i].m_recipe.GemsToPickup.Length == m_filterNumber && !m_RecipesCompleated.Contains(i))
				{
					m_filteredRecipes.Add(m_allRecipes[i]);
				}
			}
		}

		void RefreshPickupList()
		{
			// while there are more gems in the list than the focused recipies require, pop the oldest elements of the list
			while (m_collectedGems.Count > m_filterNumber)
				m_collectedGems.RemoveAt(0);
		}
	}
}