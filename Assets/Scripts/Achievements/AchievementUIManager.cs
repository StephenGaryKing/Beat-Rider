using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct AchievementUI
{
	public GameObject Root;
	public Text Description;
	public Text Progress;
	public Image PreviewImage;
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
				newAchievementUI.TargetAchievement = a;

				// create description display
				GameObject descriptionText = Instantiate(m_progressTextPrefab, achievementObject.transform);
				newAchievementUI.Description = descriptionText.GetComponent<Text>();
				newAchievementUI.Description.text = a.m_description;

				// create progress display
				GameObject progressText = Instantiate(m_progressTextPrefab, achievementObject.transform);
				newAchievementUI.Progress = progressText.GetComponent<Text>();
				newAchievementUI.Progress.text = (newAchievementUI.TargetAchievement.CurrentValue + "/" + newAchievementUI.TargetAchievement.m_targetValue);

				// create preview image
				GameObject preview = new GameObject("Preview Image", typeof(RectTransform), typeof(Image));
				newAchievementUI.PreviewImage = preview.GetComponent<Image>();
				newAchievementUI.PreviewImage.sprite = newAchievementUI.TargetAchievement.m_previewImage;

				m_achievemntUIs.Add(newAchievementUI);
			}
		}
	}

	public void UpdateAchievements()
	{
		foreach(AchievementUI ui in m_achievemntUIs)
		{
			ui.Progress.text = (ui.TargetAchievement.CurrentValue + "/" + ui.TargetAchievement.m_targetValue);
		}
	}
}
