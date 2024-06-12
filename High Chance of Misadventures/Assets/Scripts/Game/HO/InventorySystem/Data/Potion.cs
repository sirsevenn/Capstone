using System;
using UnityEngine;

[Serializable]
public class Potion : ConsumableItem
{
    [Space(10)]
    [SerializeField] private PotionSO potionData;


    public Potion(uint id, EEffectModifier modifierType, PotionSO data) : base(id, modifierType)
    {
        this.potionData = data;

        EffectModifier modifier = data.EffectModifiersList.Find(x => x.ModifierType == modifierType);
        this.finalValue = (modifier != null) ? data.BaseValue + modifier.EffectValue : data.BaseValue;
    }

    public PotionSO PotionData
    {
        get { return potionData; }
        private set { }
    }
}