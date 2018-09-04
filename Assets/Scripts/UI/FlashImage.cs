using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashImage : MonoBehaviour {

    Image m_image;
    public float m_flashSpeed = 1;

    float m_timer = 0;

	// Use this for initialization
	void Start () {
        m_image = GetComponent<Image>();
	}

    private void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= m_flashSpeed)
        {
            ChangeViz();
            m_timer = 0;
        }
    }

    void ChangeViz()
    {
        Color col = Color.white;
        col.a = (m_image.color.a == 1)? 0 : 1;
        m_image.color = col;
    }
}
