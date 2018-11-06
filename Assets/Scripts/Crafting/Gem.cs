using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Recipe
{
	public Gem[] GemsToPickup;
}

[CreateAssetMenu(fileName = "Recipe", menuName = "Beat Rider/Crafting/Gem")]
public class Gem : ScriptableObject {

	public Recipe m_recipe;
	public Unlockable m_unlockable;
	public Material m_material;
}
