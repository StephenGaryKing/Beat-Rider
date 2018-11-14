using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	[System.Serializable]
	public class QuicktimeKey
	{
		public KeyCode Key;
		public Color NoteColour = Color.blue;
	}

	public class QuickTimeInput : MonoBehaviour
	{

		public float m_tollerance = 0.2f;
		public QuicktimeKey[] m_hardKeys = new QuicktimeKey[4];
		public QuicktimeKey m_meduimKey;

		Dictionary<KeyCode, float> m_previouslyPressedKeys = new Dictionary<KeyCode, float>();
		PlayerSoundEffects m_playerSoundEffects;
		ScoreBoardLogic m_scoreBoardLogic;
		[HideInInspector] public GameController m_gameController;

		private void Awake()
		{
			m_gameController = FindObjectOfType<GameController>();
			m_playerSoundEffects = GetComponent<PlayerSoundEffects>();
			m_scoreBoardLogic = FindObjectOfType<ScoreBoardLogic>();
		}

		private void Update()
		{
			/*
			if (m_previouslyPressedKeys.Count > 0)
			{
				foreach (var key in m_previouslyPressedKeys)
					Debug.Log(key.Key + " : " + key.Value);
			}
			*/
			FetchKey();
			DelKeys();
		}

		void FetchKey()
		{
			int e = System.Enum.GetNames(typeof(KeyCode)).Length;
			for (int i = 0; i < e; i++)
			{
				if (Input.GetKey((KeyCode)i))
				{
					if (m_previouslyPressedKeys.ContainsKey((KeyCode)i))
						m_previouslyPressedKeys[(KeyCode)i] = Time.time;
					else
						m_previouslyPressedKeys.Add((KeyCode)i, Time.time);
				}
			}
		}

		void DelKeys()
		{
			List<KeyValuePair<KeyCode, float>> valsToDel = new List<KeyValuePair<KeyCode, float>>();
			foreach (var key in m_previouslyPressedKeys)
				if (key.Value < Time.time - m_tollerance)
					valsToDel.Add(key);
			// del old keypresses
			foreach (var key in valsToDel)
				m_previouslyPressedKeys.Remove(key.Key);
		}

		public IEnumerator LookForKeyPress(int index, GameObject note)
		{
			if (GameController.GetDifficulty() == Difficulty.EASY)
			{
				PickupNote(note);
			}
			else
			{
				float timer = 0;
				bool keyFound = false;
				while (timer < m_tollerance)
				{
					List<KeyValuePair<KeyCode, float>> valsToDel = new List<KeyValuePair<KeyCode, float>>();
					timer += Time.deltaTime;
					foreach (var key in m_previouslyPressedKeys)
					{
						if (key.Key == ((GameController.GetDifficulty() == Difficulty.HARD) ? m_hardKeys[index].Key : m_meduimKey.Key))
						{
							PickupNote(note);
							m_previouslyPressedKeys.Remove(key.Key);
							keyFound = true;
							break;
						}
					}
					if (keyFound)
						break;
					yield return null;
				}
			}
		}

		void PickupNote(GameObject note)
		{
			m_scoreBoardLogic.PickupNote();
			note.SetActive(false);
			note.GetComponent<ParticleCreationLogic>().SpawnParticle();
			if (m_playerSoundEffects.m_pickupNote.soundToPlay)
				m_playerSoundEffects.m_soundManager.PlaySound(m_playerSoundEffects.m_pickupNote);
			AchievementManager.OnTallyPickups(note.tag);
		}
	}
}