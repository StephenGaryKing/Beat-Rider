using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class UnlockableManager : MonoBehaviour
	{
		public string m_saveFileName = "Unlockables";

		public UnlockableColour[] m_unlockableColours;
		public UnlockableColour[] m_unlockableHighlights;
		public UnlockableShip[] m_unlockableShips;
		public UnlockableTrail[] m_unlockableTrails;

		[HideInInspector] public List<int> m_unlockedColours = new List<int>();
		[HideInInspector] public List<int> m_unlockedHighlights = new List<int>();
		[HideInInspector] public List<int> m_unlockedShips = new List<int>();
		[HideInInspector] public List<int> m_unlockedTrails = new List<int>();

		public void Start()
		{
			//LoadUnlockables();
		}

		public void UnlockColour(UnlockableColour colour)
		{
			int index = FindUnlockedID(colour, m_unlockableColours);
			if (!m_unlockedColours.Contains(index))
				m_unlockedColours.Add(index);
		}

		public void UnlockHighlight(UnlockableColour highlight)
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
			saveFile.Save(m_saveFileName);
		}

		public void LoadUnlockables()
		{
			SaveFile saveFile = new SaveFile();
			saveFile.Load(m_saveFileName);

			m_unlockedColours = saveFile.m_numbers[0].list;
			m_unlockedHighlights = saveFile.m_numbers[1].list;
			m_unlockedShips = saveFile.m_numbers[2].list;
			m_unlockedTrails = saveFile.m_numbers[3].list;
		}

		int FindUnlockedID<T>(T unlockable, T[] arrayToSearch)
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