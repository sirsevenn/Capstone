using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Crafting Material Properties")]
    [SerializeField] private List<CraftingMaterialSO> craftingMaterialsList;

    [Space(10)] [Header("Craftables Properties")]
    [SerializeField] private List<Consumable> consumablesList;


    public event Action<CraftingMaterialSO, bool> OnUpdateMaterialsEvent;
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


    #region Consumable Methods
    public bool HasConsumable(EConsumableType type)
    {
        return consumablesList.Exists(x => x.ConsumableData.ConsumableType == type);
    }

    public int GetConsumablesTotalAmount()
    {
        return consumablesList.Count;
    }

    public int GetConsumableAmountOfType(EConsumableType type)
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

    public Consumable GetOneSpellOfType(EElementalAttackType element)
    {
        foreach (var consumable in consumablesList)
        {
            ElementalSpellSO spell = consumable.ConsumableData as ElementalSpellSO;

            if (spell != null && spell.ElementalType == element) return consumable;
        }
        return null;
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

    public void ResetConsumables()
    {
        consumablesList.Clear();
    }
    #endregion


    #region Crafting Materials Methods
    public bool HasMaterial(ECraftingMaterialType material)
    {
        return craftingMaterialsList.Exists(x => x.MaterialType == material);
    }

    //public uint GetMaterialAmount(ECraftingMaterialType material)
    //{
    //    CraftingMaterial selectedMaterial = craftingMaterialsList.Find(x => x.MaterialData.MaterialType == material);
    //    return selectedMaterial.Amount;
    //}

    public CraftingMaterialSO GetCraftingMaterial(ECraftingMaterialType material)
    {
        return craftingMaterialsList.Find(x => x.MaterialType == material);
    }

    public List<CraftingMaterialSO> GetCraftingMaterialsList()
    {
        return craftingMaterialsList;
    }

    public void AddMaterials(CraftingMaterialSO material)
    {
        if (material.MaterialType == ECraftingMaterialType.Unknown) return;

        //int index = craftingMaterialsList.FindIndex(x => x.MaterialData.MaterialType == materials.MaterialData.MaterialType);

        //if (index != -1)
        //{
        //    craftingMaterialsList[index].Amount += materials.Amount;
        //    OnUpdateMaterialsEvent?.Invoke(craftingMaterialsList[index], true);
        //}
        //else
        //{
        //    craftingMaterialsList.Add(materials);
        //    OnUpdateMaterialsEvent?.Invoke(materials, true);
        //}

        if (!craftingMaterialsList.Exists(x => x.MaterialType == material.MaterialType))
        {
            craftingMaterialsList.Add(material);
            OnUpdateMaterialsEvent?.Invoke(material, true);
        }
    }

    public void ReduceMaterials(CraftingMaterialSO material)
    {
        if (material.MaterialType == ECraftingMaterialType.Unknown) return;

        //int index = craftingMaterialsList.FindIndex(x => x.MaterialData.MaterialType == materials.MaterialData.MaterialType);

        //if (index != -1 && craftingMaterialsList[index].Amount >= materials.Amount)
        //{
        //    craftingMaterialsList[index].Amount -= materials.Amount;
        //    OnUpdateMaterialsEvent?.Invoke(craftingMaterialsList[index], false);

        //    if (craftingMaterialsList[index].Amount == 0) 
        //    {
        //        craftingMaterialsList.RemoveAt(index);
        //    }
        //}

        if (craftingMaterialsList.Exists(x => x.MaterialType == material.MaterialType))
        {
            craftingMaterialsList.Remove(material);
            OnUpdateMaterialsEvent?.Invoke(material, false);
        }
    }
    #endregion

    public void ResetInventory()
    {
        consumablesList.Clear();
        craftingMaterialsList.Clear();
    }
}
