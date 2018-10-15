using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MenuMusicManager : MonoBehaviour {

	public AudioClip[] m_songs;

	AudioSource m_source;

	// Use this for initialization
	void Start () {
		m_source = GetComponent<AudioSource>();
	}

	private void OnEnable()
	{
		PlayRandomSong();
	}

	public void PlayRandomSong()
	{
		m_source.Stop();
		int ran = Random.Range(0, m_songs.Length);
		m_source.clip = m_songs[ran];
		m_source.Play();
	}

	public void StopSong()
	{
		m_source.Stop();
	}

	public void PlaySong()
	{
		m_source.Play();
	}
}
