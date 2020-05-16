using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CameraFade : MonoBehaviour
{

    [SerializeField] private List<Image> m_blackPanels = new List<Image>();
    [SerializeField] private Image m_blackImage = null;
    private bool m_isFadeOut = false;
    private bool m_startFade = false;

    [SerializeField] private float m_fadeTimer = 10f;
    [SerializeField] private float m_fadeWaitDuration = 0.5f;

    public UnityEvent afterFadeIn;
    public UnityEvent afterFadeOut;

    //float currentRatio = 0;


    private float m_fadeTime = 0;
    private float m_fadeWaitTime = 0;
    private Color m_targetColour = new Color(0, 0, 0, 0);
    private Color m_currentColour = new Color(0, 0, 0, 0);
    private float m_lerpTime = 0;

    // Use this for initialization
    void Start()
    {
        //FadeGame(m_fadeTimer, false, m_fadeWaitDuration);
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (m_startFade)
    //    {
    //        Debug.Log("fade called");
    //        if (m_fadeWaitTime > 0)
    //        {
    //            m_fadeWaitTime -= Time.fixedDeltaTime;
    //            return;
    //        }
    //        m_lerpTime -= Time.fixedDeltaTime / m_fadeTimer;
    //        m_blackImage.color = Color.Lerp(m_targetColour, m_currentColour, m_lerpTime);
    //        if (m_lerpTime <= 0)
    //        {
    //            //m_startFade = false;
    //            if (m_isFadeOut)
    //            {
    //                afterFadeOut.Invoke();
    //                m_targetColour = new Color(0, 0, 0, 0);
    //                m_currentColour = m_blackImage.color;
    //                m_fadeWaitTime = m_fadeWaitDuration;
    //                m_lerpTime = 1;
    //                m_isFadeOut = false;
    //            }
    //            else
    //            {
    //                afterFadeIn.Invoke();
    //                m_startFade = false;
    //                gameObject.SetActive(false);
    //            }
    //        }
    //    }
    //}

    private void Update()
    {
        if (m_startFade)
        {
            Debug.Log("fade called");
            if (m_fadeWaitTime > 0)
            {
                m_fadeWaitTime -= Time.deltaTime;
                return;
            }
            m_lerpTime -= Time.deltaTime / m_fadeTimer;
            m_blackImage.color = Color.Lerp(m_targetColour, m_currentColour, m_lerpTime);
            if (m_lerpTime <= 0)
            {
                //m_startFade = false;
                if (m_isFadeOut)
                {
                    afterFadeOut.Invoke();
                    m_targetColour = new Color(0, 0, 0, 0);
                    m_currentColour = m_blackImage.color;
                    m_fadeWaitTime = m_fadeWaitDuration;
                    m_lerpTime = 1;
                    m_isFadeOut = false;
                }
                else
                {
                    afterFadeIn.Invoke();
                    m_startFade = false;
                    //gameObject.SetActive(false);
                }
            }
        }
    }

    public void FadeMenu(/*bool isFadeOut = false, float fadeTimer = 10f, float fadeWaitTime = 0f*/)
    {
        Debug.Log("Fade method called");
        m_startFade = true;
        m_isFadeOut = true;
        //m_fadeTime = m_fadeTimer;
        //m_fadeTimer = fadeTimer;
        m_fadeWaitTime = m_fadeWaitDuration;
        m_currentColour = new Color(0, 0, 0, 0);
        m_targetColour = Color.black;
        m_lerpTime = 1;
        //m_lerpTime = m_fadeTime / m_fadeTimer;
    }

}
