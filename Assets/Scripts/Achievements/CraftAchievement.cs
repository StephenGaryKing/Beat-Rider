using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
