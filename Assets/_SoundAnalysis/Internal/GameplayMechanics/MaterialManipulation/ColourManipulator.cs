using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicalGameplayMechanics
{
	public class ColourManipulator : GameObjectManipulator
	{
		public Color m_startingColour;
		public Color m_destinationColour;
		Renderer _renderer;

		protected override void Start()
		{
			_renderer = GetComponent<Renderer>();
			base.Start();
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
				_renderer.material.color = Color.Lerp(_renderer.material.color, m_startingColour, m_smoothing);     // Smooth back to the starting colour
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
					_renderer.material.color = m_destinationColour;         // pop the colour to the destination scale

			if (m_reactToMusic)
			{
				float scalarToUse = m_audioReactor.FindValueToReactTo(data);

				float travelPercentage = (scalarToUse - m_minValue) / (m_maxValue - m_minValue);				// Normalize the value between max and min
				Color newColour = Color.Lerp(m_startingColour, m_destinationColour, travelPercentage);
				if (m_audioReactor.ConditionsAreMet(data))
					_renderer.material.color = Color.Lerp(_renderer.material.color, newColour, m_smoothing);	// Lerp towards the new scale
			}
		}
	}
}