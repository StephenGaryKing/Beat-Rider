﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BeatRider;
using MusicalGameplayMechanics;

public class CompletionCanvas : MonoBehaviour {

    [SerializeField] private Text m_notesHitText = null;
    [SerializeField] private Text m_percentageText = null;
    [SerializeField] private Text m_gemDustText = null;
    [SerializeField] private Turntable m_shop = null;

    private SongController m_controller = null;
    [SerializeField] private PlayerCollision m_player = null;
	// Use this for initialization
	void Start () {
        if (!m_player)
            m_player = FindObjectOfType<PlayerCollision>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GameComplete(int notesHit = 0, int totalNotes = 0, SongController controller = null)
    {
        int gemDust = 0;
        if (m_player)
        {
            gemDust = m_player.totalGemDust;
            Debug.Log("Gem Dust is: " + gemDust + ". Player Gem Dust is: " + m_player.totalGemDust);
            m_player.totalGemDust = 0;
        }
        else
            Debug.Log("Player not found");

        if (controller)
            m_controller = controller;

        float percentage = ((float)notesHit / (float) totalNotes) * 100f;

        m_notesHitText.text = "Notes Hit: " + notesHit;
        m_percentageText.text = "Percentage: " + percentage.ToString("0");
        m_gemDustText.text = "Gem Dust: " + gemDust;
        m_shop.m_currentGemDust += gemDust;
    }

    public void MainMenu()
    {
        gameObject.SetActive(false);
        m_controller.ReturnToMenu();
    }
}