using System;
using UnityEngine;

[Serializable]
public class Armor
{
    [SerializeField] private ArmorDataSO armorData;
    [SerializeField] private int armorLevel;
    [SerializeField] private int bonusDEF;

    public Armor(ArmorDataSO armorData, int armorLevel, int bonusDEF)
    {
        this.armorData = armorData;
        this.armorLevel = armorLevel;
        this.bonusDEF = bonusDEF;
    }

    public ArmorDataSO GetArmorData() => armorData;

    public int GetArmorLevel() => armorLevel;

    public int GetBonusDEF() => bonusDEF;

    public int GetTotalDEF() => (armorData.GetDefenseValue() + bonusDEF);

    public void IncreaseArmorLevel(int increase = 1)
    {
        armorLevel += increase;
    }

    public void IncreaseBonusDEF(int increase)
    {
        bonusDEF += increase;
    }
}
