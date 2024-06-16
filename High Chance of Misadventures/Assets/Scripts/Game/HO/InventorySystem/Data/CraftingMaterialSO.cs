using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Material", menuName = "ScriptableObjects/HO/CraftingMaterial")]
public class CraftingMaterialSO : ItemSO
{
    [Space(10)] [Header("Crafting Material Properties")]
    [SerializeField] private ECraftingMaterialType materialType;

    [TextArea(4, 10)] 
    [SerializeField] private string materialDescription;

    [Space(10)]
    [SerializeField] private CraftableSO itemToCraft;

    [SerializeField] private uint baseProbabilityValue;
    [SerializeField] private uint supplementaryAmount;
    [SerializeField] private EEffectModifier craftingEffect;


    public ECraftingMaterialType MaterialType
    {
        get { return materialType; }
        private set { materialType = value; }
    }

    public string MaterialDescription
    { 
        get { return materialDescription; } 
        private set { materialDescription = value; }
    }

    public CraftableSO ItemToCraft
    {
        get { return itemToCraft; }
        private set { itemToCraft = value; }
    }

    public uint BaseProbabilityValue
    {
        get { return baseProbabilityValue; }
        private set { baseProbabilityValue = value; }
    }

    public uint SupplementaryAmount
    {
        get { return supplementaryAmount; }
        private set { supplementaryAmount = value; }
    }

    public EEffectModifier CraftingEffect
    {
        get { return craftingEffect; }
        private set { craftingEffect = value; }
    }

    public override string GetItemName()
    {
        return materialType.ToString().Replace('_', ' ');
    }
}