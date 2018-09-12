using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSizeChange : MonoBehaviour {

    Vector3 m_savedScale;
    public float m_scaleAdjust = 2;

    public void Start()
    {
        m_savedScale = transform.localScale;
    }

    public void EnlargeText()
    {
        transform.localScale = m_savedScale * m_scaleAdjust;
    }
    public void ShrinkText()
    {
        transform.localScale = m_savedScale;
    }

}
