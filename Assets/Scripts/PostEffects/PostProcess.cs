using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcess : MonoBehaviour {

	public List<Material> m_mats;
	RenderTexture m_tempBuffer;
	RenderTexture m_middleManBuffer;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!m_tempBuffer)
			m_tempBuffer = RenderTexture.GetTemporary(source.descriptor);
		if (!m_middleManBuffer)
			m_middleManBuffer = RenderTexture.GetTemporary(m_tempBuffer.descriptor);

		if (m_mats.Count > 1)
		{
			// apply first shader to the temp buffer
			ApplyVariables(0, source);
			Graphics.Blit(source, m_tempBuffer, m_mats[0]);

			// loop though all post shaders
			for (int i = 1; i < m_mats.Count - 1; i++)
			{
				if (m_mats[i])
				{
					ApplyVariables(i, source);
					Graphics.Blit(m_tempBuffer, m_middleManBuffer, m_mats[i]);
					Graphics.CopyTexture(m_middleManBuffer, m_tempBuffer);
				}
			}

			// apply the final post shader to the screen
			ApplyVariables(m_mats.Count - 1, source);
			Graphics.Blit(m_tempBuffer, destination, m_mats[m_mats.Count - 1]);
		}
		else if (m_mats.Count == 1)
		{
			if (m_mats[0])
			{
				// only use 1 post shader
				ApplyVariables(0, source);
				Graphics.Blit(source, destination, m_mats[0]);
			}
		}
		// don't apply any post effects
		else
			Graphics.Blit(source, destination);
	}

	/// <summary>
	/// Apply the variables needed for post processing
	/// </summary>
	/// <param name="i">
	/// Index of the material to change
	/// </param>
	/// <param name="source">
	/// Image to get size from
	/// </param>
	void ApplyVariables(int i, RenderTexture source)
	{
		// apply the variables needed for post processing
		if (m_mats[i].HasProperty("_DeltaX"))
		{
			m_mats[i].SetFloat("_DeltaX", 1.0f / source.width);
			m_mats[i].SetFloat("_DeltaY", 1.0f / source.height);
		}
	}
}