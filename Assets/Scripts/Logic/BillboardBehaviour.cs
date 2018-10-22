using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardBehaviour : MonoBehaviour {

	Camera m_mainCam;

	// Use this for initialization
	void Start () {
		m_mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.forward = (m_mainCam.transform.position - transform.position);
	}
}
