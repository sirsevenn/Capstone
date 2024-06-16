using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Potion : ConsumableItem
{
    [Space(10)]
    [SerializeField] private PotionSO potionData;


    public Potion(uint id, int finalValue, PotionSO data) : base(id)
    {
        this.potionData = data;
        this.finalValue = Mathf.Clamp(finalValue, data.GetEffectValueFromModifier(EEffectModifier.Bad_Effect), data.GetEffectValueFromModifier(EEffectModifier.Strong_Effect));
        this.appliedModifierType = data.GetModifierFromEffectValue(this.finalValue);
    }

    public PotionSO PotionData
    {
        get { return potionData; }
        private set { }
    }
}