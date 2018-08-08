using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicalGameplayMechanics
{
	public class EmissionManipulator : GameObjectManipulator
	{
		public float m_startingBrightness;
		public float m_destinationBrightness;
		public Color m_startingColour;
		public Color m_destinationColour;
		Renderer _renderer;

		Color m_tempColour;
		float m_tempBrightness;

		protected override void Start()
		{
			_renderer = GetComponent<Renderer>();
			base.Start();
			m_tempColour = Color.white;
			m_tempBrightness = 0;
		}

		/// <summary>
		/// Change the object's colour based on the info provided by the AudioReactor
		/// </summary>
		/// <param name="sfi">
		/// List of SpectralFluxInfo's
		/// </param>
		/// <param name="index">
		/// Index of SpectralFluxInfo's to use
		/// </param>
		public override void ManipulateObject(List<SavedPass> passes, int index)
		{
			foreach (SavedPass pass in passes)
			{
				if (m_passToReactTo != "DEFAULT" && pass.name != m_passToReactTo)
					continue;
				if (m_reactToBeat && !pass.runtimeData[index].isPeak)
					continue;
				m_currentValue = m_audioReactor.FindValueToReactTo(pass.runtimeData[index]);
				if (m_audioReactor.ConditionsAreMet(pass.runtimeData[index]))
					ChangeColour(pass.runtimeData[index]);
			}
		}

		void Update()
		{
			if (m_reactToBeat)
			{
				m_tempColour = Color.Lerp(m_tempColour, m_startingColour, m_smoothing);
				m_tempBrightness = Mathf.Lerp(m_tempBrightness, m_startingBrightness, m_smoothing);

				Color newColour = m_tempColour * m_tempBrightness;
				_renderer.material.SetColor("_EmissionColor", newColour);
			}
		}

		/// <summary>
		/// Change colour based on the music
		/// </summary>
		/// <param name="sfi">
		/// SpectralFluxInfo to chnage the scale using
		/// </param>
		void ChangeColour(SavedData data)
		{
			if (m_reactToBeat)
				if (m_audioReactor.ConditionsAreMet(data))
				{
					Color newColour = m_destinationColour * Mathf.LinearToGammaSpace(m_destinationBrightness);
					_renderer.material.SetColor("_EmissionColor", newColour);

					m_tempColour = m_destinationColour;
					m_tempBrightness = m_destinationBrightness;
					Debug.Log("flash " + m_tempBrightness);
				}

			if (m_reactToMusic)
			{
				float scalarToUse = m_audioReactor.FindValueToReactTo(data);

				float travelPercentage = (scalarToUse - m_minValue) / (m_maxValue - m_minValue);                // Normalize the value between max and min
				Color newColour = Color.Lerp(m_startingColour, m_destinationColour, travelPercentage);
				float newBrightness = Mathf.Lerp(m_tempBrightness, m_destinationBrightness, travelPercentage);

				newColour = newColour * Mathf.LinearToGammaSpace(newBrightness);

				if (m_audioReactor.ConditionsAreMet(data))
					_renderer.material.SetColor("_EmissionColor", Color.Lerp(m_tempColour, newColour, m_smoothing));

				m_tempColour = newColour;
				m_tempBrightness = newBrightness;

			}
		}
	}
}