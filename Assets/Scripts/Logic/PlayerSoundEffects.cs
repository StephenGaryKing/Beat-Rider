using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sound
{
	public AudioClip soundToPlay;
	public float volume;
}

public class PlayerSoundEffects : MonoBehaviour {

	public Sound m_boost;
	public Sound m_pickupNote;
	public Sound m_hitObstical;

	[HideInInspector] public SoundManager m_soundManager;

	// Use this for initialization
	void Start () {
		m_soundManager = FindObjectOfType<SoundManager>();
	}
}
