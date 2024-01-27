using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraSmooth : MonoBehaviour
{
    public float rotationSpeed = 5f; 

    private bool isRotating = false;
    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
           
            isRotating = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
           
            isRotating = false;
        }

        if (isRotating)
        {
           
            Quaternion targetRotation = originalRotation * Quaternion.Euler(0f, -170f, 0f);

           
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
           
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);
        }
    }
}