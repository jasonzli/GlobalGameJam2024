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
            Debug.LogError("NavMeshAgent component not found.");
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