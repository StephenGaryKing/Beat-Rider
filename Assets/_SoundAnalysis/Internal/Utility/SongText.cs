using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MusicalGameplayMechanics
{
	public class SongText : MonoBehaviour
	{

		public Text m_currentSong = null;
		public AudioSource m_audioSource = null;

		public void Update()
		{
			if (m_audioSource.clip != null)
				m_currentSong.text = m_audioSource.clip.name;
		}
	}
}