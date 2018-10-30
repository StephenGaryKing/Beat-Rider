using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	/// <summary>
	/// Keeps track of the number of unlocks of a specific type have been unlocked
	/// </summary>
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
}