using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryModifier : MonoBehaviour {

	public float m_percentageOfSnapPointsToUse = 0.5f;
	public Transform[] m_snapPoints;
	public GameObject[] m_snapablePrefabs;

	List<GameObject> m_snapablePool;
	List<GameObject> m_snappedGameObjects = new List<GameObject>();

	// Use this for initialization
	void Awake()
	{
		if (m_percentageOfSnapPointsToUse > 1)
			m_percentageOfSnapPointsToUse = 1;
	}

	public void CreatePool()
	{
		// create a pool of objects to be used
		m_snapablePool = new List<GameObject>();
		for (int i = 0; i < (m_snapPoints.Length * m_percentageOfSnapPointsToUse) * 2; i++)
		{
			foreach (var obj in m_snapablePrefabs)
			{
				GameObject go = Instantiate(obj, transform);
				go.SetActive(false);
				m_snapablePool.Add(go);
			}
		}
	}

	public void ResetScenery()
	{
		foreach (var obj in m_snappedGameObjects)
			obj.SetActive(false);
		m_snappedGameObjects.Clear();
	}

	public void ModifyScenery()
	{
		List<Transform> snappablePoints = new List<Transform>(m_snapPoints);
		List<GameObject> snappableObjects = new List<GameObject>(m_snapablePool);
		// pick some random points and snap some objects
		for (int i = 0; i < m_snapPoints.Length * m_percentageOfSnapPointsToUse; i ++)
		{
			// pick a snap point
			int ranPoint = Random.Range(0, snappablePoints.Count - 1);
			// pick a random object
			int ranObj = Random.Range(0, snappableObjects.Count - 1);

			// put the object there
			snappableObjects[ranObj].transform.localScale = snappablePoints[ranPoint].localScale;
			snappableObjects[ranObj].transform.localPosition = snappablePoints[ranPoint].localPosition;
			snappableObjects[ranObj].transform.localRotation = snappablePoints[ranPoint].localRotation;
			// remove both from their lists after taking note of what is snapped
			snappableObjects[ranObj].SetActive(true);
			m_snappedGameObjects.Add(snappableObjects[ranObj]);
			snappablePoints.RemoveAt(ranPoint);
			snappableObjects.RemoveAt(ranObj);
		}
	}
}
