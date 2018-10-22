using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

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
			source.Play();
			Destroy(source, sound.length);
		}
	}
}
