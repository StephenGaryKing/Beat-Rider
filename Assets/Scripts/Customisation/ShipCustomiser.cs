using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCustomiser : MonoBehaviour {

	public ParticleSystem m_trail;
	public Renderer m_body;
	public float m_highlightBrightness;

    Mesh m_defaultModel;
    Material m_defaultMaterial;
    Color m_defaultColour;
    Color m_defaultHighlight;

	List<Color> m_shipColour = new List<Color>();
	List<Color> m_highlightColour = new List<Color>();

    private void Start()
    {
        m_defaultModel = m_body.GetComponent<MeshFilter>().mesh;
        m_defaultMaterial = m_body.GetComponent<Renderer>().material;
        m_defaultColour = m_body.material.color;
        m_defaultHighlight = m_body.material.GetColor("_EmissionColor");
    }

    public void CustomiseColour(UnlockableColour colour)
	{

        m_body.material.color = colour.m_colour;
	}

	public void CustomiseHighlights(UnlockableHighlight highlight)
	{
		m_body.material.SetColor("_EmissionColor", highlight.m_colour * m_highlightBrightness);
	}

	public void CustomiseShip(UnlockableShip ship)
	{
        Color tempColour = m_body.material.color;
        Color tempHighlight = m_body.material.GetColor("_EmissionColor");
        tempHighlight /= m_highlightBrightness;

        m_body.GetComponent<MeshFilter>().mesh = ship.m_model;
        m_body.GetComponent<Renderer>().material = ship.m_material;

        m_body.material.color = tempColour;
        m_body.material.SetColor("_EmissionColor", tempHighlight * m_highlightBrightness);
    }

    public void ResetShip()
    {
        m_body.GetComponent<MeshFilter>().mesh = m_defaultModel;
        m_body.GetComponent<Renderer>().material = m_defaultMaterial;
        m_body.material.color = m_defaultColour;
        m_body.material.SetColor("_EmissionColor", m_defaultHighlight * m_highlightBrightness);
    }

	public void CustomiseTrail(UnlockableTrail trail)
	{
        
	}
}
