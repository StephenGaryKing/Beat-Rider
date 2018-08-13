using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Transition
{
	public GameObject ObjectToAnimate;
}

public class MenuTransition : MonoBehaviour {

	public Transition[] m_outTransitions;
	public Transition[] m_inTransitions;

	public void PlayOutTransitions()
	{
		StartCoroutine(OutTransition());
	}

	IEnumerator OutTransition()
	{
		foreach (Transition tran in m_outTransitions)
		{
			if (tran.ObjectToAnimate)
			{
				Destroy(tran.ObjectToAnimate.GetComponent<Animation>());
				tran.ObjectToAnimate.SetActive(false);
			}
		}
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
			if (tran.ObjectToAnimate)
			{
				tran.ObjectToAnimate.SetActive(true);
			}
		}
		yield return null;
	}
}
