using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingWithBananaPile : MonoBehaviour
{
    [Header("BananaPileColider")]
    public GameObject bananaOnHand;

    [Header("Check Can Eat")]
    public FPSController playerController;
    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bananaOnHand.SetActive(true);
            playerController.isBanana = true;
            Destroy(gameObject);
        }
    }
}
