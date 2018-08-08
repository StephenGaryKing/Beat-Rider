using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicalGameplayMechanics
{
	/// <summary>
	/// The area in which to spawn the objects
	/// </summary>
	[System.Serializable]
	public struct SpawningArea
	{
		public Vector3 m_centerPosition;
		public Vector3 m_size;
	}

	[System.Serializable]
	public class SpawnableObject
	{
		public float chanceToSpawn;
		[HideInInspector] public int realChanceToSpawn;
		public GameObject prefab;
	}

	/// <summary>
	/// Base class for spawning objects based on sound
	/// </summary>
	public class ObjectSpawner : MonoBehaviour
	{
		protected SongController _songController;
		public List<SpawnableObject> m_prefabsToSpawn;

		// What to react to
		public string m_passToReactTo = "DEFAULT";
		public bool m_reactToBeat = false;

		Color m_lineColour = new Color(1, 0, 0, 0.7f);		// Line colour used to draw the spawning area
		public SpawningArea[] m_spawningAreas;

		public AudioReactor m_audioReactor;					// Modifier to change the way this spawns objects

		public virtual void Start()
		{
			int num = 0;
			for (int i = 0; i < m_prefabsToSpawn.Count; i++)
			{
				num += (int)(m_prefabsToSpawn[i].chanceToSpawn * 100000f);
				m_prefabsToSpawn[i].realChanceToSpawn = num;
			}
			Debug.Log(num);
			if (num > 1000000)
				Debug.LogError("WARNING! \nThe object named " + transform.name + " threw an over 100% error... \nCheck your Prefabs To Spawn chances" );
			_songController = FindObjectOfType<SongController>();
			_songController.m_onMusicIsPlaying.AddListener(SpawnObject);
		}

		/// <summary>
		/// Draw the area to spawn the objects in as a wire cube (a sphere if the spawn area is a point)
		/// </summary>
		void OnDrawGizmos()
		{
			if (m_spawningAreas != null)
			{
				for (int i = 0; i < m_spawningAreas.Length; i++)
				{
					Gizmos.color = m_lineColour;
					if (m_spawningAreas[i].m_size == Vector3.zero)
						Gizmos.DrawSphere(transform.position + m_spawningAreas[i].m_centerPosition, 0.5f * transform.localScale.magnitude);
					else
						Gizmos.DrawWireCube(transform.position + m_spawningAreas[i].m_centerPosition, m_spawningAreas[i].m_size);
				}
			}
		}

		/// <summary>
		/// Spawns an object in a random spot within the spawn area
		/// </summary>
		/// <param name="sfi">
		/// List of spectralFluxInfo's
		/// </param>
		/// <param name="index">
		/// The index of the SpectralFluxInfo's to use
		/// </param>
		public virtual void SpawnObject(List<SavedPass> passes, int index)
		{
			foreach (SavedPass pass in passes)
			{
				if (m_passToReactTo != "DEFAULT" && pass.name != m_passToReactTo)
					continue;
				if (m_reactToBeat && !pass.runtimeData[index].isPeak)
					continue;
				if (m_audioReactor.ConditionsAreMet(pass.runtimeData[index]) && m_spawningAreas.Length > 0)
				{
					if (m_reactToBeat && !pass.runtimeData[index].isPeak)
						continue;
					//find a random point in the spawning areas
					int ran = Random.Range(0, m_spawningAreas.Length);
					SpawningArea sa = m_spawningAreas[ran];
					Vector3 pos = new Vector3(Random.Range(-sa.m_size.x / 2, sa.m_size.x / 2), Random.Range(-sa.m_size.y / 2, sa.m_size.y / 2), Random.Range(-sa.m_size.z / 2, sa.m_size.z / 2));
					int val = Random.Range(0, 100000);
					for (int i = 0; i < m_prefabsToSpawn.Count; i++)
					{
						if (val < m_prefabsToSpawn[i].realChanceToSpawn)
						{
							Instantiate(m_prefabsToSpawn[i].prefab, pos + transform.position, Quaternion.identity);
							i = m_prefabsToSpawn.Count;
						}
					}
				}
			}
		}
	}
}