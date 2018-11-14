using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCustomiser : MonoBehaviour {

	public ParticleSystem m_trail;
	public Renderer m_body;
	public float m_highlightBrightness;

	List<Color> m_shipColour = new List<Color>();
	List<Color> m_highlightColour = new List<Color>();

	public void CustomiseColour(UnlockableColour colour)
	{
		m_body.material.color = colour.m_colour;
	}

	public void CustomiseHighlights(UnlockableColour highlight)
	{
		m_body.material.SetColor("_EmissionColor", highlight.m_colour * m_highlightBrightness);
	}

	public void CustomiseShip(UnlockableShip ship)
	{
		m_body.GetComponent<MeshFilter>().mesh = ship.m_model;
        m_body.GetComponent<Renderer>().material = ship.m_material;
	}

	public void CustomiseTrail(UnlockableTrail trail)
	{
		m_trail = trail.m_particle;
	}
}
