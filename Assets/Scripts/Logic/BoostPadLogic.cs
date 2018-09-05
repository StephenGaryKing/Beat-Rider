using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class BoostPadLogic : MonoBehaviour
	{

		// this value is used when the player hits the boost pad
		public float m_boostAmount = 10;
		public Renderer[] m_arrows;
		public float m_maxEmission;
		public float m_minEmission;
		public bool m_invertLight = false;

		float m_maxVal;
		Color m_arrowColour;
		PlayerCollision m_player;

		private void Start()
		{
			m_player = FindObjectOfType<PlayerCollision>();
			if (m_arrows.Length != 0)
				m_arrowColour = m_arrows[0].material.GetColor("_EmissionColor");
			m_maxVal = m_player.m_maxFOV - m_player.m_minFOV;
		}

		private void Update()
		{
			// scale the lighting of the boost pads with the speed travelling
			// find the percentage of max speed
			float speedPercent = (m_player.m_targetFOV - m_player.m_minFOV) / (m_maxVal - (m_player.m_maxFOV - m_player.m_minFOV) / 5);
			for (int i = 0; i < m_arrows.Length; i++)
				UpdateArrow(i, speedPercent);
		}

		private void UpdateArrow(int i, float speedPercent)
		{
			// find the percentage of the current arrow through the list of arrows
			float percentThroughList = (i + 1) / (float)m_arrows.Length;

			if (percentThroughList <= speedPercent)
				m_arrows[(m_invertLight) ? m_arrows.Length - 1 - i : i].material.SetColor("_EmissionColor", m_arrowColour * ((m_invertLight) ? m_minEmission : m_maxEmission));
			else
				m_arrows[(m_invertLight) ? m_arrows.Length - 1 - i : i].material.SetColor("_EmissionColor", m_arrowColour * ((m_invertLight) ? m_maxEmission : m_minEmission));
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Obstacle"))
				gameObject.SetActive(false);
		}
	}
}