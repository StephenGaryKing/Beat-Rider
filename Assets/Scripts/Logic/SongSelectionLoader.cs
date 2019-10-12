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

    // Controller navigation variables
    [SerializeField] private UIButtonManager m_buttonManager = null;
    [SerializeField] private UIButton m_citySelectUI = null;
    private List<UIButton> songUIButtons = new List<UIButton>();

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

        //// Checks for null references, later it will be upgraded to error message
        //if (!m_buttonManager)
        //    Debug.Log("UI Button Manager has not been allocated in Song Selection Loader");
        //if (!m_citySelectUI)
        //    Debug.Log("City Select Button has not been allocated in Song Selection Loader");


        // This part is used to show error dialog messages if there are parts of the script that have not been initialised properly
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            if (!m_buttonManager)
            {
                UnityEditor.EditorUtility.DisplayDialog("Error", "Song Selection Loader script does not have a button manager", "Exit");
                UnityEditor.EditorApplication.isPlaying = false;
            }
            if (!m_citySelectUI)
            {
                UnityEditor.EditorUtility.DisplayDialog("Error", "Song Selection Loader script does not have a city select UI", "Exit");
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }

#endif
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

            // Clears song UI button list
            songUIButtons.Clear();
            m_buttonManager.buttons.Clear();

            for (int i = 0; i < m_songs.Count; i++)
			{
				Transform newButton = Instantiate(m_songButtonTemplate, m_container);

                // Adds a UI button for each song button
                UIButton buttonUI = null;
                buttonUI = newButton.gameObject.AddComponent<UIButton>();

                // Adds a reference to the new button
                songUIButtons.Add(buttonUI);

                // Adds a connection between song UIs to level select UIs
                if (m_citySelectUI)
                {
                    if (i == 0)
                        m_citySelectUI.leftBtn = buttonUI;
                    buttonUI.rightBtn = m_citySelectUI;
                }

                //Adds UI button to UI button manager
                m_buttonManager.buttons.Add(buttonUI);

                newButton.name += " " + i;
				newButton.GetComponentInChildren<Text>().text = m_songs[i].fileName;
				newButton.GetComponent<SongSelectButton>().m_filePath = m_songs[i].fileLocation;
				newButton.gameObject.SetActive(true);
			}

            // Adds navigation to song buttons once they are all instantiated
            for(int i = 0; i < songUIButtons.Count; i++)
            {
                if (i - 1 >= 0)
                    songUIButtons[i].upBtn = songUIButtons[i - 1];
                if (i + 1 < m_songs.Count)
                    songUIButtons[i].downBtn = songUIButtons[i + 1];
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
