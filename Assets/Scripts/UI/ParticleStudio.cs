using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStudio : MonoBehaviour {

    // singleton class that stores parameters from the particle studio camera to 
    public static ParticleStudio inst;
    public int textureWidth;
    public float cameraWidth;

    void Start()
    {
        inst = this;
        // get the camera
        Camera cam = GetComponent<Camera>();
        if (cam && cam.targetTexture)
        {
            textureWidth = cam.targetTexture.width;
            cameraWidth = cam.orthographicSize * 2;
        }
    }
}
