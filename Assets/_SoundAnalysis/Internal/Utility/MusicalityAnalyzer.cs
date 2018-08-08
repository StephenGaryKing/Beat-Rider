using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MusicalGameplayMechanics
{
	/// <summary>
	/// Used for finding tempo, beat density and other musical themed aspects of a song
	/// </summary>
	public class MusicalityAnalyzer
	{
		public float m_commonTempo;					// The most common BPM detected in the song
		public float m_correlatedTempo;				// The BPM with the highest correlation throughout the song
		float m_minimumTimeBetweenPeaks = 0.3f;		// Used for filtering out very fast beats that could result in incorrect bpms
		float m_averageBeatDensity = 0;				// Could be used to estimate the complexity of the song
		float m_averageDb = 0;                      // Could be used to estimate how 'intense' the song is

		/// <summary>
		/// Finds the time between peaks, finds the most commonly 
		/// occcuring time and determines that that is the most likely the tempo.
		/// </summary>
		/// <param name="spectralFluxSamples">
		/// List of spectral flux samples to look through.
		/// </param>
		public void FindTempo(List<SpectralFluxInfo> spectralFluxSamples)
		{
			List<float> listOfTimeIntervals = new List<float>();
			float timeOfLastPeak = 0;
			foreach (SpectralFluxInfo sfi in spectralFluxSamples)
			{
				if (sfi.isPeak)																// If a peak is encountered
				{
					float localTime = (float)Math.Round(sfi.time - timeOfLastPeak, 3);		// Note the time since the last peak (rounded)
					if (localTime >= m_minimumTimeBetweenPeaks)								
					{
						listOfTimeIntervals.Add(localTime);									// Add it to the list
						timeOfLastPeak = sfi.time;
					}
				}
			}

			SortedDictionary<float, int> timesEncounteredWithFrequency = new SortedDictionary<float, int>();
			foreach (float time in listOfTimeIntervals)
			{
				if (timesEncounteredWithFrequency.ContainsKey(time))
					timesEncounteredWithFrequency[time]++;
				else
					timesEncounteredWithFrequency.Add(time, 1);
			}

			KeyValuePair<float, int> commonDist = new KeyValuePair<float, int>(0, 0);
			foreach (KeyValuePair<float, int> time in timesEncounteredWithFrequency)		// Find the most commonly occuring time
			{
				if (time.Value > commonDist.Value)
					commonDist = time;
				//Debug.Log("BPM " + time.Key + " occurred " + time.Value + " times");
			}

			FindHighestCorrelation(spectralFluxSamples, timesEncounteredWithFrequency);		// Attempt a correlation analysis

			m_commonTempo = Mathf.Round(60 / commonDist.Key);								// convert seconds to minutes

			Debug.Log("this song has a common tempo of " + m_commonTempo);
			Debug.Log("NOTE: Correlation is still an unfinnished process. this song has a correlated tempo of " + m_correlatedTempo);
		}

		/// <summary>
		/// Smooth out the pitch to make it easier and more predictable to work with
		/// </summary>
		/// <param name="spectralFluxSamples">
		/// List of spectral flux samples to look through.
		/// </param>
		/// <returns>
		/// The list of smoothed pitches
		/// </returns>
		List<float> RegulatePitch(List<SpectralFluxInfo> spectralFluxSamples)
		{
			List<float> newPitch = new List<float>();
			foreach (var sfi in spectralFluxSamples)
			{
				newPitch.Add(sfi.pitch);				// populate a new list with the pitches
			}
			const float sensitivity = 0.1f;
			for (int i = 0; i < spectralFluxSamples.Count; i++)
			{
				if (i > 1)
					newPitch[i] = Mathf.Lerp(newPitch[i - 1], newPitch[i], sensitivity);	// lerp to the next value
			}
			return newPitch;
		}

		/// <summary>
		/// Roughly find the deviation from the min and max pitch and map that between 0 and 1
		/// </summary>
		/// <param name="spectralFluxSamples"></param>
		public void FindPitchVariance(ref List<SpectralFluxInfo> spectralFluxSamples)
		{
			List<float> regulatedPitch = RegulatePitch(spectralFluxSamples);
			float maxPitch = 800;		// Max pitch is set here because anything higher than this number is not necessary
			float minPitch = 0;			

			for (int i = 0; i < regulatedPitch.Count; i++)
				spectralFluxSamples[i].deviationFromMinPitch = (regulatedPitch[i] - minPitch) / (maxPitch - minPitch);
		}

		/// <summary>
		/// Smooth out the DB to make it easier and more predictable to work with
		/// </summary>
		/// <param name="spectralFluxSamples">
		/// List of spectral flux samples to look through.
		/// </param>
		/// <returns>
		/// Smoothed list of DBs
		/// </returns>
		List<float> RegulateDB(List<SpectralFluxInfo> spectralFluxSamples)
		{
			List <float> newDb = new List<float>();
			foreach (var sfi in spectralFluxSamples)
			{
				newDb.Add(sfi.db);						// Populate a new list with the DBs
			}
			const float sensitivity = 0.02f;
			const int lowestDb = -40;
			for (int i = 0; i < spectralFluxSamples.Count; i ++)
			{
				if (newDb[i] < lowestDb)
					newDb[i] = lowestDb;				// Cap the lowest DB to -40 to stop issues surounding infinity
				if (i > 1)
					newDb[i] = Mathf.Lerp(newDb[i - 1], newDb[i], sensitivity);		// Lerp to the new value
			}
			return newDb;
		}

		/// <summary>
		/// Acurately find the deviation from the min and max DB and map that between 0 and 1
		/// </summary>
		/// <param name="spectralFluxSamples">
		/// Referenced list of spectral flux samples to look through.
		/// </param>
		public void FindVolumeVariance(ref List<SpectralFluxInfo> spectralFluxSamples)
		{
			List<float> regulatedDb = RegulateDB(spectralFluxSamples);
			float averageVolume = 0;
			float maxDb = Mathf.NegativeInfinity;
			float minDb = Mathf.Infinity;
			foreach (float val in regulatedDb)
			{
				averageVolume += val;
				if (val > maxDb)
					maxDb = val;		// Update the max
				if (val < minDb)		// Update the min
					minDb = val;
			}
			averageVolume /= regulatedDb.Count;
			m_averageDb = (averageVolume - minDb) / (maxDb - minDb);    // normalize the average between the max and min db values

			for (int i = 0; i < regulatedDb.Count; i ++)
			{
				spectralFluxSamples[i].deviationFromMinDb = (regulatedDb[i] - minDb) / (maxDb - minDb);
			}
		}

		/// <summary>
		/// Finds the density of the beats throught the song
		/// </summary>
		/// <param name="spectralFluxSamples">
		/// Referenced list of spectral flux samples to look through.
		/// </param>
		public void FindBeatDensity(ref List<SpectralFluxInfo> spectralFluxSamples)
		{
			float averageTimeBetweenBeats = 0;
			float previousTime = 0;
			int numberOfBeats = 0;

			// Find the average beat density
			foreach (SpectralFluxInfo sfi in spectralFluxSamples)
			{
				if (sfi.isPeak)
				{
					averageTimeBetweenBeats += sfi.time - previousTime;
					previousTime = sfi.time;
					numberOfBeats++;
				}
			}
			averageTimeBetweenBeats /= numberOfBeats;
			m_averageBeatDensity = averageTimeBetweenBeats;

			previousTime = 0;
			int indexesToLookBack = 0;
			for (int i = 0; i < spectralFluxSamples.Count; i++)
			{
				if (spectralFluxSamples[i].isPeak)
				{
					spectralFluxSamples[i].peakDensity = averageTimeBetweenBeats / (spectralFluxSamples[i].time - previousTime);
					previousTime = spectralFluxSamples[i].time;

					float unitLength = (spectralFluxSamples[i].peakDensity - spectralFluxSamples[i - indexesToLookBack].peakDensity) / indexesToLookBack;
					for (int j = 1; j < indexesToLookBack; j ++)
					{
						int newIndex = i - j;
						spectralFluxSamples[newIndex].peakDensity = spectralFluxSamples[i].peakDensity - (unitLength * j);
					}
					indexesToLookBack = 0;
				}
				indexesToLookBack++;
			}
		}

		/// <summary>
		/// Holds data based around BPM correlation
		/// </summary>
		public class Correlation
		{
			public int bpm;
			public int hits;
			public int misses;
			public int strength;
		}

		/// <summary>
		/// Attempts to find the BPM with the highest correlation throught the song (does not function correctly)
		/// </summary>
		/// <param name="spectralFluxSamples">
		/// List of spectral flux samples to look through.
		/// </param>
		/// <param name="timesEncounteredWithFrequency">
		/// A sorted dictionary containing the time (key) and the amount of occurances (Value)
		/// </param>
		public void FindHighestCorrelation(List<SpectralFluxInfo> spectralFluxSamples, SortedDictionary<float, int> timesEncounteredWithFrequency)
		{
			List<Correlation> correlations = new List<Correlation>();

			foreach (KeyValuePair<float, int> time in timesEncounteredWithFrequency)
			{
				Correlation newCorrelation = new Correlation();
				newCorrelation.bpm = (int)(60 / time.Key);
				correlations.Add(newCorrelation);

				float startingTime = 0;
				//find where the first peak is and make that the starting time
				foreach(SpectralFluxInfo sfi in spectralFluxSamples)
				{
					if (sfi.isPeak)
					{
						startingTime = sfi.time;
						break;
					}
				}

				int bpm = (int)(60 / time.Key);
				float currentTime = startingTime;
				float timeToMatch;

				foreach (SpectralFluxInfo sfi in spectralFluxSamples)
				{
					if (sfi.isPeak)
					{
						timeToMatch = sfi.time;
						while (currentTime <= timeToMatch - time.Key)
						{
							float timeDifference = timeToMatch - currentTime;
							int BPMOfTimeDiff = (int)(60 / timeDifference);

							if (BPMOfTimeDiff > bpm)
							{
								newCorrelation.misses++;
								currentTime += time.Key;
								continue;
							}

							if (BPMOfTimeDiff == bpm)
							{
								newCorrelation.hits++;
								currentTime += time.Key;
							}

							if (BPMOfTimeDiff < bpm)
							{
								newCorrelation.misses++;
								currentTime += time.Key;
							}
						}
					}
				}

				newCorrelation.strength = newCorrelation.hits - newCorrelation.misses;
				//Debug.Log(String.Format("BPM : {0}, Strength : {1}, Hits : {2}, Misses : {3}", newCorrelation.bpm, newCorrelation.strength, newCorrelation.hits, newCorrelation.misses));
			}

			float strength = Mathf.NegativeInfinity;
			int bestTempo = 0;
			foreach (Correlation cor in correlations)
			{
				if (cor.strength > strength)
				{
					bestTempo = cor.bpm;
					strength = cor.strength;
				}
			}

			m_correlatedTempo = bestTempo;
		}
	}
}