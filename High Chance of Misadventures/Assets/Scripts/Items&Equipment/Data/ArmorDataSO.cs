using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "ScriptableObjects/Inventory/Armor")]
public class ArmorDataSO : ScriptableObject
{
    [SerializeField] private Sprite armorIcon;
    [SerializeField] private string armorName;
    [SerializeField] private int DEF;

    public Sprite GetArmorIcon() => armorIcon;

    public string GetArmorName() => armorName;

    public int GetDefenseValue() => DEF;
}