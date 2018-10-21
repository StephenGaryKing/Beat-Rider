using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MenuMusicManager : MonoBehaviour {

	public AudioClip[] m_songs;

	int m_lastSongPlayed = -1;

	AudioSource m_source;

	// Use this for initialization
	void Start () {
        if (m_source == null)
            m_source = GetComponent<AudioSource>();
        PlayRandomSong();
    }

	private void OnEnable()
	{
        if (m_source == null)
            m_source = GetComponent<AudioSource>();
        PlayRandomSong();
	}

	public void PlayRandomSong()
	{
		int ran = Random.Range(0, m_songs.Length);
		if (m_songs.Length > 1)
			while (ran == m_lastSongPlayed)
				ran = Random.Range(0, m_songs.Length);

		m_source.clip = m_songs[ran];
		m_source.Play();
	}

	public void StopSong()
	{
        if (m_source.isPlaying)
		    m_source.Stop();
	}

	public void PlaySong()
	{
		m_source.Play();
	}
}
