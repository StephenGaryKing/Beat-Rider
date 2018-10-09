using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticle : MonoBehaviour {

    public GameObject prefab;
    public GameObject instance;

    // Update is called once per frame
    void Update()
    {
        float textureWidth = ParticleStudio.inst.textureWidth;
        float cameraWidth = ParticleStudio.inst.cameraWidth;

        // update the position to match the orthographic camera, so the particles in the RawImage sit over the component
        if (instance)
            instance.transform.position = this.transform.localPosition * cameraWidth / textureWidth;
    }

    public void Show()
    {
        if (instance == null)
            instance = Instantiate(prefab);

        // toggle on and off to restart particle system (and whatever else)
        instance.SetActive(false);
        instance.SetActive(true);
    }

    //public void Show(Vector3 pos)
    //{
    //    if (instance == null)
    //    {
    //        instance = Instantiate(prefab);
    //        instance.transform.position = pos;
    //    }

    //    // toggle on and off to restart particle system (and whatever else)
    //    instance.SetActive(false);
    //    instance.SetActive(true);
    //}

}
