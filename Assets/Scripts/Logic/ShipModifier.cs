using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeatRider
{
	public class ShipModifier : MonoBehaviour
	{
		public float m_highlightBrightness = 1;
		PlayerController m_player;
		Renderer m_playerRenderer;

		Color m_defaultBodyColour;
		Color m_defaultHighlightColour;
		Color m_defaultBoostColour;

		List<Color> m_bodyColour = new List<Color>();
		List<Color> m_highlightsColour = new List<Color>();
		List<Color> m_boostColour = new List<Color>();

		// Use this for initialization
		void Start()
		{
			m_player = FindObjectOfType<PlayerController>();
			// find the renderer attached to this ship
			m_playerRenderer = m_player.GetComponentInChildren<Renderer>();

			m_defaultBodyColour = m_playerRenderer.material.color;
			m_defaultHighlightColour = m_playerRenderer.material.GetColor("_EmissionColor");
		}

		Color AddColourToList(Color colour, ref List<Color> list)
		{
			list.Add(colour);
			Color newCol = Color.black;
			foreach (var col in list)
				newCol += col;
			newCol /= list.Count;
			return newCol;
		}

		Color RemoveColourFromList(Color colour, ref List<Color> list)
		{
			list.Remove(colour);
			Color newCol = Color.black;
			foreach (var col in list)
				newCol += col;
			newCol /= list.Count;
			return newCol;
		}

		public void ClearBodyColour ()
		{
			m_bodyColour.Clear();
			m_playerRenderer.material.color = m_defaultBodyColour;
		}

		public void ClearHighlightColour()
		{
			m_highlightsColour.Clear();
			Color newColour = m_defaultHighlightColour * m_highlightBrightness;
			m_playerRenderer.material.SetColor("_EmissionColor", newColour);
		}

		public void UpdateBodyColour(Toggle changeColourButton)
		{
			if (changeColourButton.isOn)
				m_playerRenderer.material.color = AddColourToList(changeColourButton.GetComponentInChildren<Image>().color, ref m_bodyColour);
			else
				m_playerRenderer.material.color = RemoveColourFromList(changeColourButton.GetComponentInChildren<Image>().color, ref m_bodyColour);

			if (float.IsNaN(m_playerRenderer.material.color.r))
				m_playerRenderer.material.color = m_defaultBodyColour;
		}

		public void UpdateHighlightsColour(Color colour)
		{
			Color newColour = AddColourToList(colour, ref m_highlightsColour) * m_highlightBrightness;
			m_playerRenderer.material.SetColor("_EmissionColor", newColour);
		}

		public void UpdateBoostColour(Color colour)
		{

		}
	}
}