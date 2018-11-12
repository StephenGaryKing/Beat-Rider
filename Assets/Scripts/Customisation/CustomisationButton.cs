using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeatRider
{
	public enum PartToCustomise
	{
		Colour,
		Highlight,
		Ship,
		Trail
	}

	[RequireComponent(typeof(Button))]
	public class CustomisationButton : MonoBehaviour
	{

		public PartToCustomise m_partToCustomise;
		[HideInInspector] public Unlockable m_unlockable;
		[HideInInspector] public ShipCustomiser m_shipCustomiser;

		// Use this for initialization
		void Start()
		{
			m_shipCustomiser = FindObjectOfType<ShipCustomiser>();
		}

		public void SetUnlockable(Unlockable unlock)
		{
			m_unlockable = unlock;
			Image image = GetComponent<Image>();
            if (m_unlockable.m_icon)
			    image.sprite = m_unlockable.m_icon;
			if (m_partToCustomise == PartToCustomise.Colour || m_partToCustomise == PartToCustomise.Highlight)
				image.color = (m_unlockable as UnlockableColour).m_colour;
		}

		public void ApplyCustomisation()
		{
			switch (m_partToCustomise)
			{
				case (PartToCustomise.Colour):
					m_shipCustomiser.CustomiseColour(m_unlockable as UnlockableColour);
					break;
				case (PartToCustomise.Highlight):
					m_shipCustomiser.CustomiseHighlights(m_unlockable as UnlockableColour);
					break;
				case (PartToCustomise.Ship):
					m_shipCustomiser.CustomiseShip(m_unlockable as UnlockableShip);
					break;
				case (PartToCustomise.Trail):
					m_shipCustomiser.CustomiseTrail(m_unlockable as UnlockableTrail);
					break;
			}

			Debug.Log(m_partToCustomise.ToString());
			AchievementManager.OnCustomisation(m_partToCustomise.ToString());
		}
	}
}