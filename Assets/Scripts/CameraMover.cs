using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BeatRider
{
	public class CameraMover : MonoBehaviour
	{

		public Transform m_targetTransform;
		public float m_time = 1;
		public bool m_cinematicMovementEnabled = false;

		FloatingCameraLogic m_floatingCam;

		private void Start()
		{
			m_floatingCam = FindObjectOfType<FloatingCameraLogic>();
		}

		public void MoveToPos()
		{
			if (m_floatingCam)
				m_floatingCam.enabled = m_cinematicMovementEnabled;
			Camera.main.transform.DOMove(m_targetTransform.position, m_time);
			Camera.main.transform.DORotateQuaternion(m_targetTransform.rotation, m_time);
		}
	}
}