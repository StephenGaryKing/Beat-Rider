using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicalGameplayMechanics
{
	/// <summary>
	/// Base abstract class for manipulating objects through the use of sound
	/// </summary>
	public abstract class GameObjectManipulator : MonoBehaviour
	{
		SongController _songController;

		// used to find what the maxVal and minVal should be
		[Header("Debug")]
		[SerializeField]
		protected float m_currentValue;

		[Header("Variables")]
		// what to react to
		[SerializeField] protected string m_passToReactTo = "DEFAULT";
		[SerializeField] protected bool m_reactToBeat = false;
		[SerializeField] protected bool m_reactToMusic = false;

		// how should this react
		[SerializeField] protected float m_smoothing = 1;

		// the Maximum and minimum value to scale this object's transform between
		[SerializeField] protected float m_maxValue = 1;
		[SerializeField] protected float m_minValue = 0;

		public AudioReactor m_audioReactor;

		// Use this for initialization
		protected virtual void Start()
		{
			_songController = FindObjectOfType<SongController>();
			_songController.m_onMusicIsPlaying.AddListener(ManipulateObject);
		}

		/// <summary>
		/// Manipulate the object
		/// </summary>
		/// <param name="sfi">
		/// List of SpectralFluxInfo's
		/// </param>
		/// <param name="index">
		/// Index of SpectralFluxInfo's at this point in time
		/// </param>
		public abstract void ManipulateObject(List<SavedPass> passes, int index);
	}
}