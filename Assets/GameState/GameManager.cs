using System.Collections.Generic;
using GameState;
using UnityEngine;

/// <summary>
/// GameManager is where our globally accessible systems are stored as well as managing how to transition between particular
/// Scenes or Game States.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager Instance { get; private set; }
    
    // A customizable dictionary that we can use to dynamically load all prefabs that we need
    [SerializeField] private GameObjectReference _gameObjectReference;
    
    // Dictionary that stores all of the systems that we have created at runtime
    private readonly Dictionary<string,GameObject> _gameSystemDictionary = new ();

    void Awake()
    {
        if (_gameObjectReference == null)
        {
            Debug.LogError("You've attempted to start a scene without a game object reference set up.");
        }

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(this);
        
    }
    
    
    /// <summary>
    /// Returns the existing instance of a game manager system if it exists, otherwise creates a new one and returns it.
    /// </summary>
    /// <typeparam name="T">The Type of the System that you want to use</typeparam>
    /// <returns>The Component of system</returns>
    public T Get<T>() where T: MonoBehaviour
    {
        string systemName = typeof(T).Name;
        if (_gameSystemDictionary.TryGetValue(systemName, out var value))
        {
            // Return the existing system without creating a new one
            return value.GetComponent<T>();
        }
        else
        {
            // check if key is available in the GameObjectReference object
            if (!_gameObjectReference.IsKeyValid(systemName))
            {
                Debug.LogError($"Attempted to find a system with key name {systemName} but no key with that name was found.");
            }
            
            //Initialize system and add to dictionary
            var system = _gameObjectReference.InitializePrefabObject(systemName, transform).GetComponent<T>();
            _gameSystemDictionary.Add(systemName, system.gameObject);
            
            Debug.Log($"Added {systemName} to the game system dictionary.");
            return system;
        }
    }
}
