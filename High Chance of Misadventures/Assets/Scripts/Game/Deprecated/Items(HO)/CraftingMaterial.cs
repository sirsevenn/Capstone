using System;
using UnityEngine;

[Serializable]
public class CraftingMaterial
{
    [SerializeField] private CraftingMaterialSO materialData;
    public uint Amount;


    public CraftingMaterial(CraftingMaterialSO data, uint amount)
    {
        this.materialData = data;
        this.Amount = amount;
    }

    public CraftingMaterialSO MaterialData
    {
        get { return materialData; }
        private set { materialData = value; }
    }
}