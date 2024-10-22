using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraScript2 : MonoBehaviour
{
    public GameObject John;  // Reference to John

    public float minY = -20f;  // Optional: Minimum Y position to prevent the camera from going too low
    public float maxY = 20f;  // Optional: Maximum Y position to prevent the camera from going too high

    public float smoothSpeed = 0.125f;  // Speed at which the camera smooths its movement
    public Vector3 offset;  // Offset to position the camera relative to John

    private void Start()
    {
        // Try to find John if it's not assigned in the Inspector
        if (John == null)
        {
            John = GameObject.FindWithTag("Player");  // Ensure John has the "Player" tag
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (John != null)
        {
            Vector3 position = transform.position;

            // Follow John's X and Y positions
            position.x = John.transform.position.x;
            position.y = Mathf.Clamp(John.transform.position.y, minY, maxY);  // Clamp Y position if necessary

            transform.position = position; 
        }
    }
}

