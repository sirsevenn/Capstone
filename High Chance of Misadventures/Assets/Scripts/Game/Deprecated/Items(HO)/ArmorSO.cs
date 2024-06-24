using UnityEngine;

//[CreateAssetMenu(fileName = "New Armor", menuName = "ScriptableObjects/HO/Armor")]
public class ArmorSO : CraftableSO
{
    [Space(10)] [Header("Armor Properties")]
    [SerializeField] private EArmorType armorType;
    [SerializeField] private string armorName;


    public EArmorType ArmorType
    { 
        get { return armorType; } 
        private set { } 
    }

    public override string GetItemName()
    {
        return armorName;
    }
}
