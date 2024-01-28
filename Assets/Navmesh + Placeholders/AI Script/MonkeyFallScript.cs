using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MonkeyFallScript : MonoBehaviour
{
    [Header("Fall Animation")]
    public Animator monkeyFall;

    [Header("Chase Sequence")]
    public float chaseTime;
    public EnemyFollow monkeyAIScript;

    public LookAtConstraint lookAtConstraint;


    // Update is called once per frame

    void Start()
    {
        lookAtConstraint = GetComponent<LookAtConstraint>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Banana"))
        {
            monkeyAIScript.enabled = false;
            lookAtConstraint.constraintActive = false;
            
            monkeyFall.SetTrigger("Flat");
            
            StartCoroutine(EnableScript());
        }
    }

    public IEnumerator EnableScript()
    {
        yield return new WaitForSeconds(chaseTime);
        monkeyAIScript.enabled = true;
        lookAtConstraint.constraintActive = true;
    }
}
