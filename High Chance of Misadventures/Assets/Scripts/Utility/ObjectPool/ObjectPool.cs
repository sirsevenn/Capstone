using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    None,
    EnemyType1,
    EnemyType2,
    EnemyType3,
    Boss
}

public class ObjectPool : MonoBehaviour
{

    #region singleton
    public static ObjectPool Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;

    }
    #endregion

    //Pool Location
    public Vector3 poolLocation;
    //Enemy Count
    public int maxEnemyCountPerType = 5;

    //Prefabs
    [Header("Prefabs")]
    public GameObject EnemyType1Prefab;
    public GameObject EnemyType2Prefab;
    public GameObject EnemyType3Prefab;
    public GameObject bossPrefab;

    [Space(20)]

    public List<GameObject> inactivePool_EnemyType1 = new List<GameObject>();
    public List<GameObject> activePool_EnemyType1 = new List<GameObject>();

    public List<GameObject> inactivePool_EnemyType2 = new List<GameObject>();
    public List<GameObject> activePool_EnemyType2 = new List<GameObject>();

    public List<GameObject> inactivePool_EnemyType3 = new List<GameObject>();
    public List<GameObject> activePool_EnemyType3 = new List<GameObject>();

    public List<GameObject> inactivePool_Boss = new List<GameObject>();
    public List<GameObject> activePool_Boss = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < maxEnemyCountPerType; i++)
        {
            GameObject clone = Instantiate(EnemyType1Prefab, poolLocation, Quaternion.identity, this.transform);
            inactivePool_EnemyType1.Add(clone);
            clone.GetComponent<Poolable>().ResetPoolableObject();
        }

        for (int i = 0; i < maxEnemyCountPerType; i++)
        {
            GameObject clone = Instantiate(EnemyType2Prefab, poolLocation, Quaternion.identity, this.transform);
            inactivePool_EnemyType2.Add(clone);
            clone.GetComponent<Poolable>().ResetPoolableObject();
        }

        for (int i = 0; i < maxEnemyCountPerType; i++)
        {
            GameObject clone = Instantiate(EnemyType3Prefab, poolLocation, Quaternion.identity, this.transform);
            inactivePool_EnemyType3.Add(clone);
            clone.GetComponent<Poolable>().ResetPoolableObject();
        }

        GameObject bossClone = Instantiate(bossPrefab, poolLocation, Quaternion.identity, this.transform);
        inactivePool_Boss.Add(bossClone);
        bossClone.GetComponent<Poolable>().ResetPoolableObject();
    }

    
    public GameObject GetObject(EnemyType type, Transform spawnPoint)
    {
        GameObject clone = null;

        switch (type)
        {
            case EnemyType.None:
                break;

            case EnemyType.EnemyType1:

                clone = inactivePool_EnemyType1[0];
                activePool_EnemyType1.Add(clone);
                inactivePool_EnemyType1.Remove(clone);
                break;

            case EnemyType.EnemyType2:

                clone = inactivePool_EnemyType2[0];
                activePool_EnemyType2.Add(clone);
                inactivePool_EnemyType2.Remove(clone);
                break;

            case EnemyType.EnemyType3:

                clone = inactivePool_EnemyType3[0];
                activePool_EnemyType3.Add(clone);
                inactivePool_EnemyType3.Remove(clone);
                break;

            case EnemyType.Boss:

                clone = inactivePool_Boss[0];
                activePool_EnemyType1.Add(clone);
                inactivePool_EnemyType1.Remove(clone);
                break;
        }

        clone.GetComponent<Poolable>().UseObject(spawnPoint.position);
        clone.transform.parent = spawnPoint;
        clone.SetActive(true);

        return clone;
    }

    public void ResetObjectPools()
    {
        int index = activePool_EnemyType1.Count;
        for (int i = 0; i < index; i++)
        {
            
            GameObject clone = activePool_EnemyType1[0];
            clone.GetComponent<Poolable>().ResetPoolableObject();
            clone.transform.parent = this.transform;

            inactivePool_EnemyType1.Add(clone);
            activePool_EnemyType1.Remove(clone);
      
        }

        index = activePool_EnemyType2.Count;
        for (int i = 0; i < index; i++)
        {

            GameObject clone = activePool_EnemyType2[0];
            clone.GetComponent<Poolable>().ResetPoolableObject();
            clone.transform.parent = this.transform;

            inactivePool_EnemyType2.Add(clone);
            activePool_EnemyType2.Remove(clone);

        }

        index = activePool_EnemyType3.Count;
        for (int i = 0; i < index; i++)
        {

            GameObject clone = activePool_EnemyType3[0];
            clone.GetComponent<Poolable>().ResetPoolableObject();
            clone.transform.parent = this.transform;

            inactivePool_EnemyType3.Add(clone);
            activePool_EnemyType3.Remove(clone);

        }

        index = activePool_Boss.Count;
        for (int i = 0; i < index; i++)
        {

            GameObject clone = activePool_Boss[0];
            clone.GetComponent<Poolable>().ResetPoolableObject();
            clone.transform.parent = this.transform;

            inactivePool_Boss.Add(clone);
            activePool_Boss.Remove(clone);

        }



    }
    

}
