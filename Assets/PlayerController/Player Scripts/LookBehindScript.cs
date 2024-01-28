using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookBehindScript : MonoBehaviour
{
    public float rotationSpeed = 2.0f;
    public float returnSpeed = 5.0f;
    private bool isRotating = false;
    private Quaternion originalRotation;
    private Quaternion targetRotation;

    [Header("Player Control Script")]
    public PlayerController playerControls;

    void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isRotating)
        {
            isRotating = true;
            SetTargetRotation(-170f);
            playerControls.canMoveCamera = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            StartCoroutine(ReturnToOriginalRotation());
        }

        if (isRotating)
        {
            RotateCamera();
        }
    }

    void SetTargetRotation(float angle)
    {
        targetRotation = originalRotation * Quaternion.Euler(0, angle, 0);
    }

    void RotateCamera()
    {
        float t = Time.deltaTime * rotationSpeed;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);

        // Check if rotation is complete
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            isRotating = false;
        }
    }

    IEnumerator ReturnToOriginalRotation()
    {
        float elapsedTime = 0f;
        Quaternion currentCameraRotation = transform.rotation;

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Lerp(currentCameraRotation, originalRotation, elapsedTime * returnSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = originalRotation;
        playerControls.canMoveCamera = true;
        isRotating = false;
    }
}