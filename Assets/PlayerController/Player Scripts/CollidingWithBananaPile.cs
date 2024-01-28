using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingWithBananaPile : MonoBehaviour
{
    [Header("BananaPileColider")]
    public GameObject bananaOnHand;

    [Header("Check Can Eat")]
    public PlayerController playerController;

    public bool DestroyOnCollide = false;
    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (DestroyOnCollide) Destroy(gameObject);
        }
    }
}
