using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicalGameplayMechanics
{
	/// <summary>
	/// Manipulate an object's position based on music
	/// </summary>
	public class PositionManipulator : GameObjectManipulator
	{
		public Vector3 m_startingPosition = Vector3.zero;
		public Vector3 m_destinationPosition = Vector3.zero;
		[SerializeField] bool m_useLocalSpace = true;           // Moves in local space

		/// <summary>
		/// Change the object's position based on the info provided by the AudioReactor
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
					ChangePosition(pass.runtimeData[index]);
			}
		}

		void Update()
		{
			if (m_reactToBeat)
				transform.localPosition = Vector3.Lerp(transform.localPosition, m_startingPosition, m_smoothing);       // Smooth back to the starting position
		}

		/// <summary>
		/// Change position based on the music
		/// </summary>
		/// <param name="sfi">
		/// SpectralFluxInfo to chnage the position using
		/// </param>
		void ChangePosition(SavedData data)
		{
			if (m_reactToBeat)
				if (m_audioReactor.ConditionsAreMet(data))
					transform.localPosition = m_destinationPosition;			// pop the position to the destination position
			if (m_reactToMusic)
			{
				float scalarToUse = m_audioReactor.FindValueToReactTo(data);

				float travelPercentage = (scalarToUse - m_minValue) / (m_maxValue - m_minValue);                        // Normalize the value between max and min
				Vector3 newPostion = Vector3.Lerp(m_startingPosition, m_destinationPosition, travelPercentage);
				if (m_audioReactor.ConditionsAreMet(data))
				{
					if (m_useLocalSpace)
						transform.localPosition = Vector3.Lerp(transform.localPosition, newPostion, m_smoothing);        // Lerp towards the new position
					else
						transform.position = Vector3.Lerp(transform.position, newPostion, m_smoothing);                  // Lerp towards the new position
				}
			}
		}
	}
}