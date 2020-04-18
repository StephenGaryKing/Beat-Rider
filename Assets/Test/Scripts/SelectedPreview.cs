using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedPreview : MonoBehaviour
{

    [SerializeField] private float speed = 0.1f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed, 0);
    }
}
