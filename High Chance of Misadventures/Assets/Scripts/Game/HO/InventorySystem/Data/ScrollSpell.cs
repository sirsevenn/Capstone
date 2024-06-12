using System;
using UnityEngine;

[Serializable]
public class ScrollSpell : ConsumableItem
{
    [Space(10)]
    [SerializeField] private ScrollSpellSO scrollData;


    public ScrollSpell(uint id, EEffectModifier modifierType, ScrollSpellSO data) : base(id, modifierType)
    {
        this.scrollData = data;

        EffectModifier modifier = data.EffectModifiersList.Find(x => x.ModifierType == modifierType);
        this.finalValue = (modifier != null) ? data.BaseValue + modifier.EffectValue : data.BaseValue;
    }
    public ScrollSpellSO ScrollData
    {
        get { return scrollData; }
        private set { }
    }
}