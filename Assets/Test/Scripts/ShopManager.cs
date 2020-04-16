using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeatRider;


public class ShopManager : MonoBehaviour {

    [SerializeField] private string m_saveFileName = "Shop";
    // Money variables
    [SerializeField] private int m_initialGemDust = 10;
    /*[HideInInspector] */public int m_currentGemDust = 0;


    // Use this for initialization
    void Start () {
        LoadDust();
	}

    void LoadDust()
    {
        SaveFile loadFile = new SaveFile();

        if (loadFile.Load(m_saveFileName))
        {
            List<int> data = new List<int>();
            if (loadFile.m_numbers.Count > 0)
            {
                data = loadFile.m_numbers[0].list;
                m_currentGemDust += data[0];
            }
        }
        else
        {
            m_currentGemDust = m_initialGemDust;
            SaveDust();
        }
    }

    void SaveDust()
    {
        List<int> data = new List<int>();

        data.Add(m_currentGemDust);

        SaveFile save = new SaveFile();
        save.AddList(data);
        save.Save(m_saveFileName);
    }



    // Update is called once per frame
    void Update () {
		
	}

    private void OnApplicationQuit()
    {
        SaveDust();
    }
}
