using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "ScriptableObjects/HO/Potion")]
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
        return "Tier " + tierLevel.ToString() + " " + potionType.ToString().Replace('_', ' ');
    }
}
