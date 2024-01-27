using UnityEngine;

public class Thrower : MonoBehaviour
{
    // First Declare a private local inputSystem
    private InputSystem _inputSystem;
    [SerializeField] private Transform _bananaSpawnPoint;
    [SerializeField] private float _bananaThrowForce;
    [SerializeField] private float _bananaThrowTorque;
    
    void OnEnable()
    {
        // Only do these GameManager level assignments in Start() if you need to reference a particular system
        // Assign the Local InputSystem to the GameManager's InputSystem
        _inputSystem = GameManager.Instance.Get<InputSystem>();
        // Hook into any events using the Action Assignment
        _inputSystem.OnLeftClick += ThrowBanana;
    }
    
    void ThrowBanana()
    {
        // Note, see how this GameManager code no longer requires the prefab to be set on the gameobject?
        Banana banana =
            GameManager.Instance.CreateInstance<Banana>(transform, _bananaSpawnPoint.position,
                _bananaSpawnPoint.rotation);
        
        var bananaRigidbody = banana.GetComponent<Rigidbody>();
        bananaRigidbody.AddForce(_bananaSpawnPoint.forward * _bananaThrowForce);
        bananaRigidbody.AddTorque(_bananaSpawnPoint.up * _bananaThrowTorque);
    }

    void OnDisable()
    {
        // Unsubscribe when you disable
        _inputSystem.OnLeftClick -= ThrowBanana;
    }
}
