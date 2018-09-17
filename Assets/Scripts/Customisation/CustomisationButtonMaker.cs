using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomisationButtonMaker : MonoBehaviour {

	public CustomisationButton[] m_customiastionButtons;
	List<GameObject>[] m_buttonPools;

	// Use this for initialization
	void Start () {
		
	}

	public void MakePools()
	{
		m_buttonPools = new List<GameObject>[m_customiastionButtons.Length];
		for (int i = 0; i < m_buttonPools.Length; i ++)
		{
			m_buttonPools[i] = new List<GameObject>();
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
