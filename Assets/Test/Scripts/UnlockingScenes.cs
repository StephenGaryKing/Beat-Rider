using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnlockingScenes : MonoBehaviour {

    public ScenesList sceneToUnlock = ScenesList.None;
    public SceneryManager sceneManager = null;

    private void OnEnable()
    {
        sceneManager = FindObjectOfType<SceneryManager>();
        sceneManager.ActivateButton(sceneToUnlock);
    }
}
