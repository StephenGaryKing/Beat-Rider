using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextSceneAfterTime : MonoBehaviour {

    public float m_waitTime = 5f;
    float m_timer = 0;
	bool m_going = false;
	
	// Update is called once per frame
	void Update () {
        m_timer += Time.deltaTime;
		if (!m_going && m_timer >= m_waitTime) 
		{
			m_going = true;
			SceneManager.LoadSceneAsync (1);
		}
	}
}
