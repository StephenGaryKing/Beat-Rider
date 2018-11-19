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
            if (!m_unlockableManager)
                m_unlockableManager = FindObjectOfType<UnlockableManager>();
            m_unlockableManager.UnlockUnlockable(unlock);
			SaveAchievements();
        }

		void SaveAchievements()
		{
			List<int> value = new List<int>();
			// get all the achievements on this object and serialize their value
			foreach (Achievement a in m_achievements)
				value.Add(a.CurrentValue);

			List<int> compleated = new List<int>();
			// get all the achievements on this object and serialize their value
			foreach (Achievement a in m_achievements)
				compleated.Add((a.Completed)? 1:0);

			SaveFile saver = new SaveFile();
			saver.AddList(value);
			saver.AddList(compleated);
			saver.Save(m_saveFileName);
		}

		void LoadAchievements()
		{
			// deserialize the saved data and apply it to the achievements on this object
			SaveFile loader = new SaveFile();
			if (loader.Load(m_saveFileName))
			{
				if (loader.m_numbers.Count > 0)
				{
					List<int> value = loader.m_numbers[0].list;
					for (int i = 0; i < value.Count; i++)
						m_achievements[i].CurrentValue = value[i];

					List<int> compleated = loader.m_numbers[1].list;
					for (int i = 0; i < compleated.Count; i++)
						if (compleated[i] == 1)
							m_achievements[i].Complete();
				}
			}
			else
			{
				Debug.LogError("generating file now");
				SaveAchievements();
			}
		}

		public void DeleteSaveData()
		{
			SaveFile DeleteFile = new SaveFile();
			DeleteFile.Delete(m_saveFileName);

			// reset data
			foreach (var achievement in m_achievements)
				achievement.Revert();
			m_unlockableManager.LockAll();
			LoadAchievements();
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