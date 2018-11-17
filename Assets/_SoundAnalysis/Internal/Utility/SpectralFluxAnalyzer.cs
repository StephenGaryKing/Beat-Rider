using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicalGameplayMechanics
{
	/// <summary>
	/// Data set to store information about a song at a point in time
	/// </summary>
	public class SpectralFluxInfo
	{
		public float[] spectrum;
		public int time;
		public float rms;
		public float peakDensity;
		public float db;
		public float deviationFromMinDb;
		public float pitch;
		public float deviationFromMinPitch;
		public float spectralFlux;
		public float threshold;
		public float prunedSpectralFlux;
		public bool isPeak;
	}

	/// <summary>
	/// Data set to store information about a song at a point in time
	/// </summary>
	[Serializable]
	public class SavedData
	{
		public int time;
		public float peakDensity;
		public float deviationFromMinDb;
		public float db;
		public float pitch;
		public bool isPeak;
	}

	/// <summary>
	/// Used to analyze the spectral flux throughout a song as well as define beats throughout a song.
	/// </summary>
	public class SpectralFluxAnalyzer
	{
		// Sensitivity multiplier to scale the average threshold.
		// In this case, if a rectified spectral flux sample is > m_thresholdMultiplier times the average, it is a peak
		public float m_thresholdMultiplier = 3f;
		public float m_sampleRate;

		// Number of samples to average in our window
		int m_thresholdWindowSize = 200;

		public List<SpectralFluxInfo> m_spectralFluxSamples;
		public Pass m_pass;

		float[] m_curSpectrum;
		float[] m_prevSpectrum;

		int m_indexToProcess;

		/// <summary>
		/// Value initialization
		/// </summary>
		public SpectralFluxAnalyzer()
		{
			m_spectralFluxSamples = new List<SpectralFluxInfo>();

			// Start processing from middle of first window and increment by 1 from there
			m_indexToProcess = m_thresholdWindowSize / 2;
		}

		public void SetCurSpectrum(float[] spectrum)
		{
			int specLength = m_pass.maxFreq - m_pass.minFreq;
			if (m_curSpectrum == null)
				m_curSpectrum = new float[specLength];
			if (m_prevSpectrum == null)
				m_prevSpectrum = new float[specLength];

			float[] newSpectrum = new float[specLength];
			for (int i = 0; i < specLength; i ++)
				newSpectrum[i] = spectrum[i + m_pass.minFreq];

			m_curSpectrum.CopyTo(m_prevSpectrum, 0);
			newSpectrum.CopyTo(m_curSpectrum, 0);
		}

		/// <summary>
		/// Gets the Root Mean Square of a spectrum of sound (square root of the mean value of the channels)
		/// </summary>
		/// <param name="spectrum">
		/// The spectrum to analyze
		/// </param>
		/// <returns>
		/// RMS
		/// </returns>
		float GetRMS(float[] spectrum)
		{
			//Get the RMS
			float sum = 0;
			for (int i = 0; i < spectrum.Length; i++)
			{
				sum += spectrum[i] * spectrum[i];
			}
			return Mathf.Sqrt(sum / spectrum.Length);
		}

		/// <summary>
		/// Gets the decibel value using a given rms value (db is the difference in level in terms of a logarithmic scale)
		/// </summary>
		/// <param name="rms">
		/// Root mean square
		/// </param>
		/// <returns>
		/// decibel value
		/// </returns>
		float GetDB(float rms)
		{
			//Find the db
			return 20 * Mathf.Log10(rms / 0.1f);
		}

		/// <summary>
		/// Gets the Pitch of a spectrum of sound
		/// </summary>
		/// <param name="spectrum">
		/// The spectrum to analyze
		/// </param>
		/// <returns>
		/// Pitch
		/// </returns>
		float GetPitch(float[] spectrum)
		{
			//Find Pitch
			float maxV = 0;
			int maxN = 0;
			for (int i = 0; i < spectrum.Length; i++)
			{
				if (!(spectrum[i] > maxV) || !(spectrum[i] > 0.0f))
					continue;
				maxV = spectrum[i];
				maxN = i;
			}

			float freqN = maxN;
			if (maxN > 0 && maxN < spectrum.Length - 1)
			{
				float dL = spectrum[maxN - 1] / spectrum[maxN];
				float dR = spectrum[maxN + 1] / spectrum[maxN];
				freqN += 0.5f * (dR * dR - dL - dL * dL);
			}
			float pitch = freqN * (m_sampleRate / 2) / spectrum.Length;
			return pitch;
		}

		/// <summary>
		/// Analyze the various aspects of a spectrum at a point in time
		/// </summary>
		/// <param name="spectrum">
		/// The spectrum to analyze
		/// </param>
		/// <param name="time">
		/// The time that this spectrum was observed in the song
		/// </param>
		public void AnalyzeSpectrum(float[] spectrum, float time)
		{
			// Set spectrum using the pass min and max provided
			SetCurSpectrum(spectrum);

			SpectralFluxInfo curInfo = new SpectralFluxInfo();
			curInfo.spectrum = spectrum;									// Set the spectrum
			curInfo.time = Mathf.RoundToInt(time * 10f);					// Set the time
			curInfo.rms = GetRMS(spectrum);									// Find the root mean square and set it
			curInfo.db = GetDB(curInfo.rms);								// Find the DB and set it
			curInfo.pitch = GetPitch(spectrum);								// Find the Pitch and set it
			curInfo.spectralFlux = CalculateRectifiedSpectralFlux();		// Get current spectral flux from spectrum
			m_spectralFluxSamples.Add(curInfo);

			// We have enough samples to detect a peak
			if (m_spectralFluxSamples.Count >= m_thresholdWindowSize)
			{
				m_spectralFluxSamples[m_indexToProcess].threshold = GetFluxThreshold(m_indexToProcess);                 // Get Flux threshold of time window surrounding index to process
				m_spectralFluxSamples[m_indexToProcess].prunedSpectralFlux = GetPrunedSpectralFlux(m_indexToProcess);   // Only keep amp amount above threshold to allow peak filtering

				// Now that we are processed at n, n-1 has neighbors (n-2, n) to determine peak
				int indexToDetectPeak = m_indexToProcess - 1;
				bool curPeak = IsPeak(indexToDetectPeak);
				if (curPeak)
					m_spectralFluxSamples[indexToDetectPeak].isPeak = true;

				m_indexToProcess++;
			}
			else
			{
				//Debug.Log(string.Format("Not ready yet.  At spectral flux sample size of {0} growing to {1}", m_spectralFluxSamples.Count, m_thresholdWindowSize));
			}
		}

		/// <summary>
		/// Finds the aggregated positive changes in the spectrum data from the previous spectrum analyzed and the current spectrum being analyzed
		/// </summary>
		/// <returns>
		/// Amount of aggregated positive change
		/// </returns>
		float CalculateRectifiedSpectralFlux()
		{
			float sum = 0f;
			int numSamples = m_curSpectrum.Length;

			// Aggregate positive changes in spectrum data
			for (int i = 0; i < numSamples; i++)
			{
				sum += Mathf.Max(0f, m_curSpectrum[i] - m_prevSpectrum[i]);
			}
			return sum;
		}

		/// <summary>
		/// Finds the threshold at which the spectral flux must reach to be classified as a peak
		/// </summary>
		/// <param name="spectralFluxIndex">
		/// The index of the spectral flux samples that is being modified
		/// </param>
		/// <returns>
		/// Threshold
		/// </returns>
		float GetFluxThreshold(int spectralFluxIndex)
		{
			// How many samples in the past and future we include in our average
			int windowStartIndex = Mathf.Max(0, spectralFluxIndex - m_thresholdWindowSize / 2);
			int windowEndIndex = Mathf.Min(m_spectralFluxSamples.Count - 1, spectralFluxIndex + m_thresholdWindowSize / 2);

			// Add up our spectral flux over the window
			float sum = 0f;
			for (int i = windowStartIndex; i < windowEndIndex; i++)
			{
				sum += m_spectralFluxSamples[i].spectralFlux;
			}

			// Return the average multiplied by our sensitivity multiplier
			float avg = sum / (windowEndIndex - windowStartIndex);
			return avg * m_thresholdMultiplier;
		}

		/// <summary>
		/// Prune the spectral flux to find whether the it is great enough to be classified as a peak
		/// </summary>
		/// <param name="spectralFluxIndex">
		/// The index of the spectral flux samples that is being modified/analyzed
		/// </param>
		/// <returns>
		/// the degree of the peak (0 : no peak) (more than 0 : peak)
		/// </returns>
		float GetPrunedSpectralFlux(int spectralFluxIndex)
		{
			// If the spectral flux is not more than the threshold at this point in time, cull it
			return Mathf.Max(0f, m_spectralFluxSamples[spectralFluxIndex].spectralFlux - m_spectralFluxSamples[spectralFluxIndex].threshold);	
		}

		/// <summary>
		/// Find if a new peak has occured
		/// </summary>
		/// <param name="spectralFluxIndex">
		/// The index of the spectral flux samples that is being modified/analyzed
		/// </param>
		/// <returns>
		/// </returns>
		bool IsPeak(int spectralFluxIndex)
		{
			if (m_spectralFluxSamples[spectralFluxIndex].prunedSpectralFlux > m_spectralFluxSamples[spectralFluxIndex + 1].prunedSpectralFlux &&
				m_spectralFluxSamples[spectralFluxIndex].prunedSpectralFlux > m_spectralFluxSamples[spectralFluxIndex - 1].prunedSpectralFlux)
			{
				return true;	// This index contains a peak in spectral flux when compared to its neighbours
			}
			else
			{
				return false;	// This index does not contain a peak in spectral flux when compared to its neighbours
			}
		}

		/// <summary>
		/// log used for finding issues in a specific index of the sample
		/// </summary>
		/// <param name="indexToLog">
		/// The index of the spectral flux samples that is being logged
		/// </param>
		void LogSample(int indexToLog)
		{
			int windowStart = Mathf.Max(0, indexToLog - m_thresholdWindowSize / 2);
			int windowEnd = Mathf.Min(m_spectralFluxSamples.Count - 1, indexToLog + m_thresholdWindowSize / 2);
			Debug.Log(string.Format(
				"Peak detected at song time {0} with pruned flux of {1} ({2} over thresh of {3}).\n" +
				"Thresh calculated on time window of {4}-{5} ({6} seconds) containing {7} samples.",
				m_spectralFluxSamples[indexToLog].time,
				m_spectralFluxSamples[indexToLog].prunedSpectralFlux,
				m_spectralFluxSamples[indexToLog].spectralFlux,
				m_spectralFluxSamples[indexToLog].threshold,
				m_spectralFluxSamples[windowStart].time,
				m_spectralFluxSamples[windowEnd].time,
				m_spectralFluxSamples[windowEnd].time - m_spectralFluxSamples[windowStart].time,
				windowEnd - windowStart
			));
		}
	}
}