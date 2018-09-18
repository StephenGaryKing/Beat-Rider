using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

public class GameController : MonoBehaviour {

    public SongController songcontroller;		// used when pausing the song

		// Pause the game
    [SerializeField] GameObject m_pauseMenu;
    [SerializeField] GameObject m_playButton;
    [SerializeField] GameObject m_pauseButton;
	
	// Update is called once per frame
	void Update () {
        CursorControl();
        if(Input.GetKeyDown(KeyCode.Escape) && songcontroller.m_paused == false)
        {
            songcontroller.Pause();
            if (songcontroller.m_paused)
            {
                Time.timeScale = 0;
                m_pauseMenu.SetActive(true);
                m_pauseButton.SetActive(true);
                m_playButton.SetActive(false);
            }
        }
		// Play the game from a paused state
        else if (Input.GetKeyDown(KeyCode.Escape) && songcontroller.m_paused == true)
        {
            songcontroller.UnPause();
            if (!songcontroller.m_paused)
            {
                ReturnToGame();
            }
        }
    }

	/// <summary>
	/// Un-pause the game
	/// </summary>
	public void ReturnToGame()
    {
        songcontroller.UnPause();
        m_pauseMenu.SetActive(false);
        m_pauseButton.SetActive(false);
        m_playButton.SetActive(true);
        Time.timeScale = 1;
    }

	/// <summary>
	/// Exit the game or stop running if in editor
	/// </summary>
    private void CursorControl()
    {
        if(songcontroller.m_songIsBeingPlayed == true)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }

        if (songcontroller.m_paused == true && Time.timeScale == 0 && songcontroller.m_songIsBeingPlayed == true)
            Cursor.visible = true;

        else if (songcontroller.m_paused == false && Time.timeScale == 1 && songcontroller.m_songIsBeingPlayed == true)
            Cursor.visible = false;
    }
    public void ExitGame()
    {
#if UNITY_STANDALONE
		Application.Quit();
#endif
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
