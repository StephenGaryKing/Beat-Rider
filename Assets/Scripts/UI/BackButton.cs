using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour {

    private Button m_backButton = null;

	// Use this for initialization
	void Start () {
        m_backButton = gameObject.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_backButton && Input.GetButtonDown("Cancel"))
            m_backButton.onClick.Invoke();
	}
}
