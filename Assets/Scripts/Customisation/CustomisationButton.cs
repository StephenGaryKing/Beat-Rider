using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PartToCustomise
{
	Colour,
	Highlight,
	Ship,
	Trail
}

[RequireComponent(typeof(Toggle))]
public class CustomisationButton : MonoBehaviour {

	public PartToCustomise m_partToCustomise;
	[HideInInspector] public Unlockable m_unlockable;
	[HideInInspector] public ShipCustomiser m_shipCustomiser;
	Toggle m_toggle;

	// Use this for initialization
	void Start()
	{
		m_toggle = GetComponent<Toggle>();
		m_shipCustomiser = FindObjectOfType<ShipCustomiser>();
	}

	public void SetUnlockable(Unlockable unlock)
	{
		m_unlockable = unlock;
		Image image = GetComponent<Image>();
		image.sprite = m_unlockable.m_icon;
		if (m_partToCustomise == PartToCustomise.Colour || m_partToCustomise == PartToCustomise.Highlight)
			image.color = (m_unlockable as UnlockableColour).m_colour;
	}

	public void ApplyCustomisation()
	{
		switch (m_partToCustomise)
		{
			case (PartToCustomise.Colour):
				m_shipCustomiser.CustomiseColour(m_unlockable as UnlockableColour, m_toggle.isOn);
				break;
			case (PartToCustomise.Highlight):
				m_shipCustomiser.CustomiseHighlights(m_unlockable as UnlockableColour, m_toggle.isOn);
				break;
			case (PartToCustomise.Ship):
				m_shipCustomiser.CustomiseShip(m_unlockable as UnlockableShip, m_toggle.isOn);
				break;
			case (PartToCustomise.Trail):
				m_shipCustomiser.CustomiseTrail(m_unlockable as UnlockableTrail, m_toggle.isOn);
				break;
		}
	}
}
