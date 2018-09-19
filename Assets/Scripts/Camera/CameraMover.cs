using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

namespace BeatRider
{
	/// <summary>
	/// Moves the camera for use user interface animation
	/// </summary>
	public class CameraMover : MonoBehaviour
	{

		public Transform m_targetTransform;				//location to move the camera
		public float m_time = 1;						//time for animation to take (in seconds)
		public bool m_cinematicMovementEnabled = false; //should the ingame cinematics be disabled
		public TweenCurveHelper.CurveType m_curveType;    // using this type of tween curve

		FloatingCameraLogic m_floatingCam;				//the floating camera script. used when disabling ingame cinematics

		public void Start()
		{
			m_floatingCam = FindObjectOfType<FloatingCameraLogic>();
		}

		public void MoveToPos()
		{
			// set the cinematics of the camera
			if (m_floatingCam)
				m_floatingCam.enabled = m_cinematicMovementEnabled;
			else
				Debug.LogError("floating cam not found");
			// use the tweening library to move the camera to the desired position with the desired rotation
			Tween.LocalPosition(Camera.main.transform, m_targetTransform.position, m_time, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);
			Tween.LocalRotation(Camera.main.transform, m_targetTransform.rotation, m_time, 0, TweenCurveHelper.GetCurve(m_curveType), Tween.LoopType.None, null, null, false);

			//return tween;
		}
	}
}