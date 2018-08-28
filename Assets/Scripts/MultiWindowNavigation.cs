using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct WindowItem
{
	public string Title;
	public string FileLocationToLoad;
}

public class MultiWindowNavigation : MonoBehaviour {

	public GameObject m_templateTitle;
	public GameObject m_templateMenuItem;

	public float m_offsetDistance;
	public Vector3 m_offsetScale;

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
		UpdateWindows();
	}

	public void PopulateTitles()
	{
		m_titles.Clear();
		foreach (var item in m_menuItems)
		{
			GameObject newTitle = Instantiate(m_templateTitle, transform);
			newTitle.transform.localPosition = m_templateTitle.transform.localPosition;
			newTitle.name = item.Title;
			newTitle.GetComponentInChildren<Text>().text = item.Title;
			newTitle.SetActive(true);
			m_titles.Add(newTitle);
		}
	}

	public void PopulateInfo(WindowItem item)
	{

	}

	void UpdateWindows()
	{
		// disable all the titles
		foreach (var title in m_titles)
			title.SetActive(false);

		// position and enable the appropriate titles
		GameObject[] titles = new GameObject[3];
		titles[0] = m_titles[(m_currentWindow == 0) ? m_titles.Count - 1 : m_currentWindow - 1];
		titles[1] = m_titles[m_currentWindow];
		titles[2] = m_titles[(m_currentWindow == m_titles.Count - 1) ? 0 : m_currentWindow + 1];

		// position and scale
		titles[0].transform.localPosition = m_templateTitle.transform.localPosition + Vector3.left * m_offsetDistance;
		titles[0].transform.localScale = m_offsetScale;
		titles[1].transform.localPosition = m_templateTitle.transform.localPosition;
		titles[1].transform.localScale = m_templateTitle.transform.localScale;
		titles[2].transform.localPosition = m_templateTitle.transform.localPosition + Vector3.right * m_offsetDistance;
		titles[2].transform.localScale = m_offsetScale;

		// enable this title and each one on either side
		foreach (GameObject title in titles)
			title.SetActive(true);
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
	
	// Update is called once per frame
	void Update () {
		
	}
}
