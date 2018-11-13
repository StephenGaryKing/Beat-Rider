using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DotDotDotter : MonoBehaviour {

	Text m_stringToDot;
	string m_origionalString;
	public float m_timeBetweenDots;
	public string m_stringToAdd = "...";

	private void Start()
	{
		m_stringToDot = GetComponent<Text>();
		m_origionalString = m_stringToDot.text;
		StartCoroutine(DotDotDot());
	}

	private void OnEnable()
	{
		StopAllCoroutines();
		StartCoroutine(DotDotDot());
	}

	IEnumerator DotDotDot()
	{
		while (true)
		{
			for (int i = 0; i <= m_stringToAdd.Length; i ++)
			{
                if(m_stringToDot)
				    m_stringToDot.text = m_origionalString + m_stringToAdd.Substring(0, i);
				yield return new WaitForSeconds(m_timeBetweenDots);
			}
			yield return null;
		}
	}
}
