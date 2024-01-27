using System.Collections.Generic;
using GameState;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// GameManager is where our globally accessible systems are stored as well as managing how to transition between particular
/// Scenes or Game States.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager Instance { get; private set; }
    
    // A customizable dictionary that we can use to dynamically load all prefabs that we need
    [SerializeField] private GameObjectReference _systemGameObjectReference;
    [SerializeField] private GameObjectReference _gameplayObjectReference;
    
    // Dictionary that stores all of the systems that we have created at runtime
    private readonly Dictionary<string,GameObject> _gameSystemDictionary = new ();

    void Awake()
    {
        if (_systemGameObjectReference == null)
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
    /// Creates an instance of a gameplay object by name and returns it. If no transform is provided, it will use the GameManager's transform.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="trans"></param>
    /// <returns>Instance of the gameplay object's matching component</returns>
    /// <remarks>Use this method to create instances of prefabs that are not systems. Uses the GamePrefab Dictionary asset</remarks>
    public T CreateInstance<T>() where T: MonoBehaviour
    {
        string key = typeof(T).Name;
        if (!_gameplayObjectReference.IsKeyValid(key))
        {
            Debug.LogError($"Attempted to create a gameplay object with key name {key} but no key with that name was found.");
        }
        var gameplayObject = _gameplayObjectReference.InitializePrefabObject(key, transform);
        return gameplayObject.GetComponent<T>();
    }
    
    
    /// <summary>
    /// Returns the existing instance of a game manager system if it exists, otherwise creates a new one and returns it.
    /// </summary>
    /// <typeparam name="T">The Type of the System that you want to use</typeparam>
    /// <returns>The Component of system</returns>
    /// <remarks>Use this method to create instances of prefabs that are not systems. Uses the SystemPrefabObject Dictionary asset</remarks>
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
            if (!_systemGameObjectReference.IsKeyValid(systemName))
            {
                Debug.LogError($"Attempted to find a system with key name {systemName} but no key with that name was found.");
            }
            
            //Initialize system and add to dictionary
            var system = _systemGameObjectReference.InitializePrefabObject(systemName, transform).GetComponent<T>();
            _gameSystemDictionary.Add(systemName, system.gameObject);
            
            Debug.Log($"Added {systemName} to the game system dictionary.");
            return system;
        }
    }
    
        #region CreateInstance overloads for parent, position, and rotation
    // CreateInstance overloads for parent, position, and rotation
    public T CreateInstance<T>(Transform parent, Vector3 position, Quaternion rotation) where T: MonoBehaviour
    {
        var component = CreateInstance<T>();
        Transform componentTransform = component.transform;
        componentTransform.position = position;
        componentTransform.rotation = rotation;
        componentTransform.parent = parent;
        return component;
    }
    
    public T CreateInstance<T>(Vector3 position, Quaternion rotation) where T: MonoBehaviour
    {
        var component = CreateInstance<T>();
        Transform componentTransform = component.transform;
        componentTransform.position = position;
        componentTransform.rotation = rotation;
        return component;
    }
    
    public T CreateInstance<T>(Transform parent, Vector3 position) where T: MonoBehaviour
    {
        var component = CreateInstance<T>();
        Transform componentTransform = component.transform;
        componentTransform.position = position;
        componentTransform.parent = parent;
        return component;
    }
    
    public T CreateInstance<T>(Vector3 position) where T: MonoBehaviour
    {
        var component = CreateInstance<T>();
        Transform componentTransform = component.transform;
        componentTransform.position = position;
        return component;
    }
    
    public T CreateInstance<T>(Transform parent, Quaternion rotation) where T: MonoBehaviour
    {
        var component = CreateInstance<T>();
        Transform componentTransform = component.transform;
        componentTransform.rotation = rotation;
        componentTransform.parent = parent;
        return component;
    }
    
    public T CreateInstance<T>(Quaternion rotation) where T: MonoBehaviour
    {
        var component = CreateInstance<T>();
        Transform componentTransform = component.transform;
        componentTransform.rotation = rotation;
        return component;
    }
    
    public T CreateInstance<T>(Transform parent) where T: MonoBehaviour
    {
        var component = CreateInstance<T>();
        Transform componentTransform = component.transform;
        componentTransform.parent = parent;
        return component;
    }
    #endregion
}
