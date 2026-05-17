using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputIconsDB", menuName = "ScriptableObjects/InputIconsDB")]
public class InputIconsDB : ScriptableObject
{
    [Serializable]
    public struct DeviceIconMapping
    {
        public string bindingPath;
        public string spriteName;
    }

    public List<DeviceIconMapping> mappings;

    // Fast lookup dictionary
    public Dictionary<string, string> lookupDict = new Dictionary<string, string>();

    
    void OnEnable()
    {
        lookupDict.Clear();
        foreach (var mapping in mappings) {
            lookupDict.Add(mapping.bindingPath, mapping.spriteName);
        }
    }

}
