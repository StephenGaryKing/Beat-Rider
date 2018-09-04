using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

public class SongSelectButton : MonoBehaviour {

	[HideInInspector] public string m_filePath;
    public SongSelectionLoader m_selectionLoader;

	public void Select()
	{
		m_selectionLoader.StartCoroutine(m_selectionLoader.SelectSong(m_filePath));
	}

}
