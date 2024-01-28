using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class MonkeyController : MonoBehaviour
{

    
    [Header("CustomizableFields")] [SerializeField]
    private float chaseTime = 4;
    
    [Header("Prefab Internal References")]
    [SerializeField] private LookAtConstraint _lookAtConstraint;
    [SerializeField] private Animator _FallAnimator;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private AudioSource _audioSource;
    
    [SerializeField] private AudioClip _monkeyFallClip;
    [SerializeField] private float _monkeyFallVolume = 0.5f;
    
    [SerializeField] private AudioClip _monkeyChaseClip;
    [SerializeField] private float _monkeyChaseVolume = 0.5f;

    public event Action OnSlippedOnPeel; 
    
    // Private fields
    private PlayerController _activePlayerTarget;
    private bool _hasFallen = false;
    
    void SetPlayerTarget(PlayerController target)
    {
        _activePlayerTarget = target;
        //Set the lookatConstraint's source to the target
        ConstraintSource constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = target.transform;
        constraintSource.weight = 1;
        _lookAtConstraint.SetSource(0, constraintSource);
        _audioSource.clip = _monkeyChaseClip;
        _audioSource.volume = _monkeyChaseVolume;
        _audioSource.loop = true;
        _audioSource.Play();
        
    }


    private void Start()
    {
        SetPlayerTarget(GameObject.FindWithTag("Player").GetComponent<PlayerController>());
        Vector3 sourcePostion = transform.position;//The position you want to place your agent
        NavMeshHit closestHit;
        if( NavMesh.SamplePosition(  sourcePostion, out closestHit, 500, 1 ) ){
            transform.position = closestHit.position;
        }

        _navMeshAgent.Warp(transform.position);
        _navMeshAgent.enabled = true;
    }

    void Update()
    {
        if (_hasFallen) return;
        
        _navMeshAgent.SetDestination(_activePlayerTarget.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BananaPeel"))
        {
            _hasFallen = true;
            _lookAtConstraint.constraintActive = false;

            _audioSource.Stop();
            _audioSource.clip = _monkeyFallClip;
            _audioSource.loop = false;
            _audioSource.volume = _monkeyFallVolume;
            _audioSource.Play();
            
            gameObject.tag = "Untagged";
            
            OnSlippedOnPeel?.Invoke();
            
            _FallAnimator.SetTrigger("Flat");
            StartCoroutine(RenableMovement());
        }
    }

    private IEnumerator RenableMovement()
    {
        float elapsedTime = 0;
        while (elapsedTime < chaseTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _hasFallen = false;
        gameObject.tag = "Monkey";
        _lookAtConstraint.constraintActive = true;
        _audioSource.Stop();
        _audioSource.clip = _monkeyChaseClip;
        _audioSource.volume = _monkeyChaseVolume;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    private void OnDisable()
    {
        _audioSource.Stop();
        OnSlippedOnPeel = null;
    }
}