using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class ScenerySpawner : MonoBehaviour
	{
		public GameObject[] m_prefabsToSpawn;
		public float m_delay = 1;
		float m_SpeedToMoveScenery;
		public Vector3 m_BuildingScale = Vector3.one;

        private void Start()
		{
			//pupulate track from the start
			for (int i = 0; i < 100; i ++)
			{
				GameObject building = SpawnBuilding();
				building.transform.position += Vector3.back * (i * 250 * m_delay) ;
			}

			StartCoroutine(SpawnObject());
		}

		private void OnDrawGizmos()
		{
			const float HeightMod = 10;
			Vector3 tempScale = m_BuildingScale;
			tempScale.y *= HeightMod;
			Gizmos.DrawCube((Vector3.up * m_BuildingScale.y * HeightMod / 2) + transform.position, tempScale);
		}

		IEnumerator SpawnObject()
		{
			while (true)
			{
				SpawnBuilding();
				yield return new WaitForSeconds(m_delay);
			}
		}

		GameObject SpawnBuilding()
		{
			//Spawn Buildings
			int ran = Random.Range(0, m_prefabsToSpawn.Length);
			GameObject obj = Instantiate(m_prefabsToSpawn[ran]);
			// position and scale the spawned building
			obj.transform.position = transform.position;
			obj.transform.localScale = m_BuildingScale;
			return obj;
		}
	}
}