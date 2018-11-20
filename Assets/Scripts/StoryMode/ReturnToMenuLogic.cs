using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

namespace BeatRider
{
	public class ReturnToMenuLogic : MonoBehaviour {

		public float m_timeToWait = 5f;

		float m_timer = 0;

		private void Start()
		{
            LevelGenerator.HideLevel();
		}

		// Update is called once per frame
		void Update() {
			m_timer += Time.deltaTime;
			if (FetchKey() && m_timer >= m_timeToWait)
			{
				SongController.ReturnToMenuStatic();
				LevelGenerator.ShowLevel();
				Destroy(gameObject);
			}
		}

		bool FetchKey()
		{
			int e = System.Enum.GetNames(typeof(KeyCode)).Length;
			for (int i = 0; i < e; i++)
			{
				if (Input.GetKey((KeyCode)i))
					return true;
			}
			return false;
		}
	}
}