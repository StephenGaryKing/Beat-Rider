using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCreationLogic : MonoBehaviour {

	public GameObject m_particlePrefab;
	public bool m_createOnDeath = true;

	bool m_quitting = false;

	private void OnApplicationQuit()
	{
		m_quitting = true;
	}
	private void OnDestroy()
	{
		if (!m_quitting)
		{
			if (m_createOnDeath)
				SpawnParticle();
		}
	}

	public void SpawnParticle()
	{
		GameObject go = Instantiate(m_particlePrefab);
		go.transform.position = transform.position;
		float lifetime = go.GetComponent<ParticleSystem>().main.duration;
		Destroy(go, lifetime);
	}
}
