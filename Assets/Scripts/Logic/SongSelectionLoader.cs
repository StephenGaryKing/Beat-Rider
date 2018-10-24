using System.Collections;
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
		if (m_songSelectButton)
		{ 
		m_songButtonTemplate = m_songSelectButton;
		m_container = m_songSelectButton.parent;
		}
		m_defaultFileLocation = Application.dataPath + "/" + m_defaultFileLocation;
		RefreshFiles(m_defaultFileLocation);
	}
	
	public void RefreshFiles(string fileLocation)
	{
		m_songs.Clear();
		DirectoryInfo dir = new DirectoryInfo(fileLocation);
		FileInfo[] info = dir.GetFiles("*.*");
		foreach(FileInfo file in info)
		{
			string name = file.Name;
			string ext = file.Extension;
			if (ext == ".wav" || ext == ".mp3")
			{
				Song newSong = new Song
				{
					fileLocation = file.FullName,
					fileName = name
				};
				m_songs.Add(newSong);
			}
		}
		PopulateButtons();
	}

	void PopulateButtons()
	{
		if (m_container)
		{
			// destroy all children
			int childCount = m_container.childCount - 1;
			for (int i = 0; i < childCount; i++)
				Destroy(m_container.GetChild(i + 1).gameObject);

			for (int i = 0; i < m_songs.Count; i++)
			{
				Transform newButton = Instantiate(m_songButtonTemplate, m_container);
				newButton.name += " " + i;
				newButton.GetComponentInChildren<Text>().text = m_songs[i].fileName;
				newButton.GetComponent<SongSelectButton>().m_filePath = m_songs[i].fileLocation;
				newButton.gameObject.SetActive(true);
			}

			m_songButtonTemplate.gameObject.SetActive(false);
		}
	}

    public IEnumerator SelectSong(string filePath)
    {
        m_songController.OpenSoundFile(filePath);

		yield return new WaitForSeconds(1);

		while (!NAudioPlayer.threadDone)
			yield return null;


        while (m_songController.m_bgThread != null)
        {
            yield return null;
        }
        m_songController.PlayAudio();
    }
}
