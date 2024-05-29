using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "ScriptableObjects/Armor")]
public class ArmorSO : CraftableSO
{
    [Space(10)] [Header("Armor Properties")]
    [SerializeField] private ArmorType armorType;
    [SerializeField] private string armorName;
    [SerializeField] private uint armorLevel;   
    [SerializeField] private uint defValue;


    public ArmorType ArmorType
    { 
        get { return armorType; } 
        private set { armorType = value; } 
    }

    public uint ArmorLevel
    {
        get { return armorLevel; }
        private set { armorLevel = value; }
    }

    public uint DEF
    {
        get { return defValue; }
        private set { defValue = value; }
    }

    public override string GetCraftableName()
    {
        return armorName;
    }
}
