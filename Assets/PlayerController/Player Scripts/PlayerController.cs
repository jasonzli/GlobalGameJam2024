using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public event Action OnBananaEaten;
    public event Action OnCaughtByMonkey;
    public Camera CameraComponent => playerCamera;

    [Header("Prefab References")] 
    [SerializeField] private AudioSource _runningAudioSource;
    [SerializeField] private AudioSource _bananaAudioSource;
    [SerializeField] private AudioClip _bananaEatenClip;
    [SerializeField] private float _bananaEatenVolume = 0.5f;
    [SerializeField] private AudioClip _runningClip;
    [SerializeField] private float _runningVolume = 0.5f;
        
    [Header("Camera")]
    public Camera playerCamera;
    public Animator cameraRotate;

    [Header("Thrower Script")]
    public Thrower bananaThrower;

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

    public float consumeBananaTime;

    [Header("Banana Placeholders")]
    public GameObject freshBanana;
    public GameObject bananaPeel;

    [Header("Bool Check")]
    public bool canMove = true;
    public bool isBanana = false;
    public bool bananaEaten = false;
    public bool canMoveCamera = true;

    CharacterController characterController;

    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        characterController = GetComponent<CharacterController>();
        _runningAudioSource.clip = _runningClip;
        _runningAudioSource.volume = _runningVolume;
        _runningAudioSource.loop = true;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void FreeCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        
        Animate();
        CameraRotate();

        // Handles Movement
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedZ = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedZ);
        
        //Horizontal movement Vector
        Vector3 horizontalMovement = new Vector3(moveDirection.x, 0.0f, moveDirection.z);
        float dotProduct = Vector3.Dot(horizontalMovement, transform.forward);
        float normDotProduct = (dotProduct + 1 ) /2;
        float speedModifier = Mathf.Lerp(.3f,1.0f,normDotProduct);

        moveDirection *= speedModifier;
        
        
        // Handles Jumping
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
        
        //Play the running clip at a volume depending on the magnitude of move direction
        _runningAudioSource.volume = _runningVolume * Mathf.Clamp01(Mathf.Sqrt(moveDirection.magnitude));
        characterController.Move(moveDirection * Time.deltaTime);

        // Handles Rotation
        

         if (canMove && canMoveCamera)
        {
            if (!cameraRotate.GetBool("Rotate")) // Check if not in the rotating animation state
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }

         if (Input.GetMouseButtonUp(1))
         {
             cameraRotate.SetBool("Rotate", false);
                 
         }
    }

    public void Animate()
    {
        if (isBanana)
        {
            if (Input.GetMouseButtonDown(0) && Time.time > grabCooldownTimer)
            {
                animator.SetTrigger("Grab");
                
                grabCooldownTimer = Time.time + grabCooldown;
                StartCoroutine(EatBanana());
            }
        }

        if (bananaEaten)
        {
            if (Input.GetMouseButtonDown(0) && Time.time > throwCooldownTimer)
            {
                animator.SetTrigger("Throw");
                bananaPeel.SetActive(false);
                StartCoroutine(DisableThrower());
                
                throwCooldownTimer = Time.time + throwCooldown;
            }
            
        }

        
    }
    
    public IEnumerator EatBanana()
    {
        yield return new WaitForSeconds(consumeBananaTime);
        _bananaAudioSource.clip = _bananaEatenClip;
        _bananaAudioSource.volume = _bananaEatenVolume;
        _bananaAudioSource.loop = false;
        _bananaAudioSource.Play();
        bananaPeel.SetActive(true);
        freshBanana.SetActive(false);
        bananaThrower.enabled = true;
        isBanana = false;
        bananaEaten = true;
        OnBananaEaten?.Invoke();
    }
    public void CameraRotate()
    {
        if (Input.GetMouseButtonDown(1) && Time.time > throwCooldownTimer)
        {
            cameraRotate.SetBool("Rotate", true);
            isBanana = false;
           
            throwCooldownTimer = Time.time + throwCooldown;
        }
        
        if (Input.GetMouseButtonUp(1) && Time.time > throwCooldownTimer)
        {
            cameraRotate.SetBool("Rotate", false);
            isBanana = true;
            
            throwCooldownTimer = Time.time + throwCooldown;
        }
    }
   
    public IEnumerator DisableThrower()
    {
        yield return new WaitForSeconds(1);
        bananaThrower.enabled = false;
        bananaEaten = false;
    }

    public void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.gameObject.CompareTag("Monkey"))
        {
            Debug.Log("Caught by monkey");
            OnCaughtByMonkey?.Invoke();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Banana"))
        {
            if (isBanana) return;
            
            freshBanana.SetActive(true);
            isBanana = true;
        }
    }
    
}