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

[System.Serializable]
public struct SceneButton
{
    public ScenesList sceneType;
    public GameObject button;
}


public class SceneryManager : MonoBehaviour {
    public List<SceneButton> sceneButtons = new List<SceneButton>();

    private void Start()
    {
        foreach(SceneButton entry in sceneButtons)
        {
            entry.button.GetComponent<Image>().color = Color.gray;
            entry.button.GetComponent<Button>().enabled = false;
        }
    }

    public void ActivateButton(ScenesList sceneToUnlock)
    {
        foreach(SceneButton buttonStruct in sceneButtons)
        {
            if (buttonStruct.sceneType == sceneToUnlock)
            {
                buttonStruct.button.GetComponent<Image>().color = Color.white;
                buttonStruct.button.GetComponent<Button>().enabled = true;
            }
        }
    }

}
