using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoLink : MonoBehaviour {

    public Button yourButton;


    // Use this for initialization

    void Start ()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        Debug.Log("Demolink Script is working...");
    }

    // Update is called once per frame

    public virtual void Select()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=NsGE74GZqz0");
        Debug.Log("Fish");
    }

    public void URLOpenA ()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=NsGE74GZqz0");

    }

    void TaskOnClick()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=NsGE74GZqz0");
        Debug.Log("Click");
    }

    public void URLOpenB ()
    {
        // Application.OpenURL("https://www.youtube.com/watch?v=NsGE74GZqz0");
        Debug.Log("Fish");
    }

    void Update () {
		
	}
}
