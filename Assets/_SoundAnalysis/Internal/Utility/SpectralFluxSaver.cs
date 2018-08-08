using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MusicalGameplayMechanics
{
	[System.Serializable]
	public class SavedPass
	{
		public string name;
		public List<SavedData> savedData;
		public Dictionary<int, SavedData> runtimeData = new Dictionary<int, SavedData>();
	}

	// This class will be serialized into a '.json' save file
	[System.Serializable]
	public class SpectralFluxSaver
	{
		public bool m_loading = false;
		public bool m_saving = false;
		public List<SavedPass> m_savedPasses = new List<SavedPass>();

		/// <summary>
		/// Saves a list of type, 'SpectralFluxInfo', at the location of 'filePath'
		/// </summary>
		/// <param name="samples">
		/// List of SpectralFluxInfo's to save
		/// </param>
		/// <param name="filePath">
		/// File location for the file being saved
		/// </param>
		public void SaveSong(List<SpectralFluxAnalyzer> analyzers, string filePath)
		{
			m_savedPasses.Clear();
			m_saving = true;
			int lastTime = 0;
			for (int i = 0; i < analyzers.Count; i++)
			{
				SavedPass pass = new SavedPass();
				pass.name = analyzers[i].m_pass.name;
				pass.savedData = new List<SavedData>();

				for (int j = 0; j < analyzers[i].m_spectralFluxSamples.Count; j++)
				{
					var sample = analyzers[i].m_spectralFluxSamples[j];
					if (analyzers[i].m_spectralFluxSamples[j].time != lastTime)
					{
						lastTime = sample.time;
						SavedData data = new SavedData
						{
							db = sample.db,
							time = sample.time,
							pitch = sample.pitch,
							peakDensity = sample.peakDensity,
							isPeak = sample.isPeak,
							deviationFromMinDb = sample.deviationFromMinDb
						};
						pass.savedData.Add(data);
					}
					else
					{
						if (pass.savedData.Count > 0 && !pass.savedData[pass.savedData.Count - 1].isPeak && sample.isPeak)
						{
							SavedData data = new SavedData
							{
								db = sample.db,
								time = sample.time,
								pitch = sample.pitch,
								peakDensity = sample.peakDensity,
								isPeak = sample.isPeak,
								deviationFromMinDb = sample.deviationFromMinDb
							};
							pass.savedData.RemoveAt(pass.savedData.Count - 1);
							pass.savedData.Add(data);
						}
					}
				}
				m_savedPasses.Add(pass);
			}
			
			File.Create(filePath).Close();
			string dataAsJson = JsonUtility.ToJson(this, true);
			File.WriteAllText(filePath, dataAsJson);
			// populate the runtime data
			PopulateRuntimeData();
			m_saving = false;
		}

		/// <summary>
		/// Loads a previously saved spectral analysis file
		/// </summary>
		/// <param name="filePath">
		/// File location for the file being loaded
		/// </param>
		public void LoadSong(string filePath)
		{
			m_loading = true;
			if (File.Exists(filePath))
			{
				Debug.Log("Loading SpectralFluxSaver");
				SpectralFluxSaver sfs = JsonUtility.FromJson<SpectralFluxSaver>(File.ReadAllText(filePath));
				Debug.Log("SpectralFluxSaver has been loaded");
				// for each pass detected populate my passes
				m_savedPasses.Clear();
				for (int i = 0; i < sfs.m_savedPasses.Count; i++)
					m_savedPasses.Add(sfs.m_savedPasses[i]);
				Debug.Log("Spectral Flux Samples have been loaded");
			}
			// populate the runtime data
			PopulateRuntimeData();
			m_loading = false;
		}

		/// <summary>
		/// used for populating runtime data. Runtime data is used to save computation when accessing data in the song
		/// </summary>
		public void PopulateRuntimeData()
		{
			for (int i = 0; i < m_savedPasses.Count; i++)
			{
				m_savedPasses[i].runtimeData.Clear();
				for (int j = 0; j < m_savedPasses[i].savedData.Count; j++)
				{
					m_savedPasses[i].runtimeData.Add(m_savedPasses[i].savedData[j].time, m_savedPasses[i].savedData[j]);
				}
			}
		}

		/// <summary>
		/// Find if the save file exists
		/// </summary>
		/// <param name="filePath">
		/// File location to look in
		/// </param>
		/// <returns>
		/// The reult of seeing if the file exists
		/// </returns>
		public bool SongExists(string filePath)
		{
			if (File.Exists(filePath))
			{
				return true;
			}
			return false;
		}
	}
}
