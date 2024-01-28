using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform target;

   
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found on this GameObject.");
        }
        else
        {
            // Adjust NavMeshAgent settings for smooth movement
            navMeshAgent.stoppingDistance = 2f;  // Adjust the stopping distance as needed
            navMeshAgent.angularSpeed = 360f;   // Adjust the angular speed as needed
            navMeshAgent.acceleration = 8f;     // Adjust the acceleration as needed
            navMeshAgent.autoBraking = true;    // Enable auto-braking for smoother stopping
        }
    }

    void Update()
    {
        if (target != null)
        {
            navMeshAgent.SetDestination(target.position);
        }

       
    }
}