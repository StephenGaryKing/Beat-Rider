using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

namespace BeatRider
{
	public class EmissionFlasher : MonoBehaviour
	{

		// what to react to
		[SerializeField] protected string m_passToReactTo = "DEFAULT";

		public AudioReactor m_audioReactor;
		Renderer m_renderer;

		public float m_smoothing = 0.1f;
		public Color m_startingColour = Color.black;
		public Color m_endingColour = Color.white;
		public float m_emissionMagnitude = 1;

		Color m_tempColour = Color.black;

		// Use this for initialization
		void Start()
		{
			m_renderer = GetComponent<Renderer>();
			Material tempMat = new Material(m_renderer.material);
			m_renderer.material = tempMat;
			SongController.m_onMusicIsPlaying.AddListener(React);
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			m_tempColour = Color.Lerp(m_tempColour, m_startingColour, m_smoothing);
			m_renderer.material.SetColor("_EmissionColor", m_tempColour * m_emissionMagnitude);
		}

		void React(List<SavedPass> passes, int index)
		{
			foreach (SavedPass pass in passes)
			{
				if (m_passToReactTo != "DEFAULT" && pass.name != m_passToReactTo)
					continue;
				if (!pass.runtimeData[index].isPeak)
					continue;
				if (m_audioReactor)
					if (m_audioReactor.ConditionsAreMet(pass.runtimeData[index]))
						Flash();
			}
		}

		void Flash()
		{
			m_tempColour = m_endingColour;
			m_renderer.material.SetColor("_EmissionColor", m_tempColour * m_emissionMagnitude);
		}
	}
}