using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterObject : MonoBehaviour
{
   
        void Update()
        {
        // Get the main camera and the screen dimensions
        Camera mainCamera = Camera.main;
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        // Calculate the position of the center of the screen
        Vector3 screenCenter = mainCamera.ScreenToWorldPoint(new Vector3(screenSize.x / 2, screenSize.y / 2, 0));

        // Set the position of the object to the center of the screen
        transform.localPosition = screenCenter;
    }
    
}
