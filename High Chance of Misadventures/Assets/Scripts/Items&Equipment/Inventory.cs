using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
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

    [SerializeField] private List<Armor> armorList = new();
    [SerializeField] private List<Weapon> weaponsList = new();
    [SerializeField] private List<Item> itemsList = new();
    [SerializeField] private int playerGold = 0;

    #region Inventory-Getters
    public List<Armor> GetArmorList() => armorList;

    public List<Weapon> GetWeaponsList() => weaponsList;

    public List<Item> GetItemsList() => itemsList;
    #endregion

    #region Armor
    public void AddArmor(ArmorDataSO armorData, int armorLevel = 1, int bonusDEF = 0)
    {
        Armor newArmor = new Armor(armorData, armorLevel, bonusDEF);
        armorList.Add(newArmor);
    }

    public void AddArmor(Armor newArmor)
    {
        armorList.Add(newArmor);
    }

    public Armor GetArmor(string armorName)
    {
        return armorList.Find(x => x.GetArmorData().GetArmorName() == armorName);
    }
    #endregion

    #region Weapons
    public void AddWeapon(WeaponDataSO weaponData, int weaponLevel = 1, int bonusATK = 0, TempSpecialSkillDataSO specialSkillData = null)
    {
        Weapon newWeapon = new Weapon(weaponData, weaponLevel, bonusATK, specialSkillData);
        weaponsList.Add(newWeapon);
    }

    public void AddWeapon(Weapon newWeapon)
    {
        weaponsList.Add(newWeapon);
    }

    public Weapon GetWeapon(string weaponName)
    {
        return weaponsList.Find(x => x.GetWeaponData().GetWeaponName() == weaponName);
    }
    #endregion

    #region Items
    public void AddItem(ItemDataSO itemData, int quantity = 1)
    {
        Item newItem = new Item(itemData, quantity);
        itemsList.Add(newItem);
    }

    public void AddItem(Item newItem)
    {
        itemsList.Add(newItem);
    }

    public Item GetItem(string itemName)
    {
        return itemsList.Find(x => x.GetItemData().GetItemName() == itemName);
    }

    public Item GetItem(EItemTypes itemType)
    {
        return itemsList.Find(x => x.GetItemData().GetItemType() == itemType);
    }

    public int GetItemQuantity(EItemTypes itemType)
    {
        Item selectedItem = GetItem(itemType);
        return selectedItem.GetItemQuantity();
    }
    #endregion

    #region Gold
    public int GetPlayerGold()
    {
        return playerGold;
    }

    public void UpdatePlayerGold(int amount)
    {
        playerGold += amount;
    }

    public bool HasEnoughPlayerGold(int amount)
    {
        return playerGold >= amount;
    }
    #endregion
}
