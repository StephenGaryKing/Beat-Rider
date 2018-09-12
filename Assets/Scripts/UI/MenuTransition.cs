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

    Canvas canvas;

    private void Start() {
        Transform t = transform;
        while (t != null && canvas == null) {
            canvas = t.GetComponent<Canvas>();
            t = t.parent;
        }
    }

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
                Vector3 localPos = GetLocalPos(tran.ObjectToAnimate.TargetPosition);
                //Vector3 localPos = tran.ObjectToAnimate.TargetPosition;
                if (tran.ObjectToAnimate.Time == 0)
                {
                    tran.ObjectToAnimate.Obj.transform.localPosition = localPos;
                    tran.ObjectToAnimate.Obj.transform.localScale = tran.ObjectToAnimate.TargetScale;
                }
                else
                {
                    tweeners.Add(tran.ObjectToAnimate.Obj.transform.DOLocalMove(localPos, tran.ObjectToAnimate.Time));
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
                Vector3 localPos = GetLocalPos(tran.ObjectToAnimate.TargetPosition);
                if (tran.ObjectToAnimate.Time == 0)
                {
                    tran.ObjectToAnimate.Obj.transform.localPosition = localPos;
                    tran.ObjectToAnimate.Obj.transform.localScale = tran.ObjectToAnimate.TargetScale;
                }
                else
                {
                    tran.ObjectToAnimate.Obj.transform.DOLocalMove(localPos, tran.ObjectToAnimate.Time);
                    tran.ObjectToAnimate.Obj.transform.DOScale(tran.ObjectToAnimate.TargetScale, tran.ObjectToAnimate.Time);
                }
			}
		}
		yield return null;
	}

    Vector3 GetLocalPos(Vector3 anchorPos) {
        RectTransform rectTransform = transform as RectTransform;
        RectTransform canvasRect = canvas.transform as RectTransform;

        anchorPos.x += canvasRect.rect.width * (rectTransform.anchorMin.x - 0.5f);
        anchorPos.y += canvasRect.rect.height * (rectTransform.anchorMin.y - 0.5f);
        return anchorPos;
    }
}
