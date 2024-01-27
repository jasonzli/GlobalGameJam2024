using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraSmooth : MonoBehaviour
{
    public float rotationSpeed = 5f; // Adjust the rotation speed as needed

    private bool isRotating = false;
    private Quaternion originalRotation;
    private Quaternion targetRotation;

    void Start()
    {
        originalRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0f, -170f, 0f) * originalRotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Right mouse button pressed, start rotating
            isRotating = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            // Right mouse button released, stop rotating
            isRotating = false;
        }

        if (isRotating)
        {
            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}