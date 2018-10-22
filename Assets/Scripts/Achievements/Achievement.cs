using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AchievementManager))]
public abstract class Achievement : MonoBehaviour {

	public int m_targetValue;
	//[SerializeField]
	protected int m_currentValue = 0;
	protected bool m_complete = false;

	protected void Increment(int val = 1)
	{
		m_currentValue += val;
		if (m_currentValue > m_targetValue)
			Complete();
	}

	protected abstract void OnEvent(string val);

	protected void Complete()
	{
		if (!m_complete)
		{
			m_complete = true;
			AchievementManager.SaveAchievements();
		}
	}
}
