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

		public Sprite m_greyedOutGem;
		public GameObject m_equalsSignPrefab;
		public GameObject m_plusSignPrefab;
		public GameObject m_recipeContainerPrefab;

		List<RecipeUI> m_recipeUIs = new List<RecipeUI>();
		int m_numOfIngredients = 2;
		CraftingManager m_craftingManager;

		// Use this for initialization
		void Start() {
			m_craftingManager = FindObjectOfType<CraftingManager>();
			PopulateRecipes(m_craftingManager.m_allRecipes);
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

					// create product display
					newRecipe.Product = Instantiate(new GameObject("Product", typeof(RectTransform), typeof(Image))).GetComponent<Image>();
					newRecipe.Product.transform.parent = newRecipe.Root.transform;
					newRecipe.Product.sprite = recipe.m_unlockable.m_icon;
					UnlockableColour caster = recipe.m_unlockable as UnlockableColour;
					if (caster)
						newRecipe.Product.color = caster.m_colour;

					// create equals
					Instantiate(m_equalsSignPrefab, newRecipe.Root.transform);

					// create ingredient displays
					newRecipe.Ingredients = new Image[recipe.m_recipe.GemsToPickup.Length];
					for (int i = 0; i < newRecipe.Ingredients.Length; i++)
					{
						newRecipe.Ingredients[i] = Instantiate(new GameObject("Ingredient " + i + 1, typeof(RectTransform), typeof(Image))).GetComponent<Image>();
						newRecipe.Ingredients[i].transform.parent = newRecipe.Root.transform;
						newRecipe.Ingredients[i].sprite = m_greyedOutGem;
						// create plus
						if (i != newRecipe.Ingredients.Length - 1)
							Instantiate(m_plusSignPrefab, newRecipe.Root.transform);
					}

					newRecipe.Recipe = recipe;

					m_recipeUIs.Add(newRecipe);
				}
			}
		}

		public void SetSizeFilter(int numOfIngredients)
		{
			Debug.Log("Update Test");
			m_numOfIngredients = numOfIngredients;
			UpdateRecipes();
		}

		public void UpdateRecipes()
		{
			// look for each recipe in the manager to decide wether to show the recipe or not
			foreach (int num in m_craftingManager.m_RecipesCompleated)
			{
				foreach (RecipeUI ui in m_recipeUIs)
				{
					// look at ingredients
					for (int i = 0; i < ui.Recipe.m_recipe.GemsToPickup.Length; i ++)
					{
						// if the ingredient has been unlocked, show it, else, show the grey version
						if (m_craftingManager.FindGemIndex(ui.Recipe.m_recipe.GemsToPickup[i]) == num)
							ui.Ingredients[i].sprite = ui.Recipe.m_recipe.GemsToPickup[i].m_unlockable.m_icon;
						else
							ui.Ingredients[i].sprite = m_greyedOutGem;
					}

				}
			}
		}
	}
}