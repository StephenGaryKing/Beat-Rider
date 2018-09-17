using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Rarity
{
	Common,
	Rare,
	SuperRare
}

public class Unlockable : ScriptableObject {

	public Rarity m_rarity;
	public Sprite m_icon;
}
