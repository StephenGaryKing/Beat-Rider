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
public class SceneButton
{
    public ScenesList sceneType;
    public GameObject button;
    public bool isActivated;
}


public class SceneryManager : MonoBehaviour {
    public List<SceneButton> sceneButtons = new List<SceneButton>();
    private string m_saveFileName = "Scenes";


    private void Start()
    {
        foreach(SceneButton entry in sceneButtons)
        {
            entry.button.GetComponent<Image>().color = Color.gray;
            entry.button.GetComponent<Button>().enabled = false;
            entry.isActivated = false;
        }
        LoadSceneButtons();
    }

    public void ActivateButton(ScenesList sceneToUnlock)
    {
        foreach(SceneButton buttonStruct in sceneButtons)
        {
            if (buttonStruct.sceneType == sceneToUnlock)
            {
                buttonStruct.button.GetComponent<Image>().color = Color.white;
                buttonStruct.button.GetComponent<Button>().enabled = true;
                buttonStruct.isActivated = true;
            }
        }
    }

    void LoadSceneButtons()
    {
        SaveFile loadFile = new SaveFile();

        if (loadFile.Load(m_saveFileName))
        {
            List<int> data = new List<int>();
            if (loadFile.m_numbers.Count > 0)
            {
                data = loadFile.m_numbers[0].list;
            }
            for(int i = 0; i < data.Count; i++)
            {
                if (data[i] == 1)
                {
                    sceneButtons[i].button.GetComponent<Image>().color = Color.white;
                    sceneButtons[i].button.GetComponent<Button>().enabled = true;
                    sceneButtons[i].isActivated = true;
                    sceneButtons[i].isActivated = true;
                }
                else
                {
                    sceneButtons[i].button.GetComponent<Image>().color = Color.gray;
                    sceneButtons[i].button.GetComponent<Button>().enabled = false;
                    sceneButtons[i].isActivated = false;
                }
            }
        }
        else
        {
            SaveSceneButtons();
        }
    }

    void SaveSceneButtons()
    {
        List<int> data = new List<int>();

        foreach(SceneButton entry in sceneButtons)
        {
            if (entry.isActivated)
                data.Add(1);
            else
                data.Add(0);
        }
        SaveFile save = new SaveFile();
        save.AddList(data);
        save.Save(m_saveFileName);
    }

    private void OnApplicationQuit()
    {
        SaveSceneButtons();
    }

}
