using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stat", menuName = "ScriptableObjects/HO/EnemyStat")]
public class HO_EnemyDataSO : ScriptableObject
{
    [SerializeField] private EEnemyType enemyType;
    [SerializeField] private int enemyHP;
    [SerializeField] private int enemyATK;
    [SerializeField] private int enemyDEF;


    public EEnemyType EnemyType
    { 
        get { return enemyType; } 
        private set { } 
    }

    public int EnemyHP
    { 
        get { return enemyHP; } 
        private set { } 
    }

    public int EnemyATK
    {
        get { return enemyATK; }
        private set { }
    }

    public int EnemyDEF
    {
        get { return enemyDEF; }
        private set { }
    }

    public string GetEnemyName()
    {
        return enemyType.ToString().Replace("_", " ");
    }
}