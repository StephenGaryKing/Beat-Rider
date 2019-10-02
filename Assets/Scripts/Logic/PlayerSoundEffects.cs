using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sound
{
	public AudioClip soundToPlay;
	[HideInInspector] [Range(0, 1)]
	public float volume;
    [Range(0, 1)]
    public float maxVolumeRatio;
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

    public void SetPlayerSoundVolume(float volume)
    {
        m_boost.volume = volume * m_boost.maxVolumeRatio;
        m_pickupNote.volume = volume * m_pickupNote.maxVolumeRatio;
        m_hitObstical.volume = volume * m_hitObstical.maxVolumeRatio;
        m_gemPickup.volume = volume * m_gemPickup.maxVolumeRatio;
    }
}
