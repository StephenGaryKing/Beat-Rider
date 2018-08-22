using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

public class GameController : MonoBehaviour {

    public SongController songcontroller;

    [SerializeField] GameObject pauseMenu;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape) && songcontroller.m_paused == false)
        {
            songcontroller.Pause();
            if (songcontroller.m_paused)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && songcontroller.m_paused == true)
        {
            songcontroller.UnPause();
            if (!songcontroller.m_paused)
            {
                ReturnToGame();
            }
        }
    }

    public void ReturnToGame()
    {
        songcontroller.UnPause();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
