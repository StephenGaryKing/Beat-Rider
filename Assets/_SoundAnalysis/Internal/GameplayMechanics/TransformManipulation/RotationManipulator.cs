using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicalGameplayMechanics
{
	/// <summary>
	/// Manipulate an object's rotation based on music
	/// </summary>
	public class RotationManipulator : GameObjectManipulator
	{

		public Vector3 m_startingRotation;
		public Vector3 m_destinationRotation;
		[SerializeField] bool m_useLocalSpace = true;       // Rotates in local space

		/// <summary>
		/// Change the object's rotation based on the info provided by the AudioReactor
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
					ChangeRotation(pass.runtimeData[index]);
			}
		}

		void Update()
		{
			if (m_reactToBeat)
				transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(m_startingRotation), m_smoothing);      // Smooth back to the starting rotation
		}

		/// <summary>
		/// Change rotation based on the music
		/// </summary>
		/// <param name="sfi">
		/// SpectralFluxInfo to chnage the rotation using
		/// </param>
		void ChangeRotation(SavedData data)
		{
			if (m_reactToBeat)
				if (m_audioReactor.ConditionsAreMet(data))
					transform.localRotation = Quaternion.Euler(m_startingRotation);             // pop the rotation to the destination rotation
			if (m_reactToMusic)
			{
				float scalarToUse = m_audioReactor.FindValueToReactTo(data);

				float travelPercentage = (scalarToUse - m_minValue) / (m_maxValue - m_minValue);                    // Normalize the value between max and min
				Quaternion newRotation = Quaternion.Lerp(Quaternion.Euler(m_startingRotation), Quaternion.Euler(m_destinationRotation), travelPercentage);		// Lerp is used here to provide a more dramatic change at the extremes
				if (m_audioReactor.ConditionsAreMet(data))
				{
					if (m_useLocalSpace)
						transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotation, m_smoothing);      // Slerp towards the new rotation
					else
						transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, m_smoothing);                // Slerp towards the new rotation
				}
			}
		}
	}
}