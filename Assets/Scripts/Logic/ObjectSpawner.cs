﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicalGameplayMechanics;

namespace BeatRider
{
	public class ObjectSpawner : Spawner
	{

		float m_secondsToLookAhead = 0;
		float m_objectSpeed = 0;

		List<GameObject>[] m_objectLists;
		int m_previousSpawnPosition = 1;

		ScoreBoardLogic m_scoreBoardLogic;
        [SerializeField] private bool m_isNoteSpawner = false;
        public int totalNotes = 0;

		public override void Awake()
		{
			int num = 0;
			for (int i = 0; i < m_prefabsToSpawn.Count; i ++)
			{
				num += (int)(m_prefabsToSpawn[i].chanceToSpawn * 100000f);
				m_prefabsToSpawn[i].realChanceToSpawn = num;
			}
			if (num > 100000)
				Debug.LogError("WARNING! \nThe object named " + transform.name + " threw an over 100% error... Check your \"Prefabs To Spawn\" chances");
			SongController.m_onEarlyMusicIsPlaying.AddListener(SpawnObject);
			SongController.m_onFrontBufferDump.AddListener(SpawnDump);

			CreatePool();
		}

		private void Start()
		{
			m_scoreBoardLogic = FindObjectOfType<ScoreBoardLogic>();
		}

		public void SetObjectSpeed(float speed, float lookAheadTime)
		{
			m_objectSpeed = speed;
			m_secondsToLookAhead = lookAheadTime;
			SongController.m_timeToLookAhead = lookAheadTime;
			//set the speed of all the objects
			foreach(var list in m_objectLists)
				foreach(var obj in list)
				{
					LaneMovement lm = obj.GetComponent<LaneMovement>();
					if (lm)
						lm.m_unitsToMovePerSecond = speed;
				}
		}

        //private void Update()
        //{
        //    if (Input.GetButtonDown("Spawn Number"))
        //        Debug.Log("Number of spawns: " + totalNotes);
        //}


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

		public void WipeObjects()
		{
			foreach (var list in m_objectLists)
				foreach (GameObject go in list)
				{
					if (go.activeInHierarchy)
					{
						ParticleCreationLogic pc = go.GetComponent<ParticleCreationLogic>();
						if (pc)
							pc.SpawnParticle();
						go.SetActive(false);
					}
				}
		}

		public void SpawnDump(List<SavedPass> passes, int index)
		{
			int numberOfSpawns = 0;
			Debug.Log(transform.name + " is performing spawn dump");
			foreach (SavedPass pass in passes)
			{
				if (m_passToReactTo != "DEFAULT" && pass.name != m_passToReactTo)
					continue;
				// go through the list till you reach the look ahead time
				foreach (var data in pass.runtimeData)
				{
					if (data.Value.time / 10f >= m_secondsToLookAhead)
					{
						Debug.Log(numberOfSpawns + " objects were spawned by " + transform.name);
						return;
					}
					// spawn object based on the time it occures in the song
					if (m_reactToBeat && !data.Value.isPeak)
						continue;
					if (m_audioReactor.ConditionsAreMet(data.Value) && m_spawningAreas.Length > 0)
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
						pos.z = m_objectSpeed * data.Value.time / 10f;
						int val = Random.Range(0, 100000);
						for (int i = 0; i < m_prefabsToSpawn.Count; i++)
						{
							if (val < m_prefabsToSpawn[i].realChanceToSpawn)
							{
								int j = 0;
								while (m_objectLists[i][j].activeInHierarchy)
									j++;

								m_objectLists[i][j].transform.position = pos;
								m_objectLists[i][j].transform.rotation = Quaternion.identity;
								m_objectLists[i][j].gameObject.SetActive(true);
								numberOfSpawns++;
                                if (m_isNoteSpawner)
                                    totalNotes++;
								i = m_prefabsToSpawn.Count;
							}
						}
					}
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