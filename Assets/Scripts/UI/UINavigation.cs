using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINavigation : MonoBehaviour {

	public Button[] m_buttons;
	int m_currentButton;
	bool m_movingVert = false;
	bool m_movingHor = false;

	private void OnEnable()
	{
		m_currentButton = 0;
		UpdateButtons();
	}

	private void Update()
	{
		if (Input.GetAxisRaw("Horizontal") > 0.5f && !m_movingHor)
			Move(Vector2.right);
		if (Input.GetAxisRaw("Horizontal") < -0.5f && !m_movingHor)
			Move(Vector2.left);
		if (Input.GetAxisRaw("Vertical") > 0.5f && !m_movingVert)
			Move(Vector2.up);
		if (Input.GetAxisRaw("Vertical") < -0.5f && !m_movingVert)
			Move(Vector2.down);

		if (Input.GetAxisRaw("Horizontal") <= 0.5f && Input.GetAxisRaw("Horizontal") >= -0.5f)
			m_movingHor = false;
		else
			m_movingHor = true;
		if (Input.GetAxisRaw("Vertical") <= 0.5f && Input.GetAxisRaw("Vertical") >= -0.5f)
			m_movingVert = false;
		else
			m_movingVert = true;
	}

	void Move(Vector2 dir)
	{
		int btn = FindBtnInDir(dir);
		if (btn != -1)
		{
			m_currentButton = btn;
			UpdateButtons();
		}
	}

	int FindBtnInDir(Vector2 dir)
	{
		float bestDPSofar = 0;
		List<int> bestbuttonsSoFar = new List<int>();

		// look through all the buttons to find the most suitable button
		for(int i = 0; i < m_buttons.Length; i ++)
		{
			if (m_buttons[i].IsActive() && m_buttons[i].interactable)
			{
				// find the dot product between the direction of travel and (the button attempting to travel to in the local space of the current button)
				float dP = Vector2.Dot(((Vector2)m_buttons[i].transform.position - (Vector2)m_buttons[m_currentButton].transform.position).normalized, dir.normalized);
				if (dP > bestDPSofar)
				{
					bestbuttonsSoFar.Clear();
					bestbuttonsSoFar.Add(i);
					bestDPSofar = dP;
				}
				else if (dP == bestDPSofar)
					bestbuttonsSoFar.Add(i);
			}
		}
		Debug.Log(bestbuttonsSoFar.Count + " buttons found with a DP of " + bestDPSofar);

		if (bestbuttonsSoFar.Count == 0)
			return -1;

		if (bestbuttonsSoFar.Count > 1)
		{
			//find the button closest to the current button
			float bestDistanceSoFar = Mathf.Infinity;
			int bestButtonSoFar = m_currentButton;
			foreach (int btn in bestbuttonsSoFar)
			{

				float dist = Vector2.Distance(m_buttons[m_currentButton].transform.position, m_buttons[btn].transform.position);
				if (dist < bestDistanceSoFar)
				{
					bestDistanceSoFar = dist;
					bestButtonSoFar = btn;
				}
			}
			return bestButtonSoFar;
		}
		//return that
		return bestbuttonsSoFar[0];
	}

	void UpdateButtons()
	{
		int i = 0;
		foreach (Button btn in m_buttons)
		{
			/*
			TextSizeChange tsc = btn.GetComponent<TextSizeChange>();
			if (tsc)
				tsc.RollOffText();
			if (i == m_currentButton)
			{
				if (tsc)
					tsc.EnlargeText();
			}
			*/
			Tooltip tt2 = m_buttons[m_currentButton].GetComponent<Tooltip>();
			if (tt2)
				tt2.HideText();
			i++;
		}
		Tooltip tt = m_buttons[m_currentButton].GetComponent<Tooltip>();
		if (tt)
			tt.DisplayText();
		m_buttons[m_currentButton].Select();
		Debug.Log("selected button is " + m_buttons[m_currentButton].name);
	}
}
