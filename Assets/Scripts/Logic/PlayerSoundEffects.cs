using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sound
{
	public AudioClip soundToPlay;
	[Range(0, 1)]
	public float volume;
}

public class PlayerSoundEffects : MonoBehaviour {

	public Sound m_boost;
	public Sound m_pickupNote;
	public Sound m_hitObstical;
    public Sound m_gemPickup;

	[HideInInspector] public SoundManager m_soundManager;

	// Use this for initialization
	void Start () {
		m_soundManager = FindObjectOfType<SoundManager>();
	}
}
