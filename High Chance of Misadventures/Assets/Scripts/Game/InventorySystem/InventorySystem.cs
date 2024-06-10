using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Armor Properties")]
    [SerializeField] private ArmorSO helmet;
    [SerializeField] private ArmorSO chestplate;
    [SerializeField] private ArmorSO leggings;
    [SerializeField] private int totalDEF;

    [Space(10)] [Header("Other Craftables Properties")]
    [SerializeField] private List<Potion> potionsList;
    [SerializeField] private List<ScrollSpell> scrollSpellsList;
    [SerializeField] private List<string> craftablesCatalogue;

    [Space(10)] [Header("Crafting Material Properties")]
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
            return IsArmorPieceDiscovered(armor.ArmorType, armor.TierLevel);
        }
        else if (craftable is PotionSO)
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
        if (newArmor.ArmorType == EArmorType.Helmet && newArmor.TierLevel > this.helmet.TierLevel)
        {
            this.helmet = newArmor;
            UpdateTotalDEF();
        }
        else if (newArmor.ArmorType == EArmorType.Chestplate && newArmor.TierLevel > this.chestplate.TierLevel)
        {
            this.chestplate = newArmor;
            UpdateTotalDEF();
        }
        else if (newArmor.ArmorType == EArmorType.Leggings && newArmor.TierLevel > this.leggings.TierLevel)
        {
            this.leggings = newArmor;
            UpdateTotalDEF();
        }
    }

    private bool IsArmorPieceDiscovered(EArmorType type, uint level)
    {
        if (type == EArmorType.Helmet && level <= this.helmet.TierLevel) return true;
        else if (type == EArmorType.Chestplate && level <= this.chestplate.TierLevel) return true;
        else if (type == EArmorType.Leggings && level <= this.leggings.TierLevel) return true;
        else return false;
    }

    public int GetTotalDEF()
    {
        return totalDEF;
    }

    private void UpdateTotalDEF()
    {
        totalDEF = helmet.BaseValue + chestplate.BaseValue + leggings.BaseValue;
    }
    #endregion


    #region Potion Methods
    public bool HasPotion(EPotionType potion)
    {
        return potionsList.Exists(x => x.PotionData.PotionType == potion);
    }

    public int GetPotionAmount(EPotionType potionType)
    {
        int numPotions = 0;
        foreach (var potionInList in potionsList)
        {
            if (potionInList.PotionData.PotionType == potionType) numPotions++;
        }
        return numPotions;
    }

    public List<Potion> GetPotionsOfType(EPotionType potionType)
    {
        List<Potion> returnList = new();
        foreach (var potionInList in potionsList)
        {
            if (potionInList.PotionData.PotionType == potionType) returnList.Add(potionInList);
        }
        return returnList;
    }

    public Potion GetPotionByID(uint id)
    {
        return potionsList.Find(x => x.PotionID == id);
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
            if (id != potion.PotionID) break;
            else id++;
        }
        return id;
    }

    public void AddPotion(Potion potion)
    {
        potionsList.Add(potion);
        potionsList.Sort((x,y) => x.PotionID.CompareTo(y.PotionID));
    }

    public void UsePotion(uint id)
    {
        Potion potion = potionsList.Find(x => x.PotionID == id);
        // use potion

        potionsList.Remove(potion);
        potionsList.Sort((x, y) => x.PotionID.CompareTo(y.PotionID));
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
        return scrollSpellsList.Find(x => x.ScrollID == id);
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
            if (id != scroll.ScrollID) break;
            else id++;
        }
        return id;
    }

    public void AddScroll(ScrollSpell scroll)
    {
        scrollSpellsList.Add(scroll);
        scrollSpellsList.Sort((x, y) => x.ScrollID.CompareTo(y.ScrollID));
    }

    public void UseScroll(uint id)
    {
        ScrollSpell scroll = scrollSpellsList.Find(x => x.ScrollID == id);
        // use scroll

        scrollSpellsList.Remove(scroll);
        scrollSpellsList.Sort((x, y) => x.ScrollID.CompareTo(y.ScrollID));
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
