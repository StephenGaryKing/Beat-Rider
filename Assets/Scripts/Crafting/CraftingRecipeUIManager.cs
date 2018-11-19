using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeatRider
{
	struct RecipeUI
	{
		public GameObject Root;
		public Image Product;
		public Image[] Ingredients;
		public Gem Recipe;
	}

	/// <summary>
	/// This script is placed on the UI element that holds the recipes
	/// </summary>
	public class CraftingRecipeUIManager : MonoBehaviour {

		public Sprite m_unknownGem;
		public Sprite m_defaultGem;
		public GameObject m_equalsSignPrefab;
		public GameObject m_plusSignPrefab;
		public GameObject m_recipeContainerPrefab;
		public FilterButton[] m_filterButtons;

		FilterButton m_lastFilterUsed; 
		List<RecipeUI> m_recipeUIs = new List<RecipeUI>();
		CraftingManager m_craftingManager;

		// Use this for initialization
		void Start() {
			m_craftingManager = FindObjectOfType<CraftingManager>();
			if (!m_craftingManager)
				Debug.LogError("Add a Crafting Manager to the Game Controller");
			PopulateRecipes(m_craftingManager.m_allRecipes);
            if (m_filterButtons.Length > 0)
			    Filter(m_filterButtons[0]);
		}

		public void Filter(FilterButton button)
		{
			m_lastFilterUsed = button;
			foreach (FilterButton btn in m_filterButtons)
			{
				if (btn == button)
				{
					SetSizeFilter(btn.m_filterNumber);
					btn.Activate();
				}
				else
					btn.DeActivate();
			}
		}

		public void PopulateRecipes(Gem[] recipes)
		{
			// clean out container
			int count = transform.childCount;
			for (int i = 0; i < count; i++)
				Destroy(transform.GetChild(0));

			// populate container
			foreach(Gem recipe in recipes)
			{
				if (recipe.m_recipe.GemsToPickup.Length > 0)
				{
					GameObject RecipeObject = Instantiate(m_recipeContainerPrefab, transform);
					RecipeUI newRecipe;
					newRecipe.Root = RecipeObject;

					// create ingredient displays
					newRecipe.Ingredients = new Image[recipe.m_recipe.GemsToPickup.Length];
					for (int i = 0; i < newRecipe.Ingredients.Length; i++)
					{
						newRecipe.Ingredients[i] = Instantiate(new GameObject("Ingredient " + i + 1, typeof(RectTransform), typeof(Image)), newRecipe.Root.transform).GetComponent<Image>();
						newRecipe.Ingredients[i].rectTransform.parent = newRecipe.Root.transform;
						newRecipe.Ingredients[i].sprite = m_defaultGem;
						// create plus
						if (i != newRecipe.Ingredients.Length - 1)
							Instantiate(m_plusSignPrefab, newRecipe.Root.transform);

					}

					// create equals
					Instantiate(m_equalsSignPrefab, newRecipe.Root.transform);

					// create product display
					newRecipe.Product = Instantiate(new GameObject("Product", typeof(RectTransform), typeof(Image)), newRecipe.Root.transform).GetComponent<Image>();
					newRecipe.Product.sprite = recipe.m_unlockable.m_icon;
					UnlockableColour caster = recipe.m_unlockable as UnlockableColour;
					if (caster)
						newRecipe.Product.color = caster.m_colour;

					newRecipe.Recipe = recipe;
					m_recipeUIs.Add(newRecipe);
				}
			}
		}

		void SetSizeFilter(int numOfIngredients)
		{
			Debug.Log("Now looking at recipes with a size of " + numOfIngredients);
			m_craftingManager.m_filterNumber = numOfIngredients;
			UpdateRecipes();
		}

		public void RefreshWindow()
		{
			Filter(m_lastFilterUsed);
		}

		public void UpdateRecipes()
		{
			foreach (RecipeUI ui in m_recipeUIs)
			{
				// look at ingredients
				for (int i = 0; i < ui.Recipe.m_recipe.GemsToPickup.Length; i++)
				{
					//Debug.Log(m_craftingManager.FindGemIndex(ui.Recipe.m_recipe.GemsToPickup[i]) + " :: " + num);
					// if the ingredient has been unlocked, show it, else, show the grey version
					if (m_craftingManager.m_RecipesCompleated.Contains(m_craftingManager.FindGemIndex(ui.Recipe.m_recipe.GemsToPickup[i])))
					{
						ui.Ingredients[i].sprite = ui.Recipe.m_recipe.GemsToPickup[i].m_unlockable.m_icon;
						UnlockableColour caster = ui.Recipe.m_recipe.GemsToPickup[i].m_unlockable as UnlockableColour;
						if (caster)
							ui.Ingredients[i].color = caster.m_colour;
					}
					else
					{
						ui.Ingredients[i].sprite = m_unknownGem;
						ui.Ingredients[i].color = Color.white;
					}
				}
			}

			// Show the relevent sized recipes
			foreach (RecipeUI ui in m_recipeUIs)
			{
				if (ui.Recipe.m_recipe.GemsToPickup.Length == m_craftingManager.m_filterNumber)
					ui.Root.SetActive(true);
				else
					ui.Root.SetActive(false);
				if (m_craftingManager.m_RecipesCompleated.Contains(m_craftingManager.FindGemIndex(ui.Recipe)))
					ui.Root.SetActive(false);
			}
		}
	}
}