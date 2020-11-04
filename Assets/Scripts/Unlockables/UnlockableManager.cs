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

        int numberOfClassicPaints = 0;
        [SerializeField] private int maxClassicPaints = 8;
        bool classpaintsAchieved = false;

        private void Start()
        {
            foreach(UnlockableShip ship in m_unlockableShips)
            {
                ship.m_metallicTexture = ship.m_material.GetTexture("_MetallicGlossMap");
            }
            Steamworks.SteamUserStats.GetStat("classicpaintsCollected", out numberOfClassicPaints);
        }

        public void UnlockAll()
		{
			Debug.LogError("Unlocking All Unlockables");
			UnlockWholeList(m_unlockableColours);
			UnlockWholeList(m_unlockableHighlights);
			UnlockWholeList(m_unlockableShips);
			UnlockWholeList(m_unlockableTrails);
		}

		public void LockAll()
		{
            foreach (int listNumber in m_unlockedColours)
                m_unlockableColours[listNumber].unlocked = false;

            foreach (int listNumber in m_unlockedHighlights)
                m_unlockableHighlights[listNumber].unlocked = false;

            foreach (int listNumber in m_unlockedShips)
                m_unlockableShips[listNumber].unlocked = false;

            foreach (int listNumber in m_unlockedTrails)
                m_unlockableTrails[listNumber].unlocked = false;

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
            {
                m_unlockedColours.Add(index);
                m_unlockableColours[index].unlocked = true;
                if (m_unlockableColours[index].m_isMetallic == false)
                {
                    numberOfClassicPaints++;
                    Steamworks.SteamUserStats.SetStat("classicpaintsCollected", numberOfClassicPaints);
                    Steamworks.SteamUserStats.StoreStats();
                }
                if (numberOfClassicPaints >= maxClassicPaints)
                {
                    Steamworks.SteamUserStats.SetAchievement("classicpaintsAchievement");
                }
            }
        }

		public void UnlockHighlight(UnlockableHighlight highlight)
		{
			int index = FindUnlockedID(highlight, m_unlockableHighlights);
			if (!m_unlockedHighlights.Contains(index))
            {
                m_unlockedHighlights.Add(index);
                m_unlockableHighlights[index].unlocked = true;
            }

        }

        public void UnlockShip(UnlockableShip ship)
		{
			int index = FindUnlockedID(ship, m_unlockableShips);
            if (!m_unlockedShips.Contains(index))
            {
                m_unlockedShips.Add(index);
                m_unlockableShips[index].unlocked = true;
            }
        }

		public void UnlockTrail(UnlockableTrail trail)
		{
			int index = FindUnlockedID(trail, m_unlockableTrails);
			if (!m_unlockedTrails.Contains(index))
            {
                m_unlockedTrails.Add(index);
                m_unlockableTrails[index].unlocked = true;
            }
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