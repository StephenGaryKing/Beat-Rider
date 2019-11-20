using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ShipPreview : MonoBehaviour {

    private void Awake()
    {
        // Makes sure that it has a mesh filter component
        if (!GetComponent<MeshFilter>())
            gameObject.AddComponent<MeshFilter>();

        // Makes sure that it has a mesh renderer component
        if (!GetComponent<MeshRenderer>())
            gameObject.AddComponent<MeshRenderer>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.forward = Vector3.forward;
	}
}
