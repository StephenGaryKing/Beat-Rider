using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of the number of gems crafted
/// </summary>
public class CraftAchievement : Achievement
{
	void Start()
	{
		AchievementManager.onCraft.AddListener(OnEvent);
	}

	protected override void OnEvent(string val)
	{
		Increment();
	}
}
