using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraSmooth : MonoBehaviour
{
    public float rotationSpeed = 5f; 

    private bool isRotating = false;
    private Vector3 targetDirection;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetDirection = (hit.point - transform.position).normalized;
                isRotating = true;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
           
            isRotating = false;
        }

        if (isRotating)
        {
           
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}