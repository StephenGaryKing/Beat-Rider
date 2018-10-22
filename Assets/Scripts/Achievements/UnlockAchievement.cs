using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAchievement : Achievement
{
	public PartToCustomise m_PartType;

	void Start()
	{
		AchievementManager.onCustomisation.AddListener(OnEvent);
	}

	protected override void OnEvent(string val)
	{
		if (val == m_PartType.ToString())
			Increment();
	}
}
