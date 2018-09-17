using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryShipMover : MonoBehaviour {

	public float m_speed;
	Vector3 m_localStartLocation;

	private void Awake()
	{
		m_localStartLocation = transform.localPosition;
	}

	private void OnEnable()
	{
		transform.localPosition = m_localStartLocation;
	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.localPosition += transform.forward * m_speed;
	}
}
