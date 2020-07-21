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

    [SerializeField] private UnlockableShip m_currentShip = null;

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
        if (colour.m_isMetallic)
        {
            m_body.material.SetTexture("_MetallicGlossMap", null);
            m_body.material.SetFloat("_Metallic", 1f);
            m_body.material.SetFloat("_Glossiness", 1f);
        }
        else
        {
            if (!m_currentShip)
            {
                Debug.Log("Current Ship not set up");
            }
            else
            {
                m_body.material.SetTexture("_MetallicGlossMap", m_currentShip.m_metallicTexture);
                m_body.material.SetFloat("_Glossiness", 1f);
            }
        }
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

        bool metallicStatus = false;
        float metallicValue = 0f;
        float smoothnessValue = 0f;

        if (m_body.material.GetTexture("_MetallicGlossMap") == null)
        {
            metallicStatus = true;
            metallicValue = m_body.material.GetFloat("_Metallic");
            smoothnessValue = m_body.material.GetFloat("_Glossiness");
        }

        m_body.GetComponent<MeshFilter>().mesh = ship.m_model;
        m_body.GetComponent<Renderer>().material = ship.m_material;
        m_currentShip = ship;

        m_body.material.color = tempColour;
        m_body.material.SetColor("_EmissionColor", tempHighlight * m_highlightBrightness);
        if (metallicStatus)
        {
            m_body.material.SetTexture("_MetallicGlossMap", null);
            m_body.material.SetFloat("_Metallic", metallicValue);
            m_body.material.SetFloat("_Glossiness", smoothnessValue);
        }
        else
        {
            m_body.material.SetTexture("_MetallicGlossMap", ship.m_metallicTexture);
            m_body.material.SetFloat("_Glossiness", smoothnessValue);
        }
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
