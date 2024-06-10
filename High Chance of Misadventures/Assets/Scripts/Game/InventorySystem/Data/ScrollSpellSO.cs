using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scroll", menuName = "ScriptableObjects/Scroll")]
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

    public override string GetItemName()
    {
        return "Tier " + tierLevel.ToString() + " " + elementalAttackType.ToString() + " Scroll";
    }
}
