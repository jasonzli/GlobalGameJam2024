using UnityEngine;

public class Thrower : MonoBehaviour
{
    private InputSystem _inputSystem;
    [SerializeField] private Transform _bananaSpawnPoint;
    [SerializeField] private float _bananaThrowForce;
    [SerializeField] private float _bananaThrowTorque;

    void OnEnable()
    {
        _inputSystem = GameManager.Instance.Get<InputSystem>();
        _inputSystem.OnLeftClick += ThrowBanana;
    }

    void ThrowBanana()
    {
        Banana banana = GameManager.Instance.CreateInstance<Banana>(null, _bananaSpawnPoint.position, _bananaSpawnPoint.rotation);

        var bananaRigidbody = banana.GetComponent<Rigidbody>();
        bananaRigidbody.AddForce(_bananaSpawnPoint.forward * _bananaThrowForce);
        bananaRigidbody.AddTorque(_bananaSpawnPoint.up * _bananaThrowTorque);
    }

    void OnDisable()
    {
        _inputSystem.OnLeftClick -= ThrowBanana;
    }
}