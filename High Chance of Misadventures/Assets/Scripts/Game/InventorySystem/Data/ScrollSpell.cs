using System;
using UnityEngine;

public class ScrollSpell : MonoBehaviour
{
    [SerializeField] private uint scrollID;
    [SerializeField] private ScrollSpellSO scrollData;
    [SerializeField] private EEffectModifier appliedModifierType;
    [SerializeField] private int finalValue;


    public ScrollSpell(uint id, ScrollSpellSO data, EEffectModifier modifierType)
    {
        this.scrollID = id;
        this.scrollData = data;
        this.appliedModifierType = modifierType;

        EffectModifier modifier = data.EffectModifiersList.Find(x => x.ModifierType == modifierType);
        this.finalValue = (modifier != null) ? data.BaseValue + modifier.EffectValue : data.BaseValue;
    }

    public uint ScrollID
    {
        get { return scrollID; }
        private set { }
    }

    public ScrollSpellSO ScrollData
    {
        get { return scrollData; }
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
