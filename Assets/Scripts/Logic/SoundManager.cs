using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

	public float m_audioClipVolume = 0.5f;

	public AudioSource PlaySound(Sound sound)
	{
		if (sound.soundToPlay != null)
		{
			AudioSource source = gameObject.AddComponent<AudioSource>();
			source.clip = sound.soundToPlay;
			source.volume = sound.volume;
			source.Play();
			Destroy(source, sound.soundToPlay.length);
			return source;
		}
		return null;
	}

	public void PlaySound(AudioClip sound)
	{
		if (sound != null)
		{
			AudioSource source = gameObject.AddComponent<AudioSource>();
			source.clip = sound;
			source.volume = m_audioClipVolume;
			source.Play();
			Destroy(source, sound.length);
		}
	}

	public void UpdateVolume(Slider slider)
	{
		m_audioClipVolume = slider.value;
	}
}
