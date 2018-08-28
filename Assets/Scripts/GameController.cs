using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

public class GameController : MonoBehaviour {


    public SongController songcontroller;		// used when pausing the song
    [SerializeField] GameObject pauseMenu;		// displayed when the game is paused

	void Update () {
		// Pause the game
        if(Input.GetKeyDown(KeyCode.Escape) && songcontroller.m_paused == false)
        {
            songcontroller.Pause();
            if (songcontroller.m_paused)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
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
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

	/// <summary>
	/// Exit the game or stop running if in editor
	/// </summary>
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
