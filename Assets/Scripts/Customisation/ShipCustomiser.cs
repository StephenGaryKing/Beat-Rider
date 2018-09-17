using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCustomiser : MonoBehaviour {

	public ParticleSystem m_trail;
	public Renderer m_body;
	public float m_highlightBrightness;

	List<Color> m_shipColour = new List<Color>();
	List<Color> m_highlightColour = new List<Color>();

	public void CustomiseColour(UnlockableColour colour, bool on)
	{
		if (on)
			m_shipColour.Add(colour.m_colour);
		else
		{
			if (m_shipColour.Contains(colour.m_colour))
				m_shipColour.Remove(colour.m_colour);
		}

		Color newCol = Color.black;
		foreach (var col in m_shipColour)
			newCol += col;
		newCol /= m_shipColour.Count;

		if (m_shipColour.Count == 0)
			newCol = Color.white;

		m_body.material.color = newCol;
	}

	public void CustomiseHighlights(UnlockableColour highlight, bool on)
	{
		if (on)
			m_highlightColour.Add(highlight.m_colour);
		else
		{
			if (m_highlightColour.Contains(highlight.m_colour))
				m_highlightColour.Remove(highlight.m_colour);
		}

		Color newCol = Color.black;
		foreach (var col in m_highlightColour)
			newCol += col;
		newCol /= m_highlightColour.Count;

		if (m_highlightColour.Count == 0)
			newCol = Color.white;

		m_body.material.SetColor("_EmissionColor", newCol * m_highlightBrightness);
	}

	public void CustomiseShip(UnlockableShip ship, bool on)
	{
		m_body.GetComponent<MeshFilter>().mesh = ship.m_model;
	}

	public void CustomiseTrail(UnlockableTrail trail, bool on)
	{
		m_trail = trail.m_particle;
	}
}
