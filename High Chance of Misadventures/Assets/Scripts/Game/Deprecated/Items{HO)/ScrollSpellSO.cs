using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Scroll", menuName = "ScriptableObjects/HO/Scroll")]
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

    public int GetEffectValueFromModifier(ECraftingEffect modifier)
    {
        return effectModifiersList.Find(x => x.ModifierType == modifier).EffectValue;
    }

    public ECraftingEffect GetModifierFromEffectValue(int value)
    {
        value = Mathf.Clamp(value, GetEffectValueFromModifier(ECraftingEffect.Bad_Effect), GetEffectValueFromModifier(ECraftingEffect.Strong_Effect));

        if (value < GetEffectValueFromModifier(ECraftingEffect.Bad_Effect) + 0.5f)
        {
            return ECraftingEffect.Bad_Effect;
        }
        else if (value <= GetEffectValueFromModifier(ECraftingEffect.Poor_Effect))
        {
            return ECraftingEffect.Poor_Effect;
        }
        else if (value <= GetEffectValueFromModifier(ECraftingEffect.Good_Effect))
        {
            return ECraftingEffect.Good_Effect;
        }
        else if (value <= GetEffectValueFromModifier(ECraftingEffect.Strong_Effect))
        {
            return ECraftingEffect.Strong_Effect;
        }
        else
        {
            return ECraftingEffect.Unknown;
        }
    }

    public override string GetItemName()
    {
        return "Tier " + tierLevel.ToString() + " " + elementalAttackType.ToString() + " Scroll";
    }
}
