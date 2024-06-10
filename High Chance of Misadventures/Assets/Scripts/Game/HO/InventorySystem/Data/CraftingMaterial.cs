using System;

[Serializable]
public class CraftingMaterial
{
    //public CraftingMaterialType MaterialType;
    public CraftingMaterialSO MaterialData;
    public uint Amount;


    public CraftingMaterial(CraftingMaterialSO data, uint amount)
    {
        this.MaterialData = data;
        this.Amount = amount;
    }
}