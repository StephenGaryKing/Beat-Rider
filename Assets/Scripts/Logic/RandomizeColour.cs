using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeColour : MonoBehaviour {

	Renderer m_ren;
	Material m_myInst;

	private void Start()
	{
		m_ren = GetComponentInChildren<Renderer>();
		m_myInst = new Material (m_ren.material);
		m_ren.material = m_myInst;
	}

	void OnEnable () {
		Color col = new Color (Random.Range (0f, 100f) / 100f, Random.Range (0f, 100f) / 100f, Random.Range (0f, 100f) / 100f, 1f);
		if (m_ren)
			m_ren.material.color = col;
	}
}
