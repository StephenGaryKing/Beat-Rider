using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeHeight : MonoBehaviour {

	public float minHeight = 0;
	public float maxHeight = 1;

	// Use this for initialization
	void OnEnable () {
		Vector3 pos = transform.localPosition;
		pos.y = Random.Range(minHeight, maxHeight);
		transform.localPosition = pos;
	}
}
