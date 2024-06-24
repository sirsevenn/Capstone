using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "ScriptableObjects/HO/EnemyData")]
public class HO_EnemyDataSO : ScriptableObject
{
    [Header("General Enemy Properties")]
    [SerializeField] private EEnemyType enemyType;
    [SerializeField] private GameObject model;
    [SerializeField] private string attackAnimTrigger;
    [SerializeField] private string deathAnimTrigger;

    [Space(10)] [Header("Enemy Stats")]
    [SerializeField] private int enemyHP;
    [SerializeField] private int enemyATK;
    [SerializeField] private int enemyDEF;
    [SerializeField] private EElementalAttackType weakToElement;
    [SerializeField] private EElementalAttackType resistantToElement;


    public EEnemyType EnemyType
    { 
        get { return enemyType; } 
        private set { } 
    }

    public GameObject Model
    { 
        get { return model; } 
        private set { }
    }

    public string AttackAnimTrigger
    {
        get { return attackAnimTrigger; }
        private set { }
    }

    public string DeathAnimTrigger
    {
        get { return deathAnimTrigger; }
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

    public EElementalAttackType WeakToElement
    {
        get { return weakToElement; }
        private set { }
    }

    public EElementalAttackType ResistantToElement
    {
        get { return resistantToElement; }
        private set { }
    }

    public string GetEnemyName()
    {
        return enemyType.ToString().Replace("_", " ");
    }
}