using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Armor Properties")]
    [SerializeField] private ArmorSO helmet;
    [SerializeField] private ArmorSO chestplate;
    [SerializeField] private ArmorSO leggings;
    [SerializeField] private uint totalDEF;

    [Space(10)]
    [Header("Potion Properties")]
    [SerializeField] private List<Potion> potionsList;
    [SerializeField] private List<EPotionType> craftedPotionsCatalogue;

    [Space(10)]
    [Header("Crafting Material Properties")]
    [SerializeField] private List<CraftingMaterial> craftingMaterialsList;


    #region Singleton
    public static InventorySystem Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance != null && Instance == this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion


    public bool IsCraftableDiscovered(CraftableSO craftable)
    {
        if (craftable is ArmorSO)
        {
            ArmorSO armor = (ArmorSO)craftable;
            return IsArmorPieceDiscovered(armor.ArmorType, armor.ArmorLevel);
        }
        else if (craftable is PotionSO)
        {
            PotionSO potion = (PotionSO)craftable;
            return IsPotionDiscovered(potion.PotionType);
        }
        else
        {
            return false;
        }
    }

    #region Armor Methods
    public ArmorSO GetHelmet()
    {
        return helmet;
    }

    public ArmorSO GetChestplate()
    {
        return chestplate;
    }

    public ArmorSO GetLeggings()
    {
        return leggings;
    }

    public void UpgradeArmorPiece(ArmorSO newArmor)
    {
        if (newArmor.ArmorType == EArmorType.Helmet && newArmor.ArmorLevel > this.helmet.ArmorLevel)
        {
            this.helmet = newArmor;
            UpdateTotalDEF();
        }
        else if (newArmor.ArmorType == EArmorType.Chestplate && newArmor.ArmorLevel > this.chestplate.ArmorLevel)
        {
            this.chestplate = newArmor;
            UpdateTotalDEF();
        }
        else if (newArmor.ArmorType == EArmorType.Leggings && newArmor.ArmorLevel > this.leggings.ArmorLevel)
        {
            this.leggings = newArmor;
            UpdateTotalDEF();
        }
    }

    private bool IsArmorPieceDiscovered(EArmorType type, uint level)
    {
        switch (type)
        {
            case EArmorType.Helmet:
                if (level <= helmet.ArmorLevel) return true;
                break;

            case EArmorType.Chestplate:
                if (level <= chestplate.ArmorLevel) return true;
                break;

            case EArmorType.Leggings:
                if (level <= leggings.ArmorLevel) return true;
                break;

            default:
                break;
        }

        return false;
    }

    public uint GetTotalDEF()
    {
        return totalDEF;
    }

    private void UpdateTotalDEF()
    {
        totalDEF = helmet.DEF + chestplate.DEF + leggings.DEF;
    }
    #endregion


    #region Potion Methods
    public bool HasPotion(EPotionType potion)
    {
        return potionsList.Exists(x => x.PotionData.PotionType == potion);
    }

    public uint GetPotionAmount(EPotionType potion)
    {
        Potion selectedPotion = potionsList.Find(x => x.PotionData.PotionType == potion);
        return selectedPotion.Amount;
    }

    public Potion GetPotion(EPotionType potion)
    {
        return potionsList.Find(x => x.PotionData.PotionType == potion);
    }

    public List<Potion> GetPotionsList()
    {
        return potionsList;
    }

    public void AddPotions(Potion potions)
    {
        int index = potionsList.FindIndex(x => x.PotionData.PotionType == potions.PotionData.PotionType);

        if (index != -1)
        {
            potionsList[index].Amount += potions.Amount;
        }
        else
        {
            potionsList.Add(potions);
        }
    }

    public void UsePotion(EPotionType potion)
    {
        int index = potionsList.FindIndex(x => x.PotionData.PotionType == potion);

        if (index != -1)
        {
            // use method

            potionsList[index].Amount--;
            if (potionsList[index].Amount == 0)
            {
                potionsList.RemoveAt(index);
            }
        }
    }

    private bool IsPotionDiscovered(EPotionType type)
    {
        return craftedPotionsCatalogue.Contains(type);
    }

    public void AddPotionToCatalogue(EPotionType potion)
    {
        if (potion == EPotionType.Unknown || IsPotionDiscovered(potion)) return;

        craftedPotionsCatalogue.Add(potion);
    }
    #endregion


    #region Crafting Materials Methods
    public bool HasMaterial(ECraftingMaterialType material)
    {
        return craftingMaterialsList.Exists(x => x.MaterialData.MaterialType == material);
    }

    public uint GetMaterialAmount(ECraftingMaterialType material)
    {
        CraftingMaterial selectedMaterial = craftingMaterialsList.Find(x => x.MaterialData.MaterialType == material);
        return selectedMaterial.Amount;
    }

    public CraftingMaterial GetCraftingMaterial(ECraftingMaterialType material)
    {
        return craftingMaterialsList.Find(x => x.MaterialData.MaterialType == material);
    }

    public List<CraftingMaterial> GetCraftingMaterialsList()
    {
        return craftingMaterialsList;
    }

    public void AddMaterials(CraftingMaterial materials)
    {
        int index = craftingMaterialsList.FindIndex(x => x.MaterialData.MaterialType == materials.MaterialData.MaterialType);

        if (index != -1)
        {
            craftingMaterialsList[index].Amount += materials.Amount;
        }
        else
        {
            craftingMaterialsList.Add(materials);
        }
    }

    public void ReduceMaterials(CraftingMaterial materials)
    {
        int index = craftingMaterialsList.FindIndex(x => x.MaterialData.MaterialType == materials.MaterialData.MaterialType);

        if (index != -1 && craftingMaterialsList[index].Amount >= materials.Amount)
        {
            craftingMaterialsList[index].Amount -= materials.Amount;

            if (craftingMaterialsList[index].Amount == 0) 
            {
                craftingMaterialsList.RemoveAt(index);
            }
        }
    }
    #endregion
}
