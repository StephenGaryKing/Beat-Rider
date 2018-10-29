using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of how many times a part has been customised
/// </summary>
public class CustomiseAchievement : Achievement
{
	public PartToCustomise m_partType;

	void Start()
	{
		AchievementManager.onCustomisation.AddListener(OnEvent);
	}

	protected override void OnEvent(string val)
	{
		if (val.Equals(m_partType.ToString()))
			Increment();
	}
}
