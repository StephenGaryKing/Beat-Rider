using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeatRider;
using UnityEngine.UI;

public enum ScenesList
{
    Canyon,
    Forest,
    Underwater,
    Blizzard,
    Sewer,
    Desert,
    Highway,
    Galaxy,
    Volcano,
    None
}

public class SceneryManager : MonoBehaviour {
    public Dictionary<ScenesList, GameObject> sceneButtons = new Dictionary<ScenesList, GameObject>();

    private void Start()
    {
        foreach(KeyValuePair<ScenesList, GameObject> entry in sceneButtons)
        {
            entry.Value.GetComponent<Image>().color = Color.gray;
            entry.Value.GetComponent<Button>().enabled = false;
        }
    }

    public void ActivateButton(ScenesList sceneToUnlock)
    {
        if (sceneButtons.ContainsKey(sceneToUnlock))
        {
            GameObject go = sceneButtons[sceneToUnlock];
            go.GetComponent<Image>().color = Color.white;
            go.GetComponent<Button>().enabled = true;
        }
    }

}
