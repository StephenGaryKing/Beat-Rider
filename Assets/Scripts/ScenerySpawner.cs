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
		public float m_maxRandomDelay = 0;
		public float m_randomXOffset = 0;
        public float m_minScale = 1;
        public float m_maxScale = 1;

        private void Start()
		{
			StartCoroutine(SpawnObject());
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawLine(transform.position - Vector3.right * m_randomXOffset, transform.position + Vector3.right * m_randomXOffset);
		}

		IEnumerator SpawnObject()
		{
			while (true)
			{
				int i = Random.Range(0, m_prefabsToSpawn.Length);
				GameObject go = Instantiate(m_prefabsToSpawn[i]);
				go.transform.position = transform.position + Vector3.right * Random.Range(-m_randomXOffset * 1000, m_randomXOffset * 1000)/1000f;
				go.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
				go.transform.localScale = Vector3.one * Random.Range(m_minScale, m_maxScale);
				yield return new WaitForSeconds(m_delay + (Random.Range(0, m_maxRandomDelay * 1000)/ 1000f));
			}
		}
	}
}