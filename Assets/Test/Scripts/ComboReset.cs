using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeatRider;

public class ComboReset : MonoBehaviour {

    PlayerCollision m_playerCol = null;

	// Use this for initialization
	void Start () {
        m_playerCol = FindObjectOfType<PlayerCollision>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Note" && m_playerCol)
        {
            m_playerCol.ResetComboCounter();
        }
    }
}
