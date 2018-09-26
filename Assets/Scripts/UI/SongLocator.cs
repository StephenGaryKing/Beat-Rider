using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using System.IO;

public class SongLocator : MonoBehaviour {

	SongSelectionLoader m_songSelectionLoader;

	// Use this for initialization
	void Start()
	{
		m_songSelectionLoader = FindObjectOfType<SongSelectionLoader>();
	}

	public void FindSongLocation()
	{
		FileBrowser.ShowLoadDialog(Success, Cancel);
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
