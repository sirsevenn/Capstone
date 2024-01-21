using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    //Pool Location
    public Vector3 poolLocation;
    //Enemy Count
    public int goblinCount = 1;

    //Prefabs
    public GameObject goblinPrefab;

    [Space(20)]

    public List<GameObject> inactivePool = new List<GameObject>();
    public List<GameObject> activePool = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < goblinCount; i++)
        {
            GameObject clone = Instantiate(goblinPrefab, poolLocation, Quaternion.identity, this.transform);
            inactivePool.Add(clone);
        }
    }

    public GameObject GetObject()
    {
        GameObject clone = inactivePool[0];
        activePool.Add(clone);
        inactivePool.Remove(clone);

        return clone;
    }

    public void RemoveObject(GameObject clone)
    {
        activePool.Remove(clone);
        inactivePool.Add(clone);

        //reset clone
    }

}
