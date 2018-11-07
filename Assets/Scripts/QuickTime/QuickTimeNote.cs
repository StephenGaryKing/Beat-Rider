using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeatRider
{
	public class QuickTimeNote : MonoBehaviour {

		int m_val;
		public int Val { get { return m_val; } }

		public Text m_text;
		QuickTimeInput m_quickTimeInput;
		Renderer m_ren;
		Color m_defaultColour;

		private void Awake()
		{
			m_ren = GetComponent<Renderer>();
			m_defaultColour = m_ren.material.GetColor("_EmissionColor");
			m_quickTimeInput = FindObjectOfType<QuickTimeInput>();
		}

		private void OnEnable()
		{
			if (m_quickTimeInput.m_gameController.m_difficulty == Difficulty.HARD)
			{
				m_val = Random.Range(0, m_quickTimeInput.m_hardKeys.Length);
				float mag = GetMag(m_ren.material.GetColor("_EmissionColor"));
				m_ren.material.SetColor("_EmissionColor", m_quickTimeInput.m_hardKeys[m_val].NoteColour * mag);
				m_text.text = m_quickTimeInput.m_hardKeys[m_val].ToString();
			}
			else if(m_quickTimeInput.m_gameController.m_difficulty == Difficulty.MEDUIM)
			{
				float mag = GetMag(m_ren.material.GetColor("_EmissionColor"));
				m_ren.material.SetColor("_EmissionColor", m_quickTimeInput.m_meduimKey.NoteColour * mag);
				m_text.text = m_quickTimeInput.m_meduimKey.ToString();
			}
			else if (m_quickTimeInput.m_gameController.m_difficulty == Difficulty.EASY)
				m_ren.material.SetColor("_EmissionColor", m_defaultColour);
		}

		float GetMag(Color col)
		{
			return Mathf.Sqrt((col.r * col.r) + (col.g * col.g) + (col.b * col.b));
		}
	}
}