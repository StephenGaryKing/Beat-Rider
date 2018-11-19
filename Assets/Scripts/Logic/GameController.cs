using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

public enum Difficulty
{
	EASY,
	MEDUIM,
	HARD
}

public class GameController : MonoBehaviour {

    public SongController songcontroller;		// used when pausing the song

		// Pause the game
    [SerializeField] GameObject m_pauseMenu;
    [SerializeField] GameObject m_playIcon;
    [SerializeField] GameObject m_pauseIcon;
	bool m_unpauseLocked = false;

	static Difficulty m_difficulty;
	
	[EnumAction(typeof(Difficulty))]
	public void ChangeDifficulty(int diff)
	{
		RealChangeDifficulty((Difficulty)diff);
	}

	public void RealChangeDifficulty(Difficulty diff)
	{
		m_difficulty = diff;
	}

    public static Difficulty GetDifficulty()
    {
        return m_difficulty;
    }

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
                m_pauseIcon.SetActive(true);
                m_playIcon.SetActive(false);
            }
        }
		// Play the game from a paused state
        else if (Input.GetKeyDown(KeyCode.Escape) && songcontroller.m_paused == true)
        {
			if (!m_unpauseLocked)
			{
				songcontroller.UnPause();
				if (!songcontroller.m_paused)
				{
					ReturnToGame();
				}
			}
        }
    }

	public void LockPauseMenu()
	{
		m_unpauseLocked = true;
	}

	public void UnlockPauseMenu()
	{
		m_unpauseLocked = false;
	}

	/// <summary>
	/// Un-pause the game
	/// </summary>
	public void ReturnToGame()
    {
        songcontroller.UnPause();
        m_pauseMenu.SetActive(false);
        m_pauseIcon.SetActive(false);
        m_playIcon.SetActive(true);
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
