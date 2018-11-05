using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MusicalGameplayMechanics;

namespace BeatRider
{
	public class ScoreBoardLogic : MonoBehaviour
	{
		[HideInInspector] public int m_score = 0;
		public Text m_scoreText = null;
		SongController m_songController;
		public ObjectSpawner m_noteSpawner;
		string m_passToCountBeats;
		int m_totalAmountOfNotes;
		public bool m_displayAsPercentage = false;

		float m_scoreToAdd = 0;

		private void Awake()
		{
			m_songController = FindObjectOfType<SongController>();
			m_passToCountBeats = m_noteSpawner.m_passToReactTo;
		}

		public void Update()
		{
            if (!m_songController)
                m_songController = FindObjectOfType<SongController>();
			if (m_displayAsPercentage)
				m_scoreText.text = FindPercentageOfNotes().ToString() + "%";
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

		public float FindPercentageOfNotes()
		{
			float percentage = Mathf.Round(((float)m_score / m_totalAmountOfNotes) * 100f);
			if (float.IsNaN(percentage))
				percentage = 0;
			return percentage;
		}
	}
}