using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour {

	Text m_notificationTextBox;
	Text m_loggerTextBox;
	ParticleSystemRenderer m_particleSystem;

	void Notify(Sprite img, string notificationText = null, string logText = null)
	{
		// make a new material for the particle emmiter
		Material mat = new Material(Shader.Find("Particles/Additive"));
		mat.mainTexture = img.texture;

		// play the particles
		m_particleSystem.material = mat;
		m_particleSystem.GetComponent<ParticleSystem>().Play();

		// update notify text
		if (notificationText != null)
			m_notificationTextBox.text = notificationText;

		// update log
		if (logText != null)
			m_loggerTextBox.text += logText + "\n";
	}
}
