using UnityEngine;

public class Thrower : MonoBehaviour
{
    private InputSystem _inputSystem;
    [SerializeField] private GameObject _bananaPrefabObject;
    [SerializeField] private Transform _bananaSpawnPoint;
    [SerializeField] private float _bananaThrowForce;
    [SerializeField] private float _bananaThrowTorque;
    
    void Start()
    {
        // Only do these GameManager level assignments in Start() if you need to reference a particular system
        _inputSystem = GameManager.Instance.Get<InputSystem>();
        _inputSystem.OnLeftClick += ThrowBanana;    
    }
    
    void ThrowBanana()
    {
        var banana = Instantiate(_bananaPrefabObject, _bananaSpawnPoint.position, _bananaSpawnPoint.rotation);
        var bananaRigidbody = banana.GetComponent<Rigidbody>();
        bananaRigidbody.AddForce(_bananaSpawnPoint.forward * _bananaThrowForce);
        bananaRigidbody.AddTorque(_bananaSpawnPoint.up * _bananaThrowTorque);
    }

    void OnDisable()
    {
        _inputSystem.OnLeftClick -= ThrowBanana;
    }
}
