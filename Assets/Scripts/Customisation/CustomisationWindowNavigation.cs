﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BeatRider;
using System.IO;

[System.Serializable]
public class WindowItem
{
	public string Title;
	public Sprite TitleImage;
	public CustomisationButton TemplateMenuItem;
	[HideInInspector] public GameObject TitleGameObject;
	[HideInInspector] public List<GameObject> MenuItemGameObjects = new List<GameObject>();
}

public class CustomisationWindowNavigation : MonoBehaviour {

	public GameObject m_templateTitle;

	public float m_offsetDistance;
	public Vector3 m_offsetScale;
	public float m_animationSpeed = 0.5f;

	public List<WindowItem> m_menuItems = new List<WindowItem>();

	UnlockableManager m_unlockableManager;
	int m_currentWindow = 0;


	// Use this for initialization
	void Start()
	{
		m_unlockableManager = FindObjectOfType<UnlockableManager>();
		m_templateTitle.SetActive(false);
		PopulateTitles();
		SnapUpdateWindows();
	}

	public void PopulateTitles()
	{
		for (int i = 0; i < m_menuItems.Count; i ++)
		{
			GameObject newTitle = Instantiate(m_templateTitle, transform);
			newTitle.transform.localPosition = m_templateTitle.transform.localPosition;
			newTitle.name = m_menuItems[i].Title;
			newTitle.GetComponentInChildren<Text>().text = "";
			if (m_menuItems[i].TitleImage)
				newTitle.GetComponentInChildren<Image>().sprite = m_menuItems[i].TitleImage;
			else
				newTitle.GetComponentInChildren<Text>().text = m_menuItems[i].Title;
			newTitle.SetActive(true);
			m_menuItems[i].TitleGameObject = newTitle;
			PopulateInfo(m_menuItems[i]);
			if (m_menuItems[i].TemplateMenuItem)
				m_menuItems[i].TemplateMenuItem.gameObject.SetActive(false);
		}
	}

	public void PopulateInfo (WindowItem item)
	{
		if (item.TemplateMenuItem)
		{
			// create buttons
			switch (item.TemplateMenuItem.m_partToCustomise)
			{
				case (PartToCustomise.Colour):
					foreach (var index in m_unlockableManager.m_unlockedColours)
					{
						GameObject newButton = Instantiate(item.TemplateMenuItem.gameObject, item.TemplateMenuItem.transform.parent);
						newButton.GetComponent<CustomisationButton>().SetUnlockable(m_unlockableManager.m_unlockableColours[index]);
						item.MenuItemGameObjects.Add(newButton);
					}
					break;
				case (PartToCustomise.Highlight):
					foreach (var index in m_unlockableManager.m_unlockedHighlights)
					{
						GameObject newButton = Instantiate(item.TemplateMenuItem.gameObject, item.TemplateMenuItem.transform.parent);
						newButton.GetComponent<CustomisationButton>().m_unlockable = m_unlockableManager.m_unlockableHighlights[index];
						item.MenuItemGameObjects.Add(newButton);
					}
					break;
				case (PartToCustomise.Ship):
					foreach (var index in m_unlockableManager.m_unlockedShips)
					{
						GameObject newButton = Instantiate(item.TemplateMenuItem.gameObject, item.TemplateMenuItem.transform.parent);
						newButton.GetComponent<CustomisationButton>().m_unlockable = m_unlockableManager.m_unlockableShips[index];
						item.MenuItemGameObjects.Add(newButton);
					}
					break;
				case (PartToCustomise.Trail):
					foreach (var index in m_unlockableManager.m_unlockedTrails)
					{
						GameObject newButton = Instantiate(item.TemplateMenuItem.gameObject, item.TemplateMenuItem.transform.parent);
						newButton.GetComponent<CustomisationButton>().m_unlockable = m_unlockableManager.m_unlockableTrails[index];
						item.MenuItemGameObjects.Add(newButton);
					}
					break;
			}
		}
	}

