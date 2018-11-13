using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour {

	public Button[] m_buttons;
	public Color m_NormalColour = Color.grey;
	public Color m_selectedColour = Color.white;

	Button m_activeButton;

	// Use this for initialization
	void Start()
	{
		m_activeButton = m_buttons[0];
		SwitchToButton(m_activeButton);
	}

	public void SwitchToButton(Button btn)
	{
		m_activeButton = btn;

		foreach (Button b in m_buttons)
		{
			ColorBlock bColours = b.colors;
			bColours.normalColor = m_NormalColour;
			b.colors = bColours;
		}

		ColorBlock colours = m_activeButton.colors;
		colours.normalColor = m_selectedColour;
		m_activeButton.colors = colours;
	}
}
