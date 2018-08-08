using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryLogic : MonoBehaviour {

	public float m_speed = 1;
    public float m_killAtZ = -20f;
	
	void FixedUpdate () {
		transform.position -= Vector3.forward * m_speed;
		if (transform.position.z < m_killAtZ)
			Destroy(gameObject);
	}
}
