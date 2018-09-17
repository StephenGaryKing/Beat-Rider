using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartToCustomise
{
	Colour,
	Highlight,
	Ship,
	Trail
}

public class CustomisationButton : MonoBehaviour {

	public PartToCustomise m_partToCustomise;
	[HideInInspector] public Unlockable m_unlockable;
	ShipCustomiser m_shipCustomiser;

	// Use this for initialization
	void Start()
	{
		m_shipCustomiser = FindObjectOfType<ShipCustomiser>();
	}

	public void ApplyCustomisation()
	{
		switch (m_partToCustomise)
		{
			case (PartToCustomise.Colour):
				//m_shipCustomiser.CustomiseColour(m_unlockable as UnlockableColour);
				break;
			case (PartToCustomise.Highlight):
				//m_shipCustomiser.CustomiseHighlights(m_unlockable as UnlockableColour);
				break;
			case (PartToCustomise.Ship):
				m_shipCustomiser.CustomiseShip(m_unlockable as UnlockableShip);
				break;
			case (PartToCustomise.Trail):
				m_shipCustomiser.CustomiseTrail(m_unlockable as UnlockableTrail);
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
