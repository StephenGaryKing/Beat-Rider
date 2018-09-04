using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCameraAnimationLogic : MonoBehaviour {

	public void PlayAnimation(Animation anim)
	{
		Camera.main.gameObject.AddComponent<Animation>();

	}
}
