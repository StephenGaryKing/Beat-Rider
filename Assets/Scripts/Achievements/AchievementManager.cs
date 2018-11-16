using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BeatRider
{
	public class AchievementManager : MonoBehaviour
	{

		[System.Serializable]
		public class GameplayEvent : UnityEvent<string> { }

        public string m_saveFileName = "AchievementProgress";

		public static GameplayEvent onCraft = new GameplayEvent();
		public static GameplayEvent onLevelPercent = new GameplayEvent();
		public static GameplayEvent onTallyPickups = new GameplayEvent();
		public static GameplayEvent onCustomisation = new GameplayEvent();

		[HideInInspector] public Achievement[] m_achievements;
        UnlockableManager m_unlockableManager;

		private void Start()
		{
            m_unlockableManager = FindObjectOfType<UnlockableManager>();
			m_achievements = GetComponents<Achievement>();
			LoadAchievements();
		}

        public void UnlockAchievement(Unlockable unlock)
        {
            m_unlockableManager.UnlockUnlockable(unlock);
			SaveAchievements();
        }

		public void SaveAchievements()
		{
			List<int> m_data = new List<int>();
			// get all the achievements on this object and serialize them
			foreach (Achievement a in m_achievements)
				m_data.Add(a.CurrentValue);
			SaveFile saver = new SaveFile();
			saver.AddList(m_data);
			saver.Save(m_saveFileName);
		}

		public void LoadAchievements()
		{
			// deserialize the saved data and apply it to the achievements on this object
			SaveFile loader = new SaveFile();
			loader.Load(m_saveFileName);
			List<int> data = loader.m_numbers[0].list;

			for (int i = 0; i < data.Count; i ++)
				m_achievements[i].CurrentValue = data[i];
		}

		public static void OnCraft(string val)
		{
			onCraft.Invoke(val);
		}

		public static void OnCustomisation(string val)
		{
			onCustomisation.Invoke(val);
		}

		public static void OnLevelPercent(string val)
		{
			onLevelPercent.Invoke(val);
		}

		public static void OnTallyPickups(string val)
		{
			onTallyPickups.Invoke(val);
		}
	}
}