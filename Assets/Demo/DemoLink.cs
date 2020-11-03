using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoLink : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame

    public void URLOpenA ()
    {
        Application.OpenURL("");

    }

    public void URLOpenB ()
    {
        Application.OpenURL("https://store.steampowered.com/app/1291720/Neon_Beat_Rider/");

    }

    void Update () {
		
	}
}
