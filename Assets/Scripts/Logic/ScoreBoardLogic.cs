using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MusicalGameplayMechanics;

namespace BeatRider
{
	public class ScoreBoardLogic : MonoBehaviour
	{
		public Color m_winColour = Color.green;
		public Color m_noWinColour = Color.yellow;
		[HideInInspector] public int m_score = 0;
		public Text m_scoreText = null;
		SongController m_songController;
		public ObjectSpawner m_noteSpawner;
		string m_passToCountBeats;
		int m_totalAmountOfNotes;
		public bool m_displayAsPercentage = false;
        LevelGenerator m_levelGen;

		float m_scoreToAdd = 0;

		private void Awake()
		{
            m_levelGen = FindObjectOfType<LevelGenerator>();
			m_songController = FindObjectOfType<SongController>();
			m_passToCountBeats = m_noteSpawner.m_passToReactTo;
		}

		public void Update()
		{
            if (!m_songController)
                m_songController = FindObjectOfType<SongController>();
			if (m_displayAsPercentage)
			{
				float percent = FindPercentageOfNotes();
				if (percent >= 100)
					m_scoreText.color = m_winColour;
				else
					m_scoreText.color = m_noWinColour;
				m_scoreText.text = percent.ToString() + "%";
			}
			else
				m_scoreText.text = m_score.ToString() + "/" + m_totalAmountOfNotes.ToString();
		}

		public void PickupNote()
		{
			m_score++;
		}

		public void ResetScores()
		{
			m_score = 0;
		}

		public void FindAllNotes(List<SavedPass> passes)
		{
			m_totalAmountOfNotes = 0;
			foreach (SavedPass pass in passes)
				if (pass.name == m_passToCountBeats)
					foreach(var data in pass.runtimeData)
						if (data.Value.isPeak)
							if (m_noteSpawner.m_audioReactor.ConditionsAreMet(data.Value))
								m_totalAmountOfNotes++;
		}

        float Percent(float minVal, float maxVal)
        {
            float percentage = (minVal / maxVal) * 100f;
            if (float.IsNaN(percentage))
                percentage = 0;
            return percentage;
        }

		public float FindPercentageOfNotes()
		{
            float percent = 0;
            switch (GameController.GetDifficulty())
            {
                case (Difficulty.EASY):
                    percent = Percent(m_score, m_totalAmountOfNotes);
                    percent = Percent(percent, m_levelGen.m_currentLevel.m_easyPercantage);
                    break;
                case (Difficulty.MEDUIM):
                    percent = Percent(m_score, m_totalAmountOfNotes);
                    percent = Percent(percent, m_levelGen.m_currentLevel.m_mediumPercantage);
                    break;
                case (Difficulty.HARD):
                    percent = Percent(m_score, m_totalAmountOfNotes);
                    percent = Percent(percent, m_levelGen.m_currentLevel.m_hardPercantage);
                    break;
            }

            return Mathf.Round(percent);
        }

        
	}
}