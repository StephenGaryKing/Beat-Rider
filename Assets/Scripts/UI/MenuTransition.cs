using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public struct AnimateObject
{
	public GameObject Obj;
	public Vector3 TargetPosition;
	public Vector3 TargetScale;
	public float Time;
}

[System.Serializable]
public struct Transition
{
	public GameObject ObjectToToggleEnabled;
	public AnimateObject ObjectToAnimate;
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
		List<Tweener> tweeners = new List<Tweener>();

		foreach (Transition tran in m_outTransitions)
			if (tran.ObjectToAnimate.Obj)
			{
                if (tran.ObjectToAnimate.Time == 0)
                {
                    tran.ObjectToAnimate.Obj.transform.localPosition = tran.ObjectToAnimate.TargetPosition;
                    tran.ObjectToAnimate.Obj.transform.localScale = tran.ObjectToAnimate.TargetScale;
                }
                else
                {
                    tweeners.Add(tran.ObjectToAnimate.Obj.transform.DOLocalMove(tran.ObjectToAnimate.TargetPosition, tran.ObjectToAnimate.Time));
                    tweeners.Add(tran.ObjectToAnimate.Obj.transform.DOScale(tran.ObjectToAnimate.TargetScale, tran.ObjectToAnimate.Time));
                }
			}

			foreach (var tween in tweeners)
				yield return tween.WaitForCompletion();

		foreach (Transition tran in m_outTransitions)
			if (tran.ObjectToToggleEnabled)
				tran.ObjectToToggleEnabled.SetActive(false);

		tweeners.Clear();
		yield return null;
	}

	public void PlayInTransitions()
	{
		StartCoroutine(InTransition());
	}

	IEnumerator InTransition()
	{
		foreach (Transition tran in m_inTransitions)
		{
			if (tran.ObjectToToggleEnabled)
				tran.ObjectToToggleEnabled.SetActive(true);
			if (tran.ObjectToAnimate.Obj)
			{
                if (tran.ObjectToAnimate.Time == 0)
                {
                    tran.ObjectToAnimate.Obj.transform.localPosition = tran.ObjectToAnimate.TargetPosition;
                    tran.ObjectToAnimate.Obj.transform.localScale = tran.ObjectToAnimate.TargetScale;
                }
                else
                {
                    tran.ObjectToAnimate.Obj.transform.DOLocalMove(tran.ObjectToAnimate.TargetPosition, tran.ObjectToAnimate.Time);
                    tran.ObjectToAnimate.Obj.transform.DOScale(tran.ObjectToAnimate.TargetScale, tran.ObjectToAnimate.Time);
                }
			}
		}
		yield return null;
	}
}
