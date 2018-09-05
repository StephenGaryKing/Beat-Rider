using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public void PlaySound(Sound sound)
	{
		if (sound.soundToPlay != null)
		{
			AudioSource source = gameObject.AddComponent<AudioSource>();
			source.clip = sound.soundToPlay;
			source.volume = sound.volume;
			Destroy(source, sound.soundToPlay.length);
		}
	}
}
