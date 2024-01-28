using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _rigidbody;
    
    // When the banana collides with the ground, it should stop moving and become a static object
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            //align transform with ground object
            transform.rotation = Quaternion.FromToRotation(Vector3.up, other.contacts[0].normal);
            
            // Set to be static (no long physics)
            _rigidbody.isKinematic = true;
            _collider.isTrigger = true;
        }
        
        // Destroy object if collies with Monkey or Player
        // Let them handle the logic of what happens when they get hit
        if (other.gameObject.CompareTag("Monkey") || other.gameObject.CompareTag("Player"))
        {
            //Destroy(gameObject);
        }
    }
    
    
}
