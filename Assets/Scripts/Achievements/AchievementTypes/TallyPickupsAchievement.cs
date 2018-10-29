using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tally's the number of pickups collected with the tag 'Pickup Tag' (Currently performed after level is over)
/// </summary>
public class TallyPickupsAchievement : Achievement
{
	public string m_pickupTag;
	public bool m_lessThanOrEqualTo = false;

	void Start()
	{
		AchievementManager.onTallyPickups.AddListener(OnEvent);
	}

	protected override void OnEvent(string val)
	{
		if (val == m_pickupTag)
			m_currentValue++;
		if (val == ("Final"))
			CheckFinalCondition();
	}

	void CheckFinalCondition()
	{
		if (m_lessThanOrEqualTo)
			if (m_currentValue <= m_targetValue)
				Complete();
			else
			if (m_currentValue > m_targetValue)
				Complete();
		m_currentValue = 0;
	}
}
