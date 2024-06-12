using System;
using UnityEngine;

[Serializable]
public abstract class ConsumableItem
{
    [Header("Consumable Properties")]
    [SerializeField] protected uint itemID;
    [SerializeField] protected EEffectModifier appliedModifierType;
    [SerializeField] protected int finalValue;


    public ConsumableItem(uint id, EEffectModifier modifierType)
    {
        this.itemID = id;
        this.appliedModifierType = modifierType;
    }

    public uint ItemID
    {
        get { return itemID; }
        private set { }
    }

    public EEffectModifier AppliedModifierType
    {
        get { return appliedModifierType; }
        private set { }
    }

    public int FinalValue
    {
        get { return finalValue; }
        private set { }
    }
}
