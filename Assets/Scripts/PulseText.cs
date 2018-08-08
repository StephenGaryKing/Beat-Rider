using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseText : MonoBehaviour {

    Text text;

    public float speed = 10;
    public float minBlue = 0.5f;
    public float maxBlue = 1;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {
        Color col = text.color;

        col.b = Mathf.Lerp(minBlue, maxBlue, 0.5f *(1+Mathf.Sin(speed * Time.time)));

        text.color = col;
	}
}
