using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    None,
    Goblin,
    GoblinKing
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

    [Space(20)]

    public List<GameObject> inactivePool_Goblin = new List<GameObject>();
    public List<GameObject> activePool_Goblin = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < maxEnemyCountPerType; i++)
        {
            GameObject clone = Instantiate(goblinPrefab, poolLocation, Quaternion.identity, this.transform);
            inactivePool_Goblin.Add(clone);
            clone.GetComponent<Poolable>().ResetPoolableObject();
        }
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

            case EnemyType.GoblinKing:
                break;
        }

        clone.GetComponent<Poolable>().UseObject(spawnPoint.position);
        clone.transform.parent = spawnPoint;
        clone.SetActive(true);

        return clone;
    }

    public void RemoveObject(GameObject clone, EnemyType type)
    {

        switch (type)
        {
            case EnemyType.None:
                break;
            case EnemyType.Goblin:

                activePool_Goblin.Remove(clone);
                inactivePool_Goblin.Add(clone);

                break;
            case EnemyType.GoblinKing:
                break;
        }

        //reset clone

        clone.GetComponent<Poolable>().ResetPoolableObject();

    }
    

}
