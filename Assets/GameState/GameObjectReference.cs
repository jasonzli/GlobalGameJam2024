using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    // Make it viewable in the inspector
    [System.Serializable]
    public class GameObjectReferenceEntry
    {
        public string name = "";
        public GameObject prefabObject = null;
    }
    
    /// <summary>
    /// A ScriptableObject that contains a dictionary of GameObjects that can be referenced by name. Use to create
    /// instances of prefabs when you need to
    /// </summary>
    [CreateAssetMenu(fileName = "PrefabObject Dictionary", menuName = "PrefabObjectDictionary", order = 0)]
    public class GameObjectReference : ScriptableObject
    {
        // The array of objects we want to reference
        [Header("The array of objects we want to reference")]
        [Tooltip("Use name for the KEY that you want to get the prefab by, and the prefabObject is the prefab that you want to create")]
        [SerializeField] private GameObjectReferenceEntry[] prefabObjects = null;

        // The internal dictionary that is created from the array of objects
        private Dictionary<string, GameObject> _prefabObjectDictionary = new(); 
        
        public bool IsKeyValid(string key) => _prefabObjectDictionary.ContainsKey(key);
        
        void OnEnable()
        {
            // create the dictionary from the GameObject reference entries
            _prefabObjectDictionary = new Dictionary<string, GameObject>();
            foreach (var prefabObject in prefabObjects)
            {
                if (_prefabObjectDictionary.ContainsKey(prefabObject.name))
                {
                    Debug.LogWarning($"Attempted to add a prefab with key name {prefabObject.name} but a key with that name already exists.");
                    continue;
                }
                _prefabObjectDictionary.Add(prefabObject.name, prefabObject.prefabObject);
            }
        }
        
        public GameObject InitializePrefabObject(string prefabName, Transform parent)
        {
            // Go through the prefabObjects array and find the prefab entry with the matching name, initialize that prefab and return it
            // check if key is in the dictionary
            if (!_prefabObjectDictionary.ContainsKey(prefabName))
            {
                Debug.LogError($"Attempted to create a prefab with key name {prefabName} but no key with that name was found.");
            }
            var prefabObject = _prefabObjectDictionary[prefabName];
            if (prefabObject == null)
            {
                Debug.LogError($"Attempted to create a prefab with key name {prefabName} but no prefab for that key was found.");
            }
            return Instantiate(prefabObject, parent);
        }
    }
}