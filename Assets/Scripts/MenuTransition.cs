using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public struct Transition
{
	public GameObject ObjectToAnimate;
	public Vector3 TargetPosition;
	public float Time;
}

public class MenuTransition : MonoBehaviour {

	public Transition[] m_outTransitions;
	public Transition[] m_inTransitions;

	public void PlayTransitions()
	{
		StartCoroutine(InTransition());
		StartCoroutine(OutTransition());
	}

	public void PlayOutTransitions()
	{
		StartCoroutine(OutTransition());
	}

	IEnumerator OutTransition()
	{
		Debug.Log("Started Out Transition");

		List<Tweener> tweeners = new List<Tweener>();

		foreach (Transition tran in m_outTransitions)
			if (tran.ObjectToAnimate)
				tweeners.Add(tran.ObjectToAnimate.transform.DOLocalMove(tran.TargetPosition, tran.Time));

			foreach (var tween in tweeners)
				yield return tween.WaitForCompletion();

		foreach (Transition tran in m_outTransitions)
			if (tran.ObjectToAnimate)
				tran.ObjectToAnimate.SetActive(false);

		tweeners.Clear();

		foreach (Transition tran in m_outTransitions)
			if (tran.ObjectToAnimate)
				tran.ObjectToAnimate.transform.position -= tran.TargetPosition;

		Debug.Log("Finished Out Transition");
		yield return null;
	}

	public void PlayInTransitions()
	{
		StartCoroutine(InTransition());
	}

	IEnumerator InTransition()
	{
		Debug.Log("Started In Transition");

		foreach (Transition tran in m_inTransitions)
		{
			if (tran.ObjectToAnimate)
			{
				tran.ObjectToAnimate.SetActive(true);
				tran.ObjectToAnimate.transform.DOLocalMove(tran.TargetPosition, tran.Time);
			}
		}

		Debug.Log("Finished In Transition");
		yield return null;
	}
}
