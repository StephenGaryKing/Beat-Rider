using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeatRider
{
	public class QuickTimeNote : MonoBehaviour {

		int val;
		public int Val { get { return val; } }

		public Text m_text;
		QuickTimeInput m_quickTimeInput;

		private void Awake()
		{
			m_quickTimeInput = FindObjectOfType<QuickTimeInput>();
		}

		private void OnEnable()
		{
			val = Random.Range(0, 4);
			m_text.text = m_quickTimeInput.m_keys[val].ToString();
		}
	}
}