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
		const string fileExtention = ".save";
		public List<ListOrganizer> m_numbers = new List<ListOrganizer>();

		public void AddList(List<int> list)
		{
			m_numbers.Add(new ListOrganizer(list));
		}

		public void Save(string filename)
		{
			string filepath = Path.Combine(Application.streamingAssetsPath, filename + fileExtention);
			File.Create(filepath).Close();
			string dataAsJson = JsonUtility.ToJson(this, true);
			File.WriteAllText(filepath, dataAsJson);
		}

		public bool Load(string filename)
		{
			string filepath = Path.Combine(Application.streamingAssetsPath, filename + fileExtention);
			if (File.Exists(filepath))
			{
				m_numbers.Clear();

				string dataFromJson = File.ReadAllText(filepath);
				SaveFile loadedData = JsonUtility.FromJson<SaveFile>(dataFromJson);

				// set the appropriate variables
				foreach(ListOrganizer list in loadedData.m_numbers)
					AddList(list.list);
				return true;
			}
			else
			{
				Debug.LogError("file at " + filepath + " not found");
				return false;
			}
		}

		public void Delete(string filename)
		{
			string filepath = Path.Combine(Application.streamingAssetsPath, filename + fileExtention);
			if (File.Exists(filepath))
				File.Delete(filepath);
		}
	}
}