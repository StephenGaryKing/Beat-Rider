using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MusicalGameplayMechanics
{
	/// <summary>
	/// A modifier applied to scripts to filter data recieved by the SongController
	/// </summary>
	[CreateAssetMenu(fileName = "Audio Reactor", menuName = "Musical Gameplay Mechanics/Audio Reactor", order = 1)]
	public class AudioReactor : ScriptableObject
	{

		public bool m_AndPitch = false;
		public bool m_shouldReactToPitch = false;
		public float m_pitch = 0;
		public float m_pitchLeniency = 0;
		public bool m_lessThanPitch = false;
		public bool m_equalToPitch = false;
		public bool m_moreThanPitch = false;

		public bool m_AndDB = false;
		public bool m_shouldReactToDb = false;
		public float m_db = 0;
		public float m_dbLeniency = 0;
		public bool m_lessThanDb = false;
		public bool m_equalToDb = false;
		public bool m_moreThanDb = false;

		public bool m_AndBeatDensity = false;
		public bool m_shouldReactToBeatDensity = false;
		public float m_density = 0;
		public float m_densityLeniency = 0;
		public bool m_lessThanDensity = false;
		public bool m_equalToDensity = false;
		public bool m_moreThanDensity = false;

		public bool m_shouldAlwaysReact = false;

		/// <summary>
		/// Find whether the correct conditions are met for this rector (set via the editor)
		/// </summary>
		/// <param name="sfi">
		/// Spectral info to analyze
		/// </param>
		/// <returns>
		/// Result of the combined comparisons
		/// </returns>
		public bool ConditionsAreMet(SavedData data)
		{
			bool shouldReact = false;		// Is set to false when and is nessesary
			// always react
			if (m_shouldAlwaysReact)
				return true;
			// pitch
			if (m_shouldReactToPitch)
			{
				if (m_AndPitch)
					shouldReact = false;
				if (m_lessThanPitch)
					if (data.pitch < (m_pitch + m_pitchLeniency))
						shouldReact = true;
				if (m_equalToPitch)
					if (data.pitch > (m_pitch - m_pitchLeniency) && data.pitch < (m_pitch + m_pitchLeniency))
						shouldReact = true;
				if (m_moreThanPitch)
					if (data.pitch > (m_pitch - m_pitchLeniency))
						shouldReact = true;
			}
			// db
			if (m_shouldReactToDb)
			{
				if (m_AndDB)
					shouldReact = false;
				if (m_lessThanDb)
					if (data.db < (m_db + m_dbLeniency))
						shouldReact = true;
				if (m_equalToDb)
					if (data.db > (m_db - m_dbLeniency) && data.db < (m_db + m_dbLeniency))
						shouldReact = true;
				if (m_moreThanDb)
					if (data.db > (m_db - m_dbLeniency))
						shouldReact = true;
			}
			// density
			if (m_shouldReactToBeatDensity)
			{
				if (m_AndBeatDensity)
					shouldReact = false;
				if (m_lessThanDensity)
					if (data.peakDensity < (m_density + m_densityLeniency))
						shouldReact = true;
				if (m_equalToDensity)
					if (data.peakDensity > (m_density - m_densityLeniency) && data.peakDensity < (m_density + m_densityLeniency))
						shouldReact = true;
				if (m_moreThanDensity)
					if (data.peakDensity > (m_density - m_densityLeniency))
						shouldReact = true;
			}

			if (shouldReact)
				return true;
			return false;
		}

		/// <summary>
		/// Finds the value to react to (used in the editor for setting values)
		/// </summary>
		/// <param name="sfi">
		/// Spectral info to analyze
		/// </param>
		/// <returns>
		/// value to react to (may give false results if multiple conditions are used at once)
		/// </returns>
		public float FindValueToReactTo(SavedData data)
		{
			float val = 0;
			if (m_shouldReactToPitch)
				val += data.pitch;
			if (m_shouldReactToBeatDensity)
				val += data.peakDensity;
			if (m_shouldReactToDb)
				val += data.db;
			return val;
		}
	}
}