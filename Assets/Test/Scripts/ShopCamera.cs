using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class ShopCamera : MonoBehaviour {
    [SerializeField] private float m_movementTime = 0.23f;
    [SerializeField] private TweenCurveHelper.CurveType CurveType;
    [SerializeField] private Vector3 previewPos = new  Vector3(0f, 8.5f, 50f);
    [SerializeField] private Vector3 selectionPos = new  Vector3(0f, 7f, 40f);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PreviewShip(bool state)
    {
        if (state)
            Tween.LocalPosition(transform, previewPos, m_movementTime, 0, TweenCurveHelper.GetCurve(CurveType), Tween.LoopType.None, null, null, false);
        else
            Tween.LocalPosition(transform, selectionPos, m_movementTime, 0, TweenCurveHelper.GetCurve(CurveType), Tween.LoopType.None, null, null, false);

    }
}
