using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BeatRider;
using Pixelplacement;
using System.IO;

[System.Serializable]
public class WindowItem
{
	public string Title;
	public Sprite TitleImage;
	public CustomisationButton TemplateMenuItem;
	public Button RecipeButton;
	[HideInInspector] public GameObject TitleGameObject;
	[HideInInspector] public List<GameObject> MenuItemGameObjects = new List<GameObject>();
}

public class CustomisationWindowNavigation : MonoBehaviour {

	public GameObject m_templateTitle;
	public Transform m_RecipeButtonLocation;

	public float m_offsetDistance;
	public Vector3 m_offsetScale;
	public float m_animationSpeed = 0.5f;
	// using this type of tween curve
	public TweenCurveHelper.CurveType m_curveType;

	public List<WindowItem> m_menuItems = new List<WindowItem>();

	UnlockableManager m_unlockableManager;
	int m_currentWindow = 0;


	// Use this for initialization
	void Awake()
	{
		m_unlockableManager = FindObjectOfType<UnlockableManager>();
		m_templateTitle.SetActive(false);
		PopulateTitles();
		SnapUpdateWindows();
		m_RecipeButtonLocation.gameObject.SetActive(false);
	}
	private void Start()
	{
		GotoNextWindow();
		GotoPreviousWindow();
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
			if (m_menuItems[i].RecipeButton)
			{
				m_menuItems[i].RecipeButton.gameObject.SetActive(false);
				m_menuItems[i].TitleGameObject = newTitle;
			}
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
					foreach (var index in m_unlockableManager.m_unlockableColours)
					{
						GameObject newButton = Instantiate(item.TemplateMenuItem.gameObject, item.TemplateMenuItem.transform.parent);
						newButton.GetComponent<CustomisationButton>().SetUnlockable(index);
						item.MenuItemGameObjects.Add(newButton);
					}
					break;
				case (PartToCustomise.Highlight):
					foreach (var index in m_unlockableManager.m_unlockableHighlights)
					{
						GameObject newButton = Instantiate(item.TemplateMenuItem.gameObject, item.TemplateMenuItem.transform.parent);
						newButton.GetComponent<CustomisationButton>().SetUnlockable(index);
						item.MenuItemGameObjects.Add(newButton);
					}
					break;
				case (PartToCustomise.Ship):
					foreach (var index in m_unlockableManager.m_unlockableShips)
					{
						GameObject newButton = Instantiate(item.TemplateMenuItem.gameObject, item.TemplateMenuItem.transform.parent);
						newButton.GetComponent<CustomisationButton>().SetUnlockable(index);
						item.MenuItemGameObjects.Add(newButton);
					}
					break;
				case (PartToCustomise.Trail):
					foreach (var index in m_unlockableManager.m_unlockableTrails)
					{
						GameObject newButton = Instantiate(item.TemplateMenuItem.gameObject, item.TemplateMenuItem.transform.parent);
						newButton.GetComponent<CustomisationButton>().SetUnlockable(index);
						item.MenuItemGameObjects.Add(newButton);
					}
					break;
			}
		}
	}

	public void UpdateInfo(WindowItem item)
	{
		foreach (GameObject go in item.MenuItemGameObjects)
		{
			// Default is hidden from the user
			go.SetActive(false);
			//Debug.Log(go.name + " : " + go.activeInHierarchy);

			switch (item.TemplateMenuItem.m_partToCustomise)
			{
				case (PartToCustomise.Colour):
					foreach (int index in m_unlockableManager.m_unlockedColours)
					{
						UnlockableColour col = go.GetComponent<CustomisationButton>().m_unlockable as UnlockableColour;
						if (index == m_unlockableManager.FindUnlockedID(col, m_unlockableManager.m_unlockableColours))
							go.SetActive(true);
					}
					break;
				case (PartToCustomise.Highlight):
					foreach (var index in m_unlockableManager.m_unlockedHighlights)
					{
						UnlockableHighlight col = go.GetComponent<CustomisationButton>().m_unlockable as UnlockableHighlight;
						if (index == m_unlockableManager.FindUnlockedID(col, m_unlockableManager.m_unlockableHighlights))
							go.SetActive(true);
					}
					break;
				case (PartToCustomise.Ship):
					foreach (var index in m_unlockableManager.m_unlockedShips)
					{
						UnlockableShip col = go.GetComponent<CustomisationButton>().m_unlockable as UnlockableShip;
						if (index == m_unlockableManager.FindUnlockedID(col, m_unlockableManager.m_unlockableShips))
							go.SetActive(true);
					}
					break;
				case (PartToCustomise.Trail):
					foreach (var index in m_unlockableManager.m_unlockedTrails)
					{
						UnlockableTrail col = go.GetComponent<CustomisationButton>().m_unlockable as UnlockableTrail;
						if (index == m_unlockableManager.FindUnlockedID(col, m_unlockableManager.m_unlockableTrails))
							go.SetActive(true);
					}
					break;
			}

			//Debug.Log(go.name + " : " + go.activeInHierarchy);
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
		UpdateInfo(m_menuItems[m_currentWindow]);

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
		Debug.Log(m_menuItems[m_currentWindow].Title + " has " + m_menuItems[m_currentWindow].MenuItemGameObjects.Count + " objects");
		UpdateInfo(m_menuItems[m_currentWindow]);

		// position and scale the appropriate titles

		// left 2
		RectTransform rt = titles[0].transform as RectTransform;
		if (rt)
			Tween.AnchoredPosition(rt, m_templateTitle.transform.localPosition + Vector3.left * m_offsetDistance * 1.5f, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);
		else
			Tween.LocalPosition(rt, m_templateTitle.transform.localPosition + Vector3.left * m_offsetDistance * 1.5f, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);
		Tween.LocalScale(titles[0].transform, Vector3.zero, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);
		
		// left
		rt = titles[1].transform as RectTransform;
		if (rt)
			Tween.AnchoredPosition(rt, m_templateTitle.transform.localPosition + Vector3.left * m_offsetDistance, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);
		else
			Tween.LocalPosition(rt, m_templateTitle.transform.localPosition + Vector3.left * m_offsetDistance, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);
		Tween.LocalScale(titles[1].transform, m_offsetScale, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);

		// middle
		rt = titles[2].transform as RectTransform;
		if (rt)
			Tween.AnchoredPosition(rt, m_templateTitle.transform.localPosition, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);
		else
			Tween.LocalPosition(rt, m_templateTitle.transform.localPosition, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);
		Tween.LocalScale(titles[2].transform, m_templateTitle.transform.localScale, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);

		// right
		rt = titles[3].transform as RectTransform;
		if (rt)
			Tween.AnchoredPosition(rt, m_templateTitle.transform.localPosition + Vector3.right * m_offsetDistance, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);
		else
			Tween.LocalPosition(rt, m_templateTitle.transform.localPosition + Vector3.right * m_offsetDistance, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);
		Tween.LocalScale(titles[3].transform, m_offsetScale, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);

		// right 2
		rt = titles[4].transform as RectTransform;
		if (rt)
			Tween.AnchoredPosition(rt, m_templateTitle.transform.localPosition + Vector3.right * m_offsetDistance * 1.5f, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);
		else
			Tween.LocalPosition(rt, m_templateTitle.transform.localPosition + Vector3.right * m_offsetDistance * 1.5f, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);
		Tween.LocalScale(titles[4].transform, Vector3.zero, m_animationSpeed, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);
	}

	public void GotoNextWindow()
	{
		if (m_menuItems[m_currentWindow].RecipeButton)
			m_menuItems[m_currentWindow].RecipeButton.gameObject.SetActive(false);
		m_currentWindow++;
		m_currentWindow = (m_currentWindow >= m_menuItems.Count) ? 0 : m_currentWindow;
		if (m_menuItems[m_currentWindow].RecipeButton)
		{
			m_menuItems[m_currentWindow].RecipeButton.transform.position = m_RecipeButtonLocation.position;
			m_menuItems[m_currentWindow].RecipeButton.gameObject.SetActive(true);
		}
		UpdateWindows();
	}

	public void GotoPreviousWindow()
	{
		if (m_menuItems[m_currentWindow].RecipeButton)
			m_menuItems[m_currentWindow].RecipeButton.gameObject.SetActive(false);
		m_currentWindow--;
		m_currentWindow = (m_currentWindow < 0) ? m_menuItems.Count - 1 : m_currentWindow;
		if (m_menuItems[m_currentWindow].RecipeButton)
		{
			m_menuItems[m_currentWindow].RecipeButton.transform.position = m_RecipeButtonLocation.position;
			m_menuItems[m_currentWindow].RecipeButton.gameObject.SetActive(true);
		}
		UpdateWindows();
	}
	
}
