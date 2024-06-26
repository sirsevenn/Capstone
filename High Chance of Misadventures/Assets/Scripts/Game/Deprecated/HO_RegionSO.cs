using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Region", menuName = "ScriptableObjects/Deprecated/Region")]
public class HO_RegionSO : ScriptableObject
{
    [Header("Region Properties")]
    [SerializeField] private string regionName;

    [Space(10)]
    [SerializeField] private List<HO_EnemyDataSO> enemiesList;

    [Space(10)]
    [SerializeField] private HO_EnemyDataSO bossEnemy;
    [SerializeField] private GameObject environmentModel;


    public string RegionName
    {
        get { return regionName; }
        private set { }
    }

    public List<HO_EnemyDataSO> EnemiesList
    {
        get { return enemiesList; }
        private set { }
    }

    public HO_EnemyDataSO BossEnemy
    {
        get { return bossEnemy; }
        private set { }
    }

    public GameObject EnvironmentModel
    {
        get { return environmentModel; }
        private set { }
    }
}
