using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeatRider
{
	[RequireComponent(typeof(AchievementManager))]
	public abstract class Achievement : MonoBehaviour
	{
        public Unlockable m_unlockable;
        [TextArea]
        public string m_description;
		public int m_targetValue;
		public Sprite m_previewImage;
		//[SerializeField]
		protected int m_currentValue = 0;
		protected bool m_complete = false;
        AchievementManager m_achievementManager;

        private void Awake()
        {
            m_achievementManager = GetComponent<AchievementManager>();
        }

        public int CurrentValue
		{
			get { return m_currentValue; }
		}

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
				if (m_unlockable)
					m_achievementManager.UnlockAchievement(m_unlockable);
                //AchievementManager.SaveAchievements();
            }
		}
	}
}