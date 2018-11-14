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
		public float m_requiredPercentage;
        public Level m_requiredLevel;
        public Difficulty m_requiredDifficulty;

		void Start()
		{
			AchievementManager.onLevelPercent.AddListener(OnEvent);
		}

		protected override void OnEvent(string val)
		{
            string subStr1 = "";
            string subStr2 = "";
            int index = 0;
            foreach (char c in val)
            {
                if (c == ':')
                {
                    break;
                }
                else
                    subStr1 += c;
                index++;
            }
            subStr2 = val.Substring(index, val.Length-1);

            Debug.Log("completed level " + subStr2 + " with " + subStr1);

            if (subStr2 == m_requiredLevel.name)
                if (int.Parse(subStr1) >= m_requiredPercentage)
                    CheckFinalCondition();
		}

		void CheckFinalCondition()
		{
            Complete();
		}
	}
}