using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLogic : MonoBehaviour {

	public float m_shakeMagnitude = 1;
	public float m_shakeFrequency = 0.1f;
	public float m_waitTime = 1;
	public float m_relaxTime = 1;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Note"))
			gameObject.SetActive(false);
	}
}
