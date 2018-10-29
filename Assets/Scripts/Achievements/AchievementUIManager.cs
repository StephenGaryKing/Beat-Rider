using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct AchievementUI
{
	public GameObject Root;
	public Text Description;
	public Text Progress;
	public Achievement TargetAchievement;
}

public class AchievementUIManager : MonoBehaviour {

	public GameObject m_achievementContainerPrefab;
	public GameObject m_descriptionTextPrefab;
	public GameObject m_progressTextPrefab;
	AchievementManager m_achievementManager;
	List<AchievementUI> m_achievemntUIs;

	private void Start()
	{
		m_achievementManager = FindObjectOfType<AchievementManager>();
		if (!m_achievementManager)
			Debug.LogError("Add a Achievement Manager to the Game Controller");
		PopulateAchievements(m_achievementManager.m_Achievements);
	}

	void PopulateAchievements(Achievement[] achievements)
	{
		// clean out container
		int count = transform.childCount;
		for (int i = 0; i < count; i++)
			Destroy(transform.GetChild(0));

		if (m_achievementManager)
		{
			// populate container
			foreach (Achievement a in achievements)
			{
				GameObject achievementObject = Instantiate(m_achievementContainerPrefab, transform);
				AchievementUI newAchievementUI;
				newAchievementUI.Root = achievementObject;

				// create description display
				GameObject descriptionText = Instantiate(m_progressTextPrefab, achievementObject.transform);
				newAchievementUI.Description = descriptionText.GetComponent<Text>();
				newAchievementUI.Description.text = a.m_description;

				// create progress display


				// create  
			}
		}
	}

	public void UpdateAchievements()
	{

	}
}
