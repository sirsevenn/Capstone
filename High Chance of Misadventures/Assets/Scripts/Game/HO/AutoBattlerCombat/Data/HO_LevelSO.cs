using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "ScriptableObjects/HO/Level")]
public class HO_LevelSO : ScriptableObject
{
    [Header("General Properties")]
    [SerializeField] private uint levelID;
    [SerializeField] private Sprite levelIcon;
    [SerializeField] private string regionName;

    [Space(20)] [Header("Cutscene Properties")]
    [TextArea(4, 10)]
    [SerializeField] private List<string> speechList;
    [TextArea(4, 10)]
    [SerializeField] private string reminderText;

    [Space(20)][Header("Player Properties")]
    [SerializeField] private HO_CharacterStat playerStats;
    [Space(10)]
    [SerializeField] private List<CraftingMaterialSO> materialsList;

    [Space(20)][Header("Enemy Properties")]
    [SerializeField] private HO_CharacterStat enemyStats;
    [Space(10)]
    [SerializeField] private HO_EnemyDataSO enemyData;
    [SerializeField] private bool enemyHasArmorPierce;


    public uint LevelID
    {
        get { return levelID; }
        private set { levelID = value; }
    }

    public Sprite LevelIcon
    {
        get { return levelIcon; }
        private set { levelIcon = value; }
    }

    public string RegionName
    {
        get { return regionName; }
        private set { regionName = value; }
    }

    public List<string> SpeechList
    {
        get { return speechList; }
        private set { }
    }

    public string ReminderText
    {
        get { return reminderText; }
        private set {  reminderText = value; }
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

    public bool EnemyHasArmorPierce
    {
        get { return enemyHasArmorPierce; }
        private set { enemyHasArmorPierce = value; }
    }

    public void AddMaterialsToInventory()
    {
        foreach (var material in materialsList)
        {
            InventorySystem.Instance.AddMaterials(material);
        }
    }
}
