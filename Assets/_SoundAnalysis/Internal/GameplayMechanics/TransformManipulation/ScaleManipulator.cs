using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicalGameplayMechanics
{
	/// <summary>
	/// Manipulate an object's scale based on music
	/// </summary>
	public class ScaleManipulator : GameObjectManipulator
	{
		public Vector3 m_startingScale;
		public Vector3 m_destinationScale;

		/// <summary>
		/// Change the object's scale based on the info provided by the AudioReactor
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
					ChangeScale(pass.runtimeData[index]);
			}
		}

		void Update()
		{
			if (m_reactToBeat)
				transform.localScale = Vector3.Lerp(transform.localScale, m_startingScale, m_smoothing);	// Smooth back to the starting scale
		}

		/// <summary>
		/// Change scale based on the music
		/// </summary>
		/// <param name="sfi">
		/// SpectralFluxInfo to chnage the scale using
		/// </param>
		void ChangeScale(SavedData data)
		{
			if (m_reactToBeat)
				if (m_audioReactor.ConditionsAreMet(data))
					transform.localScale = m_destinationScale;		// pop the scale to the destination scale
			if (m_reactToMusic)
			{
				float scalarToUse = m_audioReactor.FindValueToReactTo(data);

				float travelPercentage = (scalarToUse - m_minValue) / (m_maxValue - m_minValue);			// Normalize the value between max and min
				Vector3 newScale = Vector3.Lerp(m_startingScale, m_destinationScale, travelPercentage);		
				if (m_audioReactor.ConditionsAreMet(data))
					transform.localScale = Vector3.Lerp(transform.localScale, newScale, m_smoothing);       // Lerp towards the new scale
			}
		}
	}
}