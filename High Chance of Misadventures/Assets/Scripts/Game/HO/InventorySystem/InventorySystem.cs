using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    //[Header("Armor Properties")]
    //[SerializeField] private ArmorSO helmet;
    //[SerializeField] private ArmorSO chestplate;
    //[SerializeField] private ArmorSO leggings;
    //[SerializeField] private int totalDEF;

    [Space(10)] [Header("Craftables Properties")]
    [SerializeField] private List<Potion> potionsList;
    [SerializeField] private List<ScrollSpell> scrollSpellsList;
    [SerializeField] private List<string> craftablesCatalogue;

    [Space(10)] [Header("Crafting Material Properties")]
    [SerializeField] private List<CraftingMaterial> craftingMaterialsList;

    //public event Action<ArmorSO> OnUpgradeArmorEvent;
    public event Action<Potion, bool> OnUpdatePotionsEvent;
    public event Action<ScrollSpell, bool> OnUpdateScrollsEvent;
    public event Action<CraftingMaterial, bool> OnUpdateMaterialsEvent;


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
        //if (craftable is ArmorSO)
        //{
        //    ArmorSO armor = (ArmorSO)craftable;
        //    return IsArmorPieceDiscovered(armor.ArmorType, armor.TierLevel);
        //}
        if (craftable is PotionSO)
        {
            PotionSO potion = (PotionSO)craftable;
            return craftablesCatalogue.Contains(potion.GetItemName());
        }
        else if (craftable is ScrollSpellSO)
        {
            ScrollSpellSO scroll = (ScrollSpellSO)craftable;
            return craftablesCatalogue.Contains(scroll.GetItemName());
        }
        else
        {
            return false;
        }
    }

    public void RemoveAllBadItems()
    {
        for (int i = 0; i < potionsList.Count; i++)
        {
            var potion = potionsList[i];

            if (potion.AppliedModifierType == EEffectModifier.Bad_Effect || potion.AppliedModifierType == EEffectModifier.No_Effect)
            {
                potionsList.Remove(potion);
                OnUpdatePotionsEvent?.Invoke(potion, false);
                i--;
            }
        }

        for (int i = 0; i < scrollSpellsList.Count; i++)
        {
            var scroll = scrollSpellsList[i];

            if (scroll.AppliedModifierType == EEffectModifier.Bad_Effect || scroll.AppliedModifierType == EEffectModifier.No_Effect)
            {
                scrollSpellsList.Remove(scroll);
                OnUpdateScrollsEvent?.Invoke(scroll, false);
                i--;
            }
        }
    }

    #region Armor Methods
    //public ArmorSO GetHelmet()
    //{
    //    return helmet;
    //}

    //public ArmorSO GetChestplate()
    //{
    //    return chestplate;
    //}

    //public ArmorSO GetLeggings()
    //{
    //    return leggings;
    //}

    //public void UpgradeArmorPiece(ArmorSO newArmor)
    //{
    //    if (newArmor.ArmorType == EArmorType.Helmet && newArmor.TierLevel > this.helmet.TierLevel)
    //    {
    //        this.helmet = newArmor;
    //        UpdateTotalDEF();
    //        OnUpgradeArmorEvent?.Invoke(newArmor);
    //    }
    //    else if (newArmor.ArmorType == EArmorType.Chestplate && newArmor.TierLevel > this.chestplate.TierLevel)
    //    {
    //        this.chestplate = newArmor;
    //        UpdateTotalDEF();
    //        OnUpgradeArmorEvent?.Invoke(newArmor);
    //    }
    //    else if (newArmor.ArmorType == EArmorType.Leggings && newArmor.TierLevel > this.leggings.TierLevel)
    //    {
    //        this.leggings = newArmor;
    //        UpdateTotalDEF();
    //        OnUpgradeArmorEvent?.Invoke(newArmor);
    //    }
    //}

    //private bool IsArmorPieceDiscovered(EArmorType type, uint level)
    //{
    //    if (type == EArmorType.Helmet && level <= this.helmet.TierLevel) return true;
    //    else if (type == EArmorType.Chestplate && level <= this.chestplate.TierLevel) return true;
    //    else if (type == EArmorType.Leggings && level <= this.leggings.TierLevel) return true;
    //    else return false;
    //}

    //public int GetTotalDEF()
    //{
    //    return totalDEF;
    //}

    //private void UpdateTotalDEF()
    //{
    //    totalDEF = helmet.BaseValue + chestplate.BaseValue + leggings.BaseValue;
    //}
    #endregion


    #region Potion Methods
    public bool HasPotion(EPotionType potion)
    {
        return potionsList.Exists(x => x.PotionData.PotionType == potion);
    }

    public int GetPotionAmount(EPotionType potionType)
    {
        int numPotions = 0;
        foreach (var potion in potionsList)
        {
            if (potion.PotionData.PotionType == potionType) numPotions++;
        }
        return numPotions;
    }

    public Potion GetOnePotionOfType(EPotionType potionType)
    {
        foreach (var potion in potionsList)
        {
            if (potion.PotionData.PotionType == potionType) return potion;
        }
        return null;
    }

    public List<Potion> GetPotionsOfType(EPotionType potionType)
    {
        List<Potion> returnList = new();
        foreach (var potion in potionsList)
        {
            if (potion.PotionData.PotionType == potionType) returnList.Add(potion);
        }
        return returnList;
    }

    public Potion GetPotionByID(uint id)
    {
        return potionsList.Find(x => x.ItemID == id);
    }

    public List<Potion> GetPotionsList()
    {
        return potionsList;
    }

    public int GetNextPotionID()
    {
        int id = 1;
        foreach (var potion in potionsList)
        {
            if (id != potion.ItemID) break;
            else id++;
        }
        return id;
    }

    public void AddPotion(Potion potion)
    {
        if (potion.PotionData.PotionType == EPotionType.Unknown) return;

        potionsList.Add(potion);
        potionsList.Sort((x,y) => x.ItemID.CompareTo(y.ItemID));
        OnUpdatePotionsEvent?.Invoke(potion, true);
    }

    public void UsePotion(uint id, HO_CharacterStat stat)
    {
        Potion potion = potionsList.Find(x => x.ItemID == id);

        switch (potion.PotionData.PotionType)
        {
            case EPotionType.Health_Potion: stat.Heal(potion.FinalValue); break;
            case EPotionType.Attack_Potion: stat.ModifyATK(potion.FinalValue); break;
            case EPotionType.Defense_Potion: stat.ModifyDEF(potion.FinalValue); break;
            default: break;
        }

        potionsList.Remove(potion);
        potionsList.Sort((x, y) => x.ItemID.CompareTo(y.ItemID));
        OnUpdatePotionsEvent?.Invoke(potion, false);
    }

    public void AddPotionToCatalogue(string potionName)
    {
        if (craftablesCatalogue.Contains(potionName)) return;
        craftablesCatalogue.Add(potionName);
    }
    #endregion


    #region Scroll Methods
    public bool HasScroll(EElementalAttackType attackType)
    {
        return scrollSpellsList.Exists(x => x.ScrollData.ElementalAttackType == attackType);
    }

    public int GetScrollAmount(EElementalAttackType attackType)
    {
        int numScrolls = 0;
        foreach (var scroll in scrollSpellsList)
        {
            if (scroll.ScrollData.ElementalAttackType == attackType) numScrolls++;
        }
        return numScrolls;
    }

    public ScrollSpell GetOneScrollOfType(EElementalAttackType attackType)
    {
        foreach (var scroll in scrollSpellsList)
        {
            if (scroll.ScrollData.ElementalAttackType == attackType) return scroll;
        }
        return null;
    }

    public List<ScrollSpell> GetScrollsOfType(EElementalAttackType attackType)
    {
        List<ScrollSpell> returnList = new();
        foreach (var scroll in scrollSpellsList)
        {
            if (scroll.ScrollData.ElementalAttackType == attackType) returnList.Add(scroll);
        }
        return returnList;
    }

    public ScrollSpell GetScrollByID(uint id)
    {
        return scrollSpellsList.Find(x => x.ItemID == id);
    }

    public List<ScrollSpell> GetScrollsList()
    {
        return scrollSpellsList;
    }

    public int GetNextScrollID()
    {
        int id = 1;
        foreach (var scroll in scrollSpellsList)
        {
            if (id != scroll.ItemID) break;
            else id++;
        }
        return id;
    }

    public void AddScroll(ScrollSpell scroll)
    {
        if (scroll.ScrollData.ElementalAttackType == EElementalAttackType.Unknown) return;

        scrollSpellsList.Add(scroll);
        scrollSpellsList.Sort((x, y) => x.ItemID.CompareTo(y.ItemID));
        OnUpdateScrollsEvent?.Invoke(scroll, true);
    }

    public void UseScroll(uint id)
    {
        ScrollSpell scroll = scrollSpellsList.Find(x => x.ItemID == id);
        scrollSpellsList.Remove(scroll);
        scrollSpellsList.Sort((x, y) => x.ItemID.CompareTo(y.ItemID));
        OnUpdateScrollsEvent?.Invoke(scroll, false);
    }

    public void AddScrollToCatalogue(string scrollName)
    {
        if (craftablesCatalogue.Contains(scrollName)) return;
        craftablesCatalogue.Add(scrollName);
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
