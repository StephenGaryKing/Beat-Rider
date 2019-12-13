using System.Collections;
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
    private PlayerCollision m_player = null;
	// Use this for initialization
	void Start () {
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
            m_player.totalGemDust = 0;
        }
        if (controller)
            m_controller = controller;


        m_notesHitText.text = "Notes Hit: " + notesHit;
        m_percentageText.text = "Percentage: " + Mathf.Round(notesHit/totalNotes);
        m_gemDustText.text = "Gem Dust: " + gemDust;
        m_shop.m_currentGemDust += gemDust;
    }

    public void MainMenu()
    {
        m_controller.ReturnToMenu();
    }
}
