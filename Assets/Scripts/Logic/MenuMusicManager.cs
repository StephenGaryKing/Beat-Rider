using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MenuMusicManager : MonoBehaviour {

	public AudioClip[] m_songs;

	bool m_songShouldBePlaying = false;
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

	private void Update()
	{
		if (m_songShouldBePlaying && !m_source.isPlaying)
			PlayRandomSong();
	}

	public void PlayRandomSong()
	{
		m_songShouldBePlaying = true;
		int ran = Random.Range(0, m_songs.Length);
		if (m_songs.Length > 1)
			while (ran == m_lastSongPlayed)
				ran = Random.Range(0, m_songs.Length);

		m_source.clip = m_songs[ran];
		m_source.Play();

	}

	public void StopSong()
	{
		m_songShouldBePlaying = false;
        if (m_source.isPlaying)
		    m_source.Stop();
	}

	public void PauseSong()
	{
		m_songShouldBePlaying = false;
		if (m_source.isPlaying)
			m_source.Pause();
	}

	public void PlaySong()
	{
		m_songShouldBePlaying = true;
		m_source.Play();
	}
}
