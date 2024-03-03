using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneDictionary<TKey, TValue>
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();
    [SerializeField]
    private List<TValue> values = new List<TValue>();

    public void Add(TKey key, TValue value)
    {
        keys.Add(key);
        values.Add(value);
    }

    public bool ContainsKey(TKey key)
    {
        return keys.Contains(key);
    }

    public TValue GetValue(TKey key)
    {
        int index = keys.IndexOf(key);
        if (index != -1)
        {
            return values[index];
        }
        else
        {
            Debug.LogError("Key not found: " + key);
            return default(TValue);
        }
    }
}
