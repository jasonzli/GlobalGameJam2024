using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyFallScript : MonoBehaviour
{
    [Header("Fall Animation")]
    public Animator monkeyFall;

    [Header("Chase Sequence")]
    public float chaseTime;
    public EnemyFollow monkeyAIScript;


    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Banana"))
        {
            monkeyAIScript.enabled = false;
            monkeyFall.SetTrigger("Flat");
            
            StartCoroutine(EnableScript());
        }
    }

    public IEnumerator EnableScript()
    {
        yield return new WaitForSeconds(chaseTime);
        monkeyAIScript.enabled = true;
    }
}
