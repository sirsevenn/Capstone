using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    None,
    Goblin,
    Minotaur,
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
    public GameObject goblinPrefab;
    public GameObject minotaurPrefab;
    public GameObject bossPrefab;

    [Space(20)]

    public List<GameObject> inactivePool_Goblin = new List<GameObject>();
    public List<GameObject> activePool_Goblin = new List<GameObject>();

    public List<GameObject> inactivePool_Minotaur = new List<GameObject>();
    public List<GameObject> activePool_Minotaur = new List<GameObject>();

    public List<GameObject> inactivePool_Boss = new List<GameObject>();
    public List<GameObject> activePool_Boss = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < maxEnemyCountPerType; i++)
        {
            GameObject clone = Instantiate(goblinPrefab, poolLocation, Quaternion.identity, this.transform);
            inactivePool_Goblin.Add(clone);
            clone.GetComponent<Poolable>().ResetPoolableObject();
        }

        for (int i = 0; i < maxEnemyCountPerType; i++)
        {
            GameObject clone = Instantiate(minotaurPrefab, poolLocation, Quaternion.identity, this.transform);
            inactivePool_Minotaur.Add(clone);
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

            case EnemyType.Goblin:

                clone = inactivePool_Goblin[0];
                activePool_Goblin.Add(clone);
                inactivePool_Goblin.Remove(clone);
                break;

            case EnemyType.Minotaur:

                clone = inactivePool_Minotaur[0];
                activePool_Minotaur.Add(clone);
                inactivePool_Minotaur.Remove(clone);
                break;

            case EnemyType.Boss:

                clone = inactivePool_Boss[0];
                activePool_Goblin.Add(clone);
                inactivePool_Goblin.Remove(clone);
                break;
        }

        clone.GetComponent<Poolable>().UseObject(spawnPoint.position);
        clone.transform.parent = spawnPoint;
        clone.SetActive(true);

        return clone;
    }

    public void ResetObjectPools()
    {
        int index = activePool_Goblin.Count;
        for (int i = 0; i < index; i++)
        {
            
            GameObject clone = activePool_Goblin[0];
            clone.GetComponent<Poolable>().ResetPoolableObject();
            clone.transform.parent = this.transform;

            inactivePool_Goblin.Add(clone);
            activePool_Goblin.Remove(clone);
      
        }

        index = activePool_Minotaur.Count;
        for (int i = 0; i < index; i++)
        {

            GameObject clone = activePool_Minotaur[0];
            clone.GetComponent<Poolable>().ResetPoolableObject();
            clone.transform.parent = this.transform;

            inactivePool_Minotaur.Add(clone);
            activePool_Minotaur.Remove(clone);

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
