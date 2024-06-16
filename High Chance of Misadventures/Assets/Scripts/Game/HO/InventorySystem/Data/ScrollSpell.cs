using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScrollSpell : ConsumableItem
{
    [Space(10)]
    [SerializeField] private ScrollSpellSO scrollData;


    public ScrollSpell(uint id, int finalValue, ScrollSpellSO data) : base(id)
    {
        this.scrollData = data;
        this.finalValue = Mathf.Clamp(finalValue, data.GetEffectValueFromModifier(EEffectModifier.Bad_Effect), data.GetEffectValueFromModifier(EEffectModifier.Strong_Effect));
        this.appliedModifierType = data.GetModifierFromEffectValue(this.finalValue);
    }
    public ScrollSpellSO ScrollData
    {
        get { return scrollData; }
        private set { }
    }
}