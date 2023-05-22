using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float sceneWidth = 9;
        float unitPerPixel = sceneWidth / Screen.width;
        float a = 0.5f * unitPerPixel * Screen.height;
        Camera.main.orthographicSize = a;
    }
}
