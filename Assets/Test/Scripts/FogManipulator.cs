using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManipulator : MonoBehaviour {

    [SerializeField] private bool enableFog = true;
    [SerializeField] private bool enableSkybox = true;
    [SerializeField] private Material skybox = null;
    [SerializeField] private Material defaultSkybox = null;
    private Camera currentCamera = null;

    private void Awake()
    {
        defaultSkybox = RenderSettings.skybox;
    }

    // Use this for initialization
    void Start () {
		
	}

    private void OnEnable()
    {
        RenderSettings.fog = enableFog;
        if (skybox)
            RenderSettings.skybox = skybox;
        currentCamera = Camera.current;
        if (currentCamera)
            currentCamera.clearFlags = CameraClearFlags.Skybox;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnDisable()
    {
        RenderSettings.fog = !enableFog;
        RenderSettings.skybox = defaultSkybox;
        if (currentCamera)
            currentCamera.clearFlags = CameraClearFlags.SolidColor;

    }
}
