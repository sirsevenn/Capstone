using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Potion", menuName = "ScriptableObjects/HO/Potion")]
public class PotionSO : CraftableSO
{
    [Space(10)] [Header("Potion Properties")]
    [SerializeField] private EPotionType potionType;

    [TextArea(4, 10)]
    [SerializeField] private string potionDescription;

    [Space(10)]
    [SerializeField] List<EffectModifier> effectModifiersList;


    public EPotionType PotionType
    {  
        get { return potionType; } 
        private set { } 
    }

    public string PotionDescription
    { 
        get { return potionDescription; } 
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
        value = Mathf.Clamp(value, GetEffectValueFromModifier(ECraftingEffect.Worst_Effect), GetEffectValueFromModifier(ECraftingEffect.Great_Effect));

        if (value < GetEffectValueFromModifier(ECraftingEffect.Worst_Effect) + 0.5f)
        {
            return ECraftingEffect.Worst_Effect;
        }
        else if (value <= GetEffectValueFromModifier(ECraftingEffect.Bad_Effect))
        {
            return ECraftingEffect.Bad_Effect;
        }
        else if (value <= GetEffectValueFromModifier(ECraftingEffect.Good_Effect))
        {
            return ECraftingEffect.Good_Effect;
        }
        else if (value <= GetEffectValueFromModifier(ECraftingEffect.Great_Effect))
        {
            return ECraftingEffect.Great_Effect;
        }
        else
        {
            return ECraftingEffect.Unknown;
        }
    }

    public override string GetItemName()
    {
        return "Tier " + tierLevel.ToString() + " " + potionType.ToString().Replace('_', ' ');
    }
}
