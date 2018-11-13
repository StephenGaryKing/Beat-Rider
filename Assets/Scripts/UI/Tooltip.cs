using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

	public Text m_textBox;
	[Multiline]
	public string m_text;

	public void DisplayText()
	{
		m_textBox.gameObject.SetActive(true);
		m_textBox.text = m_text;
	}

	public void HideText()
	{
		m_textBox.gameObject.SetActive(true);
	}

}
