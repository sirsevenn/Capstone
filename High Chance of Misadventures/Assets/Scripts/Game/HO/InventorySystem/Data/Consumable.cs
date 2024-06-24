using System;
using UnityEngine;

[Serializable]
public class Consumable
{
    [Header("Consumable Properties")]
    [SerializeField] protected uint consumableID;
    [SerializeField] private ConsumableSO consumableData;


    public Consumable(uint id, ConsumableSO consumableData)
    {
        this.consumableID = id;
        this.consumableData = consumableData;
    }

    public uint ItemID
    {
        get { return consumableID; }
        private set { consumableID = value; }
    }

    public ConsumableSO ConsumableData
    {
        get { return consumableData; }
        private set { consumableData = value; }
    }

    public static int ConvertEffectIntoValue(ECraftingEffect effectType)
    {
        switch (effectType)
        {
            case ECraftingEffect.Worst_Effect:
                return -20;

            case ECraftingEffect.Bad_Effect:
                return -10;

            case ECraftingEffect.No_Effect:
                return 0;

            case ECraftingEffect.Good_Effect:
                return 10;

            case ECraftingEffect.Great_Effect:
                return 20;
        }

        return 0;
    }
}
