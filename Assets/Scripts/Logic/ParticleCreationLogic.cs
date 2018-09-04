using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCreationLogic : MonoBehaviour {

	public GameObject m_particlePrefab;

	public void SpawnParticle()
	{
		GameObject go = Instantiate(m_particlePrefab);
		go.transform.position = transform.position;
		float lifetime = go.GetComponent<ParticleSystem>().main.duration;
		Destroy(go, lifetime);
	}
}
