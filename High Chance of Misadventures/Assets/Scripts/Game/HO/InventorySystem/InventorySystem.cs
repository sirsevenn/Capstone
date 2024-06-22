using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Crafting Material Properties")]
    [SerializeField] private List<CraftingMaterial> craftingMaterialsList;

    [Space(10)] [Header("Craftables Properties")]
    [SerializeField] private List<Consumable> consumablesList;
    [SerializeField] private List<string> consumablesCatalogue;


    public event Action<CraftingMaterial, bool> OnUpdateMaterialsEvent;
    public event Action<Consumable, bool> OnUpdateConsumablesEvent;


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


    public bool IsCraftableDiscovered(ConsumableSO consumable)
    {
        return consumablesCatalogue.Contains(consumable.GetItemName());
    }

    //public void RemoveAllBadItems()
    //{
    //    for (int i = 0; i < potionsList.Count; i++)
    //    {
    //        var potion = potionsList[i];

    //        if (potion.AppliedModifierType == ECraftingEffect.Bad_Effect || potion.AppliedModifierType == ECraftingEffect.Poor_Effect)
    //        {
    //            potionsList.Remove(potion);
    //            OnUpdatePotionsEvent?.Invoke(potion, false);
    //            i--;
    //        }
    //    }
    //}


    #region Consumable Methods
    public bool HasConsumable(EConsumableType type)
    {
        return consumablesList.Exists(x => x.ConsumableData.ConsumableType == type);
    }

    public int GetConsumableAmount(EConsumableType type)
    {
        int numConsumables = 0;
        foreach (var consumable in consumablesList)
        {
            if (consumable.ConsumableData.ConsumableType == type) numConsumables++;
        }
        return numConsumables;
    }

    public Consumable GetOneConsumableOfType(EConsumableType type)
    {
        foreach (var consumable in consumablesList)
        {
            if (consumable.ConsumableData.ConsumableType == type) return consumable;
        }
        return null;
    }

    public List<Consumable> GetConsumablesOfType(EConsumableType type)
    {
        List<Consumable> returnList = new();
        foreach (var consumable in consumablesList)
        {
            if (consumable.ConsumableData.ConsumableType == type) returnList.Add(consumable);
        }
        return returnList;
    }

    public Consumable GetConsumableByID(uint id)
    {
        return consumablesList.Find(x => x.ItemID == id);
    }

    public List<Consumable> GetConsumablesList()
    {
        return consumablesList;
    }

    public int GetNextConsumableID()
    {
        int id = 1;
        foreach (var consumable in consumablesList)
        {
            if (id != consumable.ItemID) break;
            else id++;
        }
        return id;
    }

    public void AddConsumable(Consumable consumable)
    {
        if (consumable.ConsumableData.ConsumableType == EConsumableType.Unknown) return;

        consumablesList.Add(consumable);
        consumablesList.Sort((x,y) => x.ItemID.CompareTo(y.ItemID));
        OnUpdateConsumablesEvent?.Invoke(consumable, true);
    }

    public void ConsumeConsumable(uint id)
    {
        Consumable consumable = consumablesList.Find(x => x.ItemID == id);
        consumablesList.Remove(consumable);
        consumablesList.Sort((x, y) => x.ItemID.CompareTo(y.ItemID));
        OnUpdateConsumablesEvent?.Invoke(consumable, false);
    }

    public void ConsumeConsumable(uint id, HO_CharacterStat stat)
    {
        Consumable consumable = consumablesList.Find(x => x.ItemID == id);

        //switch (potion.PotionData.PotionType)
        //{
        //    case EPotionType.Health_Potion: stat.Heal(potion.FinalValue); break;
        //    case EPotionType.Attack_Potion: stat.ModifyATK(potion.FinalValue); break;
        //    case EPotionType.Defense_Potion: stat.ModifyDEF(potion.FinalValue); break;
        //    default: break;
        //}

        consumablesList.Remove(consumable);
        consumablesList.Sort((x, y) => x.ItemID.CompareTo(y.ItemID));
        OnUpdateConsumablesEvent?.Invoke(consumable, false);
    }

    public void AddConsumableToCatalogue(string consumableName)
    {
        if (consumablesCatalogue.Contains(consumableName)) return;
        consumablesCatalogue.Add(consumableName);
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
        if (materials.MaterialData.MaterialType == ECraftingMaterialType.Unknown) return;

        int index = craftingMaterialsList.FindIndex(x => x.MaterialData.MaterialType == materials.MaterialData.MaterialType);

        if (index != -1)
        {
            craftingMaterialsList[index].Amount += materials.Amount;
            OnUpdateMaterialsEvent?.Invoke(craftingMaterialsList[index], true);
        }
        else
        {
            craftingMaterialsList.Add(materials);
            OnUpdateMaterialsEvent?.Invoke(materials, true);
        }
    }

    public void ReduceMaterials(CraftingMaterial materials)
    {
        if (materials.MaterialData.MaterialType == ECraftingMaterialType.Unknown) return;

        int index = craftingMaterialsList.FindIndex(x => x.MaterialData.MaterialType == materials.MaterialData.MaterialType);

        if (index != -1 && craftingMaterialsList[index].Amount >= materials.Amount)
        {
            craftingMaterialsList[index].Amount -= materials.Amount;
            OnUpdateMaterialsEvent?.Invoke(craftingMaterialsList[index], false);

            if (craftingMaterialsList[index].Amount == 0) 
            {
                craftingMaterialsList.RemoveAt(index);
            }
        }
    }
    #endregion
}
