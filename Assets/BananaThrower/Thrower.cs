using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    private InputSystem _inputSystem;
    [SerializeField] private Transform _bananaSpawnPoint;
    [SerializeField] private float _bananaThrowForce;
    [SerializeField] private float _bananaThrowTorque;

    
    [SerializeField] private AudioSource _bananaAudioSource;
    [SerializeField] private AudioClip _bananaThrownClip;
    [SerializeField] private float _bananaThrownVolume = 0.5f;
    
    
    private List<GameObject> thrownBananas = new List<GameObject>();
    
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
        _bananaAudioSource.clip = _bananaThrownClip;
        _bananaAudioSource.volume = _bananaThrownVolume;
        _bananaAudioSource.loop = false;
        _bananaAudioSource.Play();
        
        thrownBananas.Add(banana.gameObject);
    }

    void OnDisable()
    {
        _inputSystem.OnLeftClick -= ThrowBanana;
    }

    public void Reset()
    {
        //Destroy all bananas in thrown bananas
        foreach (var banana in thrownBananas)
        {
            Destroy(banana);
        }
        thrownBananas.Clear();
    }
}