using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    
    [Header("Camera")]
    public Camera playerCamera;

    [Header("Speed")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;

    [Header("Gravity")]
    public float jumpPower = 7f;
    public float gravity = 10f;
 
    [Header("Sensitivity and Limits")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
 
 
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;



    [Header("Animation")]
    public Animator animator;

     public float grabCooldown = 1.5f; 
    private float grabCooldownTimer;

    public float throwCooldown = 1.5f;  
    private float throwCooldownTimer;
 
    [Header("Bool Check")]
    public bool canMove = true;
    public bool isBanana = false;
    public bool bananaEaten = false;


 
    
    CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
 
    void Update()
    {
        Animate();
 
        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
 
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
 
        #endregion
 
        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
 
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
 
        #endregion
 
        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);
 
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
 
        #endregion
    }

    public void Animate()
    {

        if(isBanana)
        {
            if (Input.GetKeyDown(KeyCode.E) && Time.time > grabCooldownTimer)
            {
                animator.SetTrigger("Grab");


                grabCooldownTimer = Time.time + grabCooldown;
            }
        }
        

        if(bananaEaten)
        {
            if (Input.GetMouseButtonDown(0) && Time.time > throwCooldownTimer)
            {
                animator.SetTrigger("Throw");

                throwCooldownTimer = Time.time + throwCooldown;
            }
        }
        
        
    }
}