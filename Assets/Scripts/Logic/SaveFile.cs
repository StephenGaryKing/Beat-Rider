using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BeatRider
{

	[System.Serializable]
	public struct ListOrganizer
	{
		public List<int> list;
		public ListOrganizer(List<int> newlist)
		{
			list = newlist;
		}

	}

	// TO DO: Add Hashing and salting

	[System.Serializable]
	public class SaveFile
	{
		public List<ListOrganizer> m_numbers = new List<ListOrganizer>();

		public void AddList(List<int> list)
		{
			m_numbers.Add(new ListOrganizer(list));
		}

		public void Save(string filename)
		{
			string filepath = Path.Combine(Application.streamingAssetsPath, filename + ".json");
			File.Create(filepath).Close();
			string dataAsJson = JsonUtility.ToJson(this, true);
			File.WriteAllText(filepath, dataAsJson);
		}

		public void Load(string filename)
		{
			string filepath = Path.Combine(Application.streamingAssetsPath, filename + ".json");
			if (File.Exists(filepath))
			{
				m_numbers.Clear();

				string dataFromJson = File.ReadAllText(filepath);
				SaveFile loadedData = JsonUtility.FromJson<SaveFile>(dataFromJson);

				// set the appropriate variables
				foreach(ListOrganizer list in loadedData.m_numbers)
					AddList(list.list);
			}
			else
			{
				Debug.LogError("file at " + filepath + " not found");
			}
		}						
	}
}