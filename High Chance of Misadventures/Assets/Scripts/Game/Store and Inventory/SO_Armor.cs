using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "Shop/Armor")]
public class SO_Armor : ScriptableObject
{
    public string armorName = "New Armor";
    public Sprite sprite;
    public int defenseValue = 0;
    public int upgradeCost = 0;
}