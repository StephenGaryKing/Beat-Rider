using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AchievementManager : MonoBehaviour {

	[System.Serializable]
	public class GameplayEvent : UnityEvent<string> { }

	public static GameplayEvent onCraft = new GameplayEvent();
	public static GameplayEvent onUnlock = new GameplayEvent();
	public static GameplayEvent onLevelPercent = new GameplayEvent();
	public static GameplayEvent onTallyPickups = new GameplayEvent();
	public static GameplayEvent onCustomisation = new GameplayEvent();
	public static GameplayEvent onCustomGameplayEvent = new GameplayEvent();

	Achievement[] m_Achievements;

	private void Start()
	{
		m_Achievements = GetComponents<Achievement>();
	}

	public static void SaveAchievements()
	{
		// get all the achievements on this object and serialize them
	}

	public static void LoadAchievements()
	{
		// deserialize the saved data and apply it to the achievements on this object
	}

	public static void OnCraft(string val)
	{
		onCraft.Invoke(val);
	}

	public static void OnUnlock(string val)
	{
		onUnlock.Invoke(val);
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


	public static void OnCustomGameplayEvent(string val)
	{
		onCustomGameplayEvent.Invoke(val);
	}
}