	void SnapUpdateWindows()
	{
		// find the titles to move
		GameObject[] titles = new GameObject[5];
		int index = 0;
		int titleSize = m_menuItems.Count;
		for (int i = m_currentWindow - 2; i <= m_currentWindow + 2; i++)
		{
			int realI = i;
			if (realI < 0)
				realI = titleSize + realI;
			else if (realI >= titleSize)
				realI = realI - titleSize;
			titles[index] = m_menuItems[realI].TitleGameObject;
			index++;
		}

		// hide buttons
		foreach (var item in m_menuItems)
			foreach (var btn in item.MenuItemGameObjects)
				btn.SetActive(false);

		// show buttons
		foreach (var btn in m_menuItems[m_currentWindow].MenuItemGameObjects)
			btn.SetActive(true);

		// position and scale the appropriate titles
		titles[0].transform.localPosition = m_templateTitle.transform.localPosition + Vector3.left * m_offsetDistance * 1.5f;
		titles[0].transform.localScale = Vector3.zero;

		titles[1].transform.localPosition = m_templateTitle.transform.localPosition + Vector3.left * m_offsetDistance;
		titles[1].transform.localScale = m_offsetScale;

		titles[2].transform.localPosition = m_templateTitle.transform.localPosition;
		titles[2].transform.localScale = m_templateTitle.transform.localScale;


		titles[3].transform.localPosition = m_templateTitle.transform.localPosition + Vector3.right * m_offsetDistance;
		titles[3].transform.localScale = m_offsetScale;

		titles[4].transform.localPosition = m_templateTitle.transform.localPosition + Vector3.right * m_offsetDistance * 1.5f;
		titles[4].transform.localScale = Vector3.zero;
	}

	void UpdateWindows()
	{
		// find the titles to move
		GameObject[] titles = new GameObject[5];
		int index = 0;
		int titleSize = m_menuItems.Count;
		for (int i = m_currentWindow - 2; i <= m_currentWindow + 2; i ++)
		{
			int realI = i;
			if (realI < 0)
				realI = titleSize + realI;
			else if (realI >= titleSize)
				realI = realI - titleSize;
			titles[index] = m_menuItems[realI].TitleGameObject;
			index++;
		}

		// hide buttons
		foreach (var item in m_menuItems)
			foreach (var btn in item.MenuItemGameObjects)
				btn.SetActive(false);

		// show buttons
		foreach (var btn in m_menuItems[m_currentWindow].MenuItemGameObjects)
			btn.SetActive(true);

		// position and scale the appropriate titles
		titles[0].transform.DOLocalMove(m_templateTitle.transform.localPosition + Vector3.left * m_offsetDistance * 1.5f, m_animationSpeed);
		titles[0].transform.DOScale(Vector3.zero, m_animationSpeed);

		titles[1].transform.DOLocalMove(m_templateTitle.transform.localPosition + Vector3.left * m_offsetDistance, m_animationSpeed);
		titles[1].transform.DOScale(m_offsetScale, m_animationSpeed);

		titles[2].transform.DOLocalMove(m_templateTitle.transform.localPosition, m_animationSpeed);
		titles[2].transform.DOScale(m_templateTitle.transform.localScale, m_animationSpeed);


		titles[3].transform.DOLocalMove(m_templateTitle.transform.localPosition + Vector3.right * m_offsetDistance, m_animationSpeed);
		titles[3].transform.DOScale(m_offsetScale, m_animationSpeed);

		titles[4].transform.DOLocalMove(m_templateTitle.transform.localPosition + Vector3.right * m_offsetDistance * 1.5f, m_animationSpeed);
		titles[4].transform.DOScale(Vector3.zero, m_animationSpeed);
	}

	public void GotoNextWindow()
	{
		m_currentWindow++;
		m_currentWindow = (m_currentWindow >= m_menuItems.Count) ? 0 : m_currentWindow;
		UpdateWindows();
	}

	public void GotoPreviousWindow()
	{
		m_currentWindow--;
		m_currentWindow = (m_currentWindow < 0) ? m_menuItems.Count - 1 : m_currentWindow;
		UpdateWindows();
	}
	
}