using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Material", menuName = "ScriptableObjects/HO/CraftingMaterial")]
public class CraftingMaterialSO : ItemSO
{
    [Space(10)] [Header("Crafting Material Properties")]
    [SerializeField] private ECraftingMaterialType materialType;

    [TextArea(4, 10)] 
    [SerializeField] private string materialDescription;

    [Space(10)]
    [SerializeField] private List<ConsumableWeight> consumableWeightsList;

    [Space(20)]
    [SerializeField] private Color particleMaterialColor;


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

    public List<ConsumableWeight> ConsumableWeightsList
    {
        get { return consumableWeightsList; }
        private set { }
    }

    public Color ParticleMaterialColor
    { 
        get { return particleMaterialColor; } 
        private set { particleMaterialColor = value; }
    }

    public override string GetItemName()
    {
        return materialType.ToString().Replace('_', ' ');
    }
}