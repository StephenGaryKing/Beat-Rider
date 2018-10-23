using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeColour : MonoBehaviour {

	Renderer m_ren;

	private void Start()
	{
		m_ren = GetComponent<Renderer>();
	}

	void OnEnable () {
		if (m_ren)
			m_ren.material.color = Random.ColorHSV(0f, 1f, 0.5f, 0.5f, 0.5f, 0.5f, 1f, 1f);
	}
}
