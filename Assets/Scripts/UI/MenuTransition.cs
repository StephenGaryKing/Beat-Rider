using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

[System.Serializable]
public struct AnimateObject
{
	public GameObject Obj;
	public Vector3 TargetPosition;
	public Vector3 TargetScale;
	public float Time;
	// using this type of tween curve
	public TweenCurveHelper.CurveType CurveType;
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

	UIParticle m_UIparticle;
    Canvas canvas;

    private void Start() {
		m_UIparticle = GetComponent<UIParticle>();
		Transform t = transform;
        while (t != null && canvas == null) {
            canvas = t.GetComponent<Canvas>();
            t = t.parent;
        }
    }

    public void PlayTransitions()
	{
		InTransition();
		OutTransition();
		//if (m_UIparticle)
		//	m_UIparticle.Show(Vector3.zero);
	}

	public void PlayOutTransitions()
	{
		OutTransition();
	}

	void OutTransition()
	{
		// do the tween
		foreach (Transition tran in m_outTransitions)
		{
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
					RectTransform rt = tran.ObjectToAnimate.Obj.transform as RectTransform;
					if (rt)
						Tween.AnchoredPosition(tran.ObjectToAnimate.Obj.transform as RectTransform, tran.ObjectToAnimate.TargetPosition, tran.ObjectToAnimate.Time, 0, TweenCurveHelper.GetCurve(tran.ObjectToAnimate.CurveType), Tween.LoopType.None, null, TweenEnd, false);
					else
						Tween.LocalPosition(tran.ObjectToAnimate.Obj.transform, tran.ObjectToAnimate.TargetPosition, tran.ObjectToAnimate.Time, 0, TweenCurveHelper.GetCurve(tran.ObjectToAnimate.CurveType), Tween.LoopType.None, null, TweenEnd, false);

					Tween.LocalScale(tran.ObjectToAnimate.Obj.transform, tran.ObjectToAnimate.TargetScale, tran.ObjectToAnimate.Time, 0, TweenCurveHelper.GetCurve(tran.ObjectToAnimate.CurveType), Tween.LoopType.None, null, null, false);
				}
			}
            TweenEnd();
        }
	}

    public void PlayInTransitions()
	{
		InTransition();
	}

	void InTransition()
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
					RectTransform rt = tran.ObjectToAnimate.Obj.transform as RectTransform;
					if (rt)
						Tween.AnchoredPosition(rt, tran.ObjectToAnimate.TargetPosition, tran.ObjectToAnimate.Time, 0, TweenCurveHelper.GetCurve(tran.ObjectToAnimate.CurveType), Tween.LoopType.None, null, null, false);
					else
						Tween.LocalPosition(tran.ObjectToAnimate.Obj.transform, tran.ObjectToAnimate.TargetPosition, tran.ObjectToAnimate.Time, 0, TweenCurveHelper.GetCurve(tran.ObjectToAnimate.CurveType), Tween.LoopType.None, null, null, false);
					Tween.LocalScale(tran.ObjectToAnimate.Obj.transform, tran.ObjectToAnimate.TargetScale, tran.ObjectToAnimate.Time, 0, TweenCurveHelper.GetCurve(tran.ObjectToAnimate.CurveType), Tween.LoopType.None, null, null, false);
				}
			}
		}
	}

	public void TweenEnd()
	{
		foreach (Transition tran in m_outTransitions)
			if (tran.ObjectToToggleEnabled)
				tran.ObjectToToggleEnabled.SetActive(false);
	}

    Vector3 GetLocalPos(Vector3 anchorPos) {
        RectTransform rectTransform = transform as RectTransform;
        RectTransform canvasRect = canvas.transform as RectTransform;

        anchorPos.x += canvasRect.rect.width * (rectTransform.anchorMin.x - 0.5f);
        anchorPos.y += canvasRect.rect.height * (rectTransform.anchorMin.y - 0.5f);
        return anchorPos;
    }
}
