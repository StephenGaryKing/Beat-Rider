using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GracesGames.SimpleFileBrowser.Scripts;
using System.IO;

public class SongLocator : MonoBehaviour {

	SongSelectionLoader m_songSelectionLoader;
	public FileBrowser m_fileManager;

	// Use this for initialization
	void Start()
	{
		m_songSelectionLoader = FindObjectOfType<SongSelectionLoader>();
	}

	public void FindSongLocation()
	{
		m_fileManager.SetupFileBrowser(ViewMode.Landscape);
		//string[] fileExtentions = new string[1];
		//fileExtentions[0] = ".wav";
		m_fileManager.OpenFilePanel(null);
		m_fileManager.OnFileSelect += Success;
		m_fileManager.OnFileBrowserClose += Cancel;
	}

	void Success(string fileLocation)
	{
		string dir = Path.GetDirectoryName(fileLocation);
		m_songSelectionLoader.RefreshFiles(dir);
	}

	void Cancel()
	{

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
