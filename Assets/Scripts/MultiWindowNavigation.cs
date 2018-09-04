using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public struct WindowItem
{
	public string Title;
	public Sprite TitleImage;
	public string FileLocationToLoad;
}

public class MultiWindowNavigation : MonoBehaviour {

	public GameObject m_templateTitle;
	public GameObject m_templateMenuItem;

	public float m_offsetDistance;
	public Vector3 m_offsetScale;
	public float m_animationSpeed = 0.5f;

	public List<WindowItem> m_menuItems = new List<WindowItem>();

	List<GameObject> m_titles = new List<GameObject>();
	List<GameObject> m_items = new List<GameObject>();
	int m_currentWindow = 0;

	// Use this for initialization
	void Start()
	{
		m_templateMenuItem.SetActive(false);
		m_templateTitle.SetActive(false);
		PopulateTitles();
		SnapUpdateWindows();
	}

	public void PopulateTitles()
	{
		m_titles.Clear();
		foreach (var item in m_menuItems)
		{
			GameObject newTitle = Instantiate(m_templateTitle, transform);
			newTitle.transform.localPosition = m_templateTitle.transform.localPosition;
			newTitle.name = item.Title;
			newTitle.GetComponentInChildren<Text>().text = "";
			if (item.TitleImage)
				newTitle.GetComponentInChildren<Image>().sprite = item.TitleImage;
			else
				newTitle.GetComponentInChildren<Text>().text = item.Title;
			newTitle.SetActive(true);
			m_titles.Add(newTitle);
		}
	}

	public void PopulateInfo(WindowItem item)
	{

	}

	void SnapUpdateWindows()
	{
		// find the titles to move
		GameObject[] titles = new GameObject[5];
		int index = 0;
		int titleSize = m_titles.Count;
		for (int i = m_currentWindow - 2; i <= m_currentWindow + 2; i++)
		{
			int realI = i;
			if (realI < 0)
				realI = titleSize + realI;
			else if (realI >= titleSize)
				realI = realI - titleSize;
			titles[index] = m_titles[realI];
			index++;
		}

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
		int titleSize = m_titles.Count;
		for (int i = m_currentWindow - 2; i <= m_currentWindow + 2; i ++)
		{
			int realI = i;
			if (realI < 0)
				realI = titleSize + realI;
			else if (realI >= titleSize)
				realI = realI - titleSize;
			titles[index] = m_titles[realI];
			index++;
		}

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
		m_currentWindow = (m_currentWindow >= m_titles.Count) ? 0 : m_currentWindow;
		UpdateWindows();
	}

	public void GotoPreviousWindow()
	{
		m_currentWindow--;
		m_currentWindow = (m_currentWindow < 0) ? m_titles.Count - 1 : m_currentWindow;
		UpdateWindows();
	}
	
}
