using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "ScriptableObjects/HO/Level")]
public class HO_LevelSO : ScriptableObject
{
    [Header("General Properties")]
    [SerializeField] private uint levelID;
    [TextArea(4, 10)]
    [SerializeField] private List<string> speechList;
    [SerializeField] private GameObject environment;

    [Space(10)] [Header("Player Properties")] 
    [SerializeField] private HO_CharacterStat playerStats;
    [SerializeField] private List<CraftingMaterialSO> materialsList;

    [Space(10)] [Header("Enemy Properties")] 
    [SerializeField] private HO_CharacterStat enemyStats;
    [SerializeField] private HO_EnemyDataSO enemyData;


    public uint LevelID
    {
        get { return levelID; }
        private set { levelID = value; }
    }

    public List<string> SpeechList
    {
        get { return speechList; }
        private set { }
    }

    public GameObject Environment
    {
        get { return environment; }
        private set { environment = value; }
    }

    public HO_CharacterStat PlayerStats
    {
        get { return playerStats; }
        private set { playerStats = value; }
    }

    public HO_CharacterStat EnemyStats
    {
        get { return enemyStats; }
        private set { enemyStats = value; }
    }

    public HO_EnemyDataSO EnemyData
    {
        get { return enemyData; }
        private set { enemyData = value; }
    }

    public void AddMaterialsToInventory()
    {
        foreach (var material in materialsList)
        {
            InventorySystem.Instance.AddMaterials(material);
        }
    }
}
