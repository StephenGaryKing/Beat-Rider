using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	/// <summary>
	/// Tally's the number of pickups collected with the tag 'Pickup Tag' (Currently performed after level is over)
	/// </summary>
	public class FinishLevelAchievement : Achievement
	{
        public Level m_requiredLevel;
        public Difficulty m_requiredDifficulty;

		void Start()
		{
			AchievementManager.onLevelPercent.AddListener(OnEvent);
		}

		protected override void OnEvent(string val)
		{
            string percent = "";
            string level = "";
			string difficulty = "";
            int index = 0;
			// percent
			for (int i = index; i < val.Length; i++)
			{
				if (val[index] == ':')
				{
					index++;
					break;
				}
				else
					percent += val[index];
				index++;
			}
			// level
			for (int i = index; i < val.Length; i++)
			{
				if (val[index] == ':')
				{
					index++;
					break;
				}
				else
					level += val[index];
				index++;
			}
			// difficulty
			for (int i = index; i < val.Length; i++)
			{
				if (val[index] == ':')
				{
					index++;
					break;
				}
				else
					difficulty += val[index];
				index++;
			}

			Debug.Log( level + " VS " + m_requiredLevel.name + " : " + percent + " VS " + m_targetValue + ":" + difficulty + " VS " + m_requiredDifficulty.ToString());

			if (level == m_requiredLevel.name)
				if (int.Parse(percent) >= m_targetValue)
					if (difficulty == m_requiredDifficulty.ToString())
						Complete();
		}
	}
}