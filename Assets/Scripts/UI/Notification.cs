﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;

public class Notification : MonoBehaviour {

	public Text m_notificationTextBox;
	public Text m_loggerTextBox;
	public ParticleSystemRenderer m_particleSystem;
	[Header("Notification Text Animation")]
	public float m_targetscale = 1;
    public float m_startingScale = 2;
    public float m_animationTime = 1;
	public TweenCurveHelper.CurveType m_curveType;
    public Sound m_notifySound;
    SoundManager m_soundManager;

	bool m_tweening = false;

	private void Start()
	{
        m_soundManager = FindObjectOfType<SoundManager>();
		m_notificationTextBox.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.PageUp))
			Notify(Color.white, null, "You Unlocked Test!");
	}

	public void Notify(Color col, Sprite img, string notificationText = null, string logText = null)
	{
        if (!m_soundManager)
            m_soundManager = FindObjectOfType<SoundManager>();
        if (m_notifySound.soundToPlay)
            m_soundManager.PlaySound(m_notifySound);

		// make a new material for the particle emmiter
		Material mat = new Material(m_particleSystem.material);
		if (img)
			mat.mainTexture = img.texture;
		mat.color = col;

		// play the particles
		m_particleSystem.material = mat;
		m_particleSystem.GetComponent<ParticleSystem>().Play();

		// update/animate notify text
		if (!m_tweening && notificationText != null)
		{
			m_notificationTextBox.text = notificationText;
			AnimateNotifyText();
		}

		// update log
		if (logText != null)
			m_loggerTextBox.text += logText + "\n";
	}

	void AnimateNotifyText()
	{
		m_notificationTextBox.gameObject.SetActive(true);
		m_tweening= true;
        (m_notificationTextBox.transform as RectTransform).localScale = Vector3.one * m_startingScale;
        Tween.LocalScale(m_notificationTextBox.transform as RectTransform, Vector3.one * m_targetscale, m_animationTime, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, TweenEnd);
	}

	void TweenEnd()
	{
		m_notificationTextBox.gameObject.SetActive(false);
		m_tweening = false;
	}
}
