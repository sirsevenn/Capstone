using System;
using UnityEngine;

[Serializable]
public class Potion
{
    [SerializeField] private uint potionID;
    [SerializeField] private PotionSO potionData;
    [SerializeField] private EEffectModifier appliedModifierType;
    [SerializeField] private int finalValue;


    public Potion(uint id, PotionSO data, EEffectModifier modifierType)
    {
        this.potionID = id;
        this.potionData = data;
        this.appliedModifierType = modifierType;

        EffectModifier modifier = data.EffectModifiersList.Find(x => x.ModifierType == modifierType);
        this.finalValue = (modifier != null) ? data.BaseValue + modifier.EffectValue : data.BaseValue;
    }

    public uint PotionID
    {
        get { return potionID; }
        private set { }
    }

    public PotionSO PotionData
    {
        get { return potionData; }
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