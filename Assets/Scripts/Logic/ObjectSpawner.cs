using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

namespace BeatRider
{
	public class ObjectSpawner : Spawner
	{

		[SerializeField] int m_secondsToLookAhead = 0;

		List<GameObject>[] m_objectLists;
		int m_previousSpawnPosition = 1;

		public override void Start()
		{
			if (m_secondsToLookAhead > 0)
			{
				int num = 0;
				for (int i = 0; i < m_prefabsToSpawn.Count; i ++)
				{
					num += (int)(m_prefabsToSpawn[i].chanceToSpawn * 100000f);
					m_prefabsToSpawn[i].realChanceToSpawn = num;
				}
				if (num > 100000)
					Debug.LogError("WARNING! \nThe object named " + transform.name + " threw an over 100% error... Check your \"Prefabs To Spawn\" chances");
				_songController = FindObjectOfType<SongController>();
				_songController.m_onEarlyMusicIsPlaying.AddListener(SpawnObject);

				if (_songController.m_timeToLookAhead < m_secondsToLookAhead)
					_songController.m_timeToLookAhead = m_secondsToLookAhead;
			}
			else
				base.Start();
			CreatePool();
		}

		void CreatePool()
		{
			GameObject m_pooler = new GameObject("Pooler");
			m_pooler.transform.parent = transform;
			m_objectLists = new List<GameObject>[m_prefabsToSpawn.Count];
			for(int i = 0; i < m_prefabsToSpawn.Count; i ++)
			{
				m_objectLists[i] = new List<GameObject>();
				for (int j = 0; j < 50; j++)
				{
					GameObject go = Instantiate(m_prefabsToSpawn[i].prefab, m_pooler.transform);
					go.SetActive(false);
					m_objectLists[i].Add(go);
				}
			}
		}

		public override void SpawnObject(List<SavedPass> passes, int index)
		{
			foreach (SavedPass pass in passes)
			{
				if (m_passToReactTo != "DEFAULT" && pass.name != m_passToReactTo)
					continue;
				if (m_reactToBeat && !pass.runtimeData[index].isPeak)
					continue;
				if (m_audioReactor.ConditionsAreMet(pass.runtimeData[index]) && m_spawningAreas.Length > 0)
				{
					//move left or right in the lanes
					int ran = Random.Range(0, 3);
					if (ran == 0)
					{
						if (m_previousSpawnPosition == 0)
							m_previousSpawnPosition++;
						if (m_previousSpawnPosition != 0)
							m_previousSpawnPosition--;
					}
					if (ran == 2)
					{
						if (m_previousSpawnPosition == 2)
							m_previousSpawnPosition--;
						if (m_previousSpawnPosition != 2)
							m_previousSpawnPosition++;
					}

					SpawningArea sa = m_spawningAreas[m_previousSpawnPosition];
					Vector3 pos = sa.m_centerPosition;
					int val = Random.Range(0, 100000);
					for(int i = 0; i < m_prefabsToSpawn.Count; i++)
					{
						if (val < m_prefabsToSpawn[i].realChanceToSpawn)
						{
							int j = 0;
							while (m_objectLists[i][j].activeInHierarchy)
								j++;

							m_objectLists[i][j].transform.position = pos + transform.position;
							m_objectLists[i][j].transform.rotation = Quaternion.identity;
							m_objectLists[i][j].gameObject.SetActive(true);

							i = m_prefabsToSpawn.Count;
						}
					}
				}
			}
		}
	}
}