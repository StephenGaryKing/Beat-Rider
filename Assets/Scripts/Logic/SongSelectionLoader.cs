﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using MusicalGameplayMechanics;

public class SongSelectionLoader : MonoBehaviour {
	[System.Serializable]
	public struct Song
	{
		public string fileName;
		public string fileLocation;
	};

	public string m_defaultFileLocation;
	public List<Song> m_songs;
	public Transform m_songSelectButton;
    SongController m_songController;

	Transform m_container;
	Transform m_songButtonTemplate;

	// Use this for initialization
	void Start () {
        m_songController = FindObjectOfType<SongController>();
		m_songButtonTemplate = m_songSelectButton;
		m_container = m_songSelectButton.parent;
		m_defaultFileLocation = Application.dataPath + "/" + m_defaultFileLocation;
		RefreshFiles(m_defaultFileLocation);
	}
	
	public void RefreshFiles(string fileLocation)
	{
		DirectoryInfo dir = new DirectoryInfo(m_defaultFileLocation);
		FileInfo[] info = dir.GetFiles("*.*");
		foreach(FileInfo file in info)
		{
			string name = file.Name;
			string sub = name.Substring(name.Length - 4);
			if (sub == ".wav")
			{
				Song newSong = new Song();
				newSong.fileLocation = file.FullName;
				newSong.fileName = name;
				m_songs.Add(newSong);
			}
		}
		PopulateButtons();
	}

	void PopulateButtons()
	{
		// destroy all children
		int childCount = m_container.childCount;
		for (int i = 1; i < childCount; i ++)
			Destroy(m_container.GetChild(0).gameObject);
		for(int i = 0; i < m_songs.Count; i ++)
		{
			Transform newButton = Instantiate(m_songButtonTemplate, m_container);
			newButton.GetComponentInChildren<Text>().text = m_songs[i].fileName;
			newButton.GetComponent<SongSelectButton>().m_filePath = m_songs[i].fileLocation;
		}
		Destroy(m_songButtonTemplate.gameObject);
		m_songButtonTemplate = m_container.GetChild(0);
	}

    public IEnumerator SelectSong(string filePath)
    {
        m_songController.OpenSoundFile(filePath);
        yield return new WaitForSeconds(1);
        while (m_songController.m_bgThread != null)
        {
            yield return null;
        }
        m_songController.PlayAudio();
    }
}