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

    public override string GetItemName()
    {
        return "Tier " + tierLevel.ToString() + " " + potionType.ToString().Replace('_', ' ');
    }
}
