using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicalGameplayMechanics
{
	/// <summary>
	/// The type of graph to display
	/// </summary>
	[System.Serializable]
	enum GraphType
	{
		Bottom,
		Middle,
		Round
	}

	/// <summary>
	/// An easy to use and modifiable Plotter for audio spectrum data
	/// </summary>
	public class SpectrumPlotter : MonoBehaviour
	{

		SongController _songController;
		[SerializeField] int m_numberOfBars = 500;					// Number of bars in the graph
		[SerializeField] AudioReactor m_audioReactor;				// AudioReactor modifier
		[SerializeField] float m_keepPercentage = 60;				// Percentage of the spectrum to keep (starting from the bottom)
		[SerializeField] float m_HeightModifier = 20;				// Multiplier for height adjustment
		[SerializeField] bool m_logarithmicScale = false;			// Logarithmicly scale the spectrum to account for higher frequencies being quieter
		[SerializeField] float m_barMovementSmoothing = 0.1f;		// Smoothing to apply to the bars' vertical movement
		[SerializeField] float m_barAppearanceSmoothing = 0.1f;		// Smoothing to apply to the spectrum as a whole
		[SerializeField] GraphType m_graphType;						// Type of graph to draw

		int amountOfIndexesToKeep = 0;
		List<Transform> m_bars = new List<Transform>();

		// Use this for initialization
		void Start()
		{
			Destroy(GetComponent<Renderer>());

			const int spectrumSize = (1024 / 2) - 1;	// Spectrum size as an array index

			if (m_numberOfBars > spectrumSize)
				m_numberOfBars = spectrumSize;			// Cap the number of bars to the spectrum Size

			CreateSpectrum();
			amountOfIndexesToKeep = (int)(spectrumSize * Mathf.Min(m_keepPercentage / 100, 1));

			_songController = FindObjectOfType<SongController>();

			_songController.m_onMusicIsPlaying.AddListener(React);	// Always react to the music
		}

		/// <summary>
		/// Create bars based on the dimentions of this' transform
		/// </summary>
		void CreateSpectrum()
		{
			switch (m_graphType)
			{
				// Create cubes left to right with the anchor for each cube at the bottom
				case GraphType.Bottom :
					for (int i = 0; i < m_numberOfBars; i++)
					{
						GameObject middleMan = new GameObject();
						GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
						middleMan.transform.parent = transform;
						middleMan.transform.localPosition = Vector3.right * (i / (float)m_numberOfBars) - Vector3.right * 0.5f + Vector3.right * (1 / (float)m_numberOfBars / 2) - Vector3.up / 2;
						middleMan.transform.localRotation = Quaternion.identity;
						middleMan.transform.localScale = new Vector3(1 / (float)m_numberOfBars, 0.001f, 1);
						middleMan.transform.name = "bar " + i;
						obj.transform.parent = middleMan.transform;
						obj.transform.localPosition = Vector3.up / 2;
						obj.transform.localRotation = Quaternion.identity;
						obj.transform.localScale = Vector3.one;
						m_bars.Add(middleMan.transform);
					}
					break;

				// Create cubes left to right with the anchor for each cube int the middle
				case GraphType.Middle :
					for (int i = 0; i < m_numberOfBars; i++)
					{
						GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
						obj.transform.parent = transform;
						obj.transform.localPosition = Vector3.right * (i / (float)m_numberOfBars) - Vector3.right * 0.5f + Vector3.right * (1 / (float)m_numberOfBars / 2);
						obj.transform.localRotation = Quaternion.identity;
						obj.transform.localScale = new Vector3(1 / (float)m_numberOfBars, 0.001f, 1);
						obj.transform.name = "bar " + i;
						m_bars.Add(obj.transform);
					}
					break;

				// Create cubes around a point with the anchor for each cube at the bottom
				case GraphType.Round :
					for (int i = 0; i < m_numberOfBars; i++)
					{
						GameObject middleMan = new GameObject();
						GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
						middleMan.transform.parent = transform;
						obj.transform.parent = middleMan.transform;
						middleMan.transform.localPosition = Vector3.zero;
						middleMan.transform.localRotation = Quaternion.Euler(Vector3.back * (360f / m_numberOfBars) * i);
						middleMan.transform.localScale = new Vector3(0.01f, 0.001f, 1);
						middleMan.transform.name = "bar " + i;
						obj.transform.localPosition = Vector3.up / 2;
						obj.transform.localRotation = Quaternion.identity;
						obj.transform.localScale = Vector3.one;
						m_bars.Add(middleMan.transform);
					}
					break;
			}
		}

		private void FixedUpdate()
		{
			foreach(Transform bar in m_bars)
			{
				Vector3 destinationVector = new Vector3(bar.localScale.x, 0.001f, bar.localScale.z);
				bar.localScale = Vector3.Lerp(bar.localScale, destinationVector, m_barMovementSmoothing);	// lerp each bar to have zero height
			}
		}

		/// <summary>
		/// Update the spectrum plotter's bars based on the music
		/// </summary>
		/// <param name="sfi">
		/// List of SpectralFluxInfo's
		/// </param>
		/// <param name="index">
		/// Index at this point in time
		/// </param>
		public void React(List<SavedPass> passes, int index)
		{
			foreach (SavedPass pass in passes)
			{
				if (m_audioReactor == null || m_audioReactor.ConditionsAreMet(pass.runtimeData[index]))
				{
					float previousBarValue = 0;
					// modify the spectrum plotter
					for (int i = 0; i < m_numberOfBars; i++)
					{
						float Scale = (i > 0 && m_logarithmicScale) ? Mathf.Log10(i + 1) * m_HeightModifier : m_HeightModifier;     // Select the correct scaling method
						float value = 0;
						int indexToUse = (int)((amountOfIndexesToKeep / (float)m_numberOfBars) * i);                                // Select the correct index from the spectrum
																																	//Debug.Log("num " + i + " : " + indexToUse + " : " + amountOfIndexesToKeep + " : " + m_numberOfBars);
						value = _songController.m_curSpectrum[indexToUse];
						value *= Scale;

						value = Mathf.Lerp(previousBarValue, value, m_barAppearanceSmoothing);                                      // Lerp to smooth out the spectrum as a whole
						previousBarValue = value;

						if (value > m_bars[i].localScale.y)
							m_bars[i].localScale = new Vector3(m_bars[i].localScale.x, value, m_bars[i].localScale.z);              // Change the scale
					}
				}
			}
		}
	}
}
 