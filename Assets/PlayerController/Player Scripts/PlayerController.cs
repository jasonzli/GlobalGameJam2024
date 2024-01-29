using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Set this in Setup() with the manager system.
    private InputSystem _inputSystem;
    
    #region External Properties
    public event Action OnBananaEaten;
    public event Action OnCaughtByMonkey;
    public Camera CameraComponent => _playerCamera;
    #endregion
    
    #region Prefab References
    [Header("Prefab Object References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Thrower _bananaThrower;
    [SerializeField] private GameObject _heldWholeBanana;
    [SerializeField] private GameObject _heldBananaPeel;
    
    [Header("Prefab Audio References")] 
    [SerializeField] private AudioSource _runningAudioSource;
    [SerializeField] private AudioSource _bananaAudioSource;
    [SerializeField] private AudioSource _gameAudioSource;
    [SerializeField] private AudioClip _bananaEatenClip;
    [SerializeField] private float _bananaEatenVolume = 0.3f;
    [SerializeField] private AudioClip _runningClip;
    [SerializeField] private float _runningVolume = 0.2f;
    
    [Header("Prefab Animator References")] 
    [SerializeField] private Animator _armAnimator;
    [SerializeField] private Animator _cameraRotationAnimator;
    #endregion
    
    #region Character Values and Properties
    [Header("Character Properties")]
    [SerializeField] private float walkSpeed = 15f;
    [SerializeField,Range(0f,1f)] private float backwardsSpeedModifier = .3f;
    [SerializeField] private float jumpPower = 7f;
    [SerializeField] private float gravity = 14f;
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float lookXLimit = 44;
    [SerializeField] private float grabCooldown = 1.5f;
    [SerializeField] private float throwCooldown = .5f;
    [SerializeField] private float timeToConsumeBanana = 2f;

    [Header("Bool Check")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool isBanana = false;
    [SerializeField] private bool bananaEaten = false;
    [SerializeField] private bool canMoveCamera = true;
    #endregion

    #region Private Fields
    private Vector3 movementVector;
    float rotationX = 0;
    private float grabCooldownTimer;
    private float throwCooldownTimer;
    #endregion
    
    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        _inputSystem = GameManager.Instance.Get<InputSystem>();
        
        _inputSystem.OnSpacePressed += Jump;
        
        _runningAudioSource.clip = _runningClip;
        _runningAudioSource.volume = _runningVolume;
        _runningAudioSource.loop = true;
        
        // TODO this should really not be in the player controller unless we're testing just the character
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void FreeCursor()
    {
        // TODO again this should not be in the player controller
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// This jump is assigned to the event in the input system.
    /// While this is a little cumbersome it does let us specify what the jump *does*
    /// </summary>
    void Jump()
    {
        if (_characterController.isGrounded)
        {
            movementVector.y = jumpPower;
        }
    }
    
    /// <summary>
    /// Determines what the movement vector should be for this frame
    /// </summary>
    void HandleMovementInput()
    {
        // Handle the horizontal movement
        Vector2 movementInput = _inputSystem.InputAxisResponse();
        movementVector.x = walkSpeed * movementInput.x;
        movementVector.z = walkSpeed * movementInput.y; //Vector2 to 3 direction
        
        // Make character slowing if going backwards
        float dotProduct = Vector3.Dot(movementVector, transform.forward);
        float normDotProduct = (dotProduct + 1 ) /2;
        float speedModifier = Mathf.Lerp(backwardsSpeedModifier,1.0f,normDotProduct);

        movementVector *= speedModifier;
        
        // Handle Vertical Movement
        if (!_characterController.isGrounded)
            movementVector.y -= gravity * Time.deltaTime;
        
        _characterController.Move(movementVector * Time.deltaTime);
    }

    void UseItem()
    {
        
    }

    void Update()
    {
        
        Animate();
        CameraRotate();

        HandleMovementInput();
        
        //Play the running clip at a volume depending on the magnitude of move direction
        _runningAudioSource.volume = _runningVolume * Mathf.Clamp01(Mathf.Sqrt(movementVector.magnitude));

        // Handles Rotation
        if (canMove && canMoveCamera)
        {
            if (!_cameraRotationAnimator.GetBool("Rotate")) // Check if not in the rotating animation state
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                _playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }

         if (Input.GetMouseButtonUp(1))
         {
             _cameraRotationAnimator.SetBool("Rotate", false);
         }
    }

    public void Animate()
    {
        if (isBanana)
        {
            if (Input.GetMouseButtonDown(0) && Time.time > grabCooldownTimer)
            {
                _armAnimator.SetTrigger("Grab");
                
                grabCooldownTimer = Time.time + grabCooldown;
                StartCoroutine(EatBanana());
            }
        }

        if (bananaEaten)
        {
            if (Input.GetMouseButtonDown(0) && Time.time > throwCooldownTimer)
            {
                _armAnimator.SetTrigger("Throw");
                _heldBananaPeel.SetActive(false);
                StartCoroutine(DisableThrower());
                
                throwCooldownTimer = Time.time + throwCooldown;
            }
        }
    }
    
    public IEnumerator EatBanana()
    {
        yield return new WaitForSeconds(timeToConsumeBanana);
        _bananaAudioSource.clip = _bananaEatenClip;
        _bananaAudioSource.volume = _bananaEatenVolume;
        _bananaAudioSource.loop = false;
        _bananaAudioSource.Play();
        _heldBananaPeel.SetActive(true);
        _heldWholeBanana.SetActive(false);
        _bananaThrower.enabled = true;
        isBanana = false;
        bananaEaten = true;
        OnBananaEaten?.Invoke();
    }
    public void CameraRotate()
    {
        if (Input.GetMouseButtonDown(1) && Time.time > throwCooldownTimer)
        {
            _cameraRotationAnimator.SetBool("Rotate", true);
            isBanana = false;
           
            throwCooldownTimer = Time.time + throwCooldown;
        }
        
        if (Input.GetMouseButtonUp(1) && Time.time > throwCooldownTimer)
        {
            _cameraRotationAnimator.SetBool("Rotate", false);
            isBanana = true;
            
            throwCooldownTimer = Time.time + throwCooldown;
        }
    }
   
    public IEnumerator DisableThrower()
    {
        yield return new WaitForSeconds(1);
        _bananaThrower.enabled = false;
        bananaEaten = false;
    }

    public void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.gameObject.CompareTag("Monkey"))
        {
            Debug.Log("Caught by monkey");
            OnCaughtByMonkey?.Invoke();
            _gameAudioSource.Stop();
            _bananaAudioSource.Stop();
            _runningAudioSource.Stop();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Banana"))
        {
            if (isBanana) return;
            
            _heldWholeBanana.SetActive(true);
            isBanana = true;
        }
    }

    private void OnDisable()
    {
        _bananaThrower.Reset();
        _inputSystem.OnSpacePressed -= Jump;
        
    }
    
}