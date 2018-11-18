using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class UnlockableManager : MonoBehaviour
	{
		public UnlockableColour[] m_unlockableColours;
		public UnlockableHighlight[] m_unlockableHighlights;
		public UnlockableShip[] m_unlockableShips;
		public UnlockableTrail[] m_unlockableTrails;

		[HideInInspector] public List<int> m_unlockedColours = new List<int>();
		[HideInInspector] public List<int> m_unlockedHighlights = new List<int>();
		[HideInInspector] public List<int> m_unlockedShips = new List<int>();
		[HideInInspector] public List<int> m_unlockedTrails = new List<int>();

		private void Update()
		{
			if (Input.GetKey(KeyCode.Insert))
				UnlockAll();
		}

		void UnlockAll()
		{
			UnlockWholeList(m_unlockableColours);
			UnlockWholeList(m_unlockableHighlights);
			UnlockWholeList(m_unlockableShips);
			UnlockWholeList(m_unlockableTrails);
		}

		public void LockAll()
		{
			m_unlockedColours.Clear();
			m_unlockedHighlights.Clear();
			m_unlockedShips.Clear();
			m_unlockedTrails.Clear();
		}

		void UnlockWholeList(Unlockable[] unlockable)
		{
			foreach (Unlockable unlock in unlockable)
				UnlockUnlockable(unlock);
		}

		public void UnlockUnlockable(Unlockable unlock)
		{
			UnlockableColour colour = unlock as UnlockableColour;
			if (colour)
				UnlockColour(colour);

			UnlockableHighlight highlight = unlock as UnlockableHighlight;
			if (highlight)
				UnlockHighlight(highlight);

			UnlockableShip ship = unlock as UnlockableShip;
			if (ship)
				UnlockShip(ship);

			UnlockableTrail trail = unlock as UnlockableTrail;
			if (trail)
				UnlockTrail(trail);
		}


		public void UnlockColour(UnlockableColour colour)
		{
			int index = FindUnlockedID(colour, m_unlockableColours);
			if (!m_unlockedColours.Contains(index))
				m_unlockedColours.Add(index);
		}

		public void UnlockHighlight(UnlockableHighlight highlight)
		{
			int index = FindUnlockedID(highlight, m_unlockableHighlights);
			if (!m_unlockedHighlights.Contains(index))
				m_unlockedHighlights.Add(index);
		}

		public void UnlockShip(UnlockableShip ship)
		{
			int index = FindUnlockedID(ship, m_unlockableShips);
			if (!m_unlockedShips.Contains(index))
				m_unlockedShips.Add(index);
		}

		public void UnlockTrail(UnlockableTrail trail)
		{
			int index = FindUnlockedID(trail, m_unlockableTrails);
			if (!m_unlockedTrails.Contains(index))
				m_unlockedTrails.Add(index);
		}

		public void SaveUnlockables()
		{
			SaveFile saveFile = new SaveFile();
			saveFile.AddList(m_unlockedColours);
			saveFile.AddList(m_unlockedHighlights);
			saveFile.AddList(m_unlockedShips);
			saveFile.AddList(m_unlockedTrails);
		}

		public int FindUnlockedID<T>(T unlockable, T[] arrayToSearch)
		{
			for (int i = 0; i < arrayToSearch.Length; i++)
			{
				if (EqualityComparer<T>.Default.Equals(unlockable, arrayToSearch[i]))
				{
					return i;
				}
			}
			return -1;
		}
	}
}