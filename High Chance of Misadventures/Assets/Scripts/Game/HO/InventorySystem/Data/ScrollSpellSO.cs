using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scroll", menuName = "ScriptableObjects/HO/Scroll")]
public class ScrollSpellSO : CraftableSO
{
    [Space(10)] [Header("Armor Properties")]
    [SerializeField] private EElementalAttackType elementalAttackType;

    [TextArea(4, 10)]
    [SerializeField] private string scrollDescription;

    [Space(10)]
    [SerializeField] List<EffectModifier> effectModifiersList;


    public EElementalAttackType ElementalAttackType
    {
        get { return elementalAttackType; }
        private set { }
    }

    public string ScrollDescription
    {
        get { return scrollDescription; }
        private set { }
    }

    public List<EffectModifier> EffectModifiersList
    {
        get { return effectModifiersList; }
        private set { }
    }

    public int GetEffectValueFromModifier(EEffectModifier modifier)
    {
        return effectModifiersList.Find(x => x.ModifierType == modifier).EffectValue;
    }

    public EEffectModifier GetModifierFromEffectValue(int value)
    {
        value = Mathf.Clamp(value, GetEffectValueFromModifier(EEffectModifier.Bad_Effect), GetEffectValueFromModifier(EEffectModifier.Strong_Effect));

        if (value < GetEffectValueFromModifier(EEffectModifier.Bad_Effect) + 0.5f)
        {
            return EEffectModifier.Bad_Effect;
        }
        else if (value <= GetEffectValueFromModifier(EEffectModifier.No_Effect))
        {
            return EEffectModifier.No_Effect;
        }
        else if (value <= GetEffectValueFromModifier(EEffectModifier.Good_Effect))
        {
            return EEffectModifier.Good_Effect;
        }
        else if (value <= GetEffectValueFromModifier(EEffectModifier.Strong_Effect))
        {
            return EEffectModifier.Strong_Effect;
        }
        else
        {
            return EEffectModifier.Unknown;
        }
    }

    public override string GetItemName()
    {
        return "Tier " + tierLevel.ToString() + " " + elementalAttackType.ToString() + " Scroll";
    }
}
