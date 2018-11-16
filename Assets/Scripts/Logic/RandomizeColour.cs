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
		if (m_ren)
			m_ren.material.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1f);
			//m_ren.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 0.5f, 1f, 1f);
	}
}
