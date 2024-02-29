using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    #region Singleton
    public static StoreManager Instance;

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
            InitializeStore();
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

    #region Fields
    [Header("Store Data")]
    [SerializeField] private List<Armor> armorList;
    [SerializeField] private List<Weapon> weaponsList;
    [SerializeField] private List<ItemDataSO> itemDataList;
    [SerializeField] private List<TempSpecialSkillDataSO> specialSkillsList;

    [Space(10)] [Header("Store Templates")]
    [SerializeField] private GameObject armorStoreTemplate;
    [SerializeField] private GameObject weaponStoreTemplate;
    [SerializeField] private GameObject itemStoreTemplate;
    [SerializeField] private GameObject itemStoreParent;

    [Space(10)] [Header("List of Store Objects")]
    [SerializeField] private List<GameObject> armorStoreObjectsList;
    [SerializeField] private List<GameObject> weaponStoreObjectsList;
    [SerializeField] private List<GameObject> itemStoreObjectsList;
    
    [Space(10)] [Header("Store Panels")]
    [SerializeField] private GameObject armorInfoPanel;
    [SerializeField] private GameObject weaponsInfoPanel;
    [SerializeField] private GameObject itemsInfoPanel;
    
    [Space(10)] [Header("Store Buttons")]
    [SerializeField] private Image armorButtonImage;
    [SerializeField] private Image weaponsButtonImage;
    [SerializeField] private Image itemsButtonImage;
    #endregion

    #region Initialization
    private void InitializeStore()
    {
        InitializeStoreData();
        InitializeArmorInfoPanel();
        InitializeWeaponInfoPanel();
        InitializeItemsInfoPanel();

        OnArmorButton();
    }

    private void InitializeStoreData()
    {
        armorList.Clear();
        armorList.AddRange(Inventory.Instance.GetArmorList());

        weaponsList.Clear();
        weaponsList.AddRange(Inventory.Instance.GetWeaponsList());
    }

    private void InitializeArmorInfoPanel()
    {
        foreach(var armorObj in armorStoreObjectsList)
        {
            DestroyImmediate(armorObj);
        }
        armorStoreObjectsList.Clear();
         

        foreach (var armor in armorList) 
        {
            GameObject newArmorObj = GameObject.Instantiate(armorStoreTemplate, armorInfoPanel.transform);
            armorStoreObjectsList.Add(newArmorObj);


            ArmorStoreScript armorScript = newArmorObj.GetComponent<ArmorStoreScript>();
            if (armorScript != null )
            {
                armorScript.SetupArmorStoreTemplate(armor);
            }
            else
            {
                Debug.LogError("Armor Script in ArmorStoreTemplate not set!!");
            }
        }
    }

    private void InitializeWeaponInfoPanel()
    {
        foreach(var weaponObj in weaponStoreObjectsList)
        {
            DestroyImmediate(weaponObj);
        }
        weaponStoreObjectsList.Clear();


        GameObject newWeaponObj = GameObject.Instantiate(weaponStoreTemplate, weaponsInfoPanel.transform);
        weaponStoreObjectsList.Add(newWeaponObj);


        WeaponStoreScript weaponScript = newWeaponObj.GetComponent<WeaponStoreScript>();
        if (weaponScript != null)
        {
            weaponScript.SetupWeaponStoreTemplate(weaponsList[0]);
        }
        else
        {
            Debug.LogError("Weapon Script in WeaponStoreTemplate not set!!");
        }
    }

    private void InitializeItemsInfoPanel()
    {
        foreach(var itemObj in itemStoreObjectsList)
        {
            DestroyImmediate(itemObj);
        }
        itemStoreObjectsList.Clear();


        foreach (var itemData in itemDataList)
        {
            GameObject newItemObj = GameObject.Instantiate(itemStoreTemplate, itemStoreParent.transform);
            itemStoreObjectsList.Add(newItemObj);


            ItemStoreScript itemScript = newItemObj.GetComponent<ItemStoreScript>();
            if (itemScript != null)
            {
                Item itemInInventory = Inventory.Instance.GetItem(itemData.GetItemType());
                if (itemInInventory != null)
                {
                    itemScript.SetupItemStoreTemplate(itemData, itemInInventory.GetItemQuantity());
                }
                else
                {
                    itemScript.SetupItemStoreTemplate(itemData, 0);
                }
            }
            else
            {
                Debug.LogError("Item Script in ItemStoreTemplate not set!!");
            }
        }
    }
    #endregion

    #region StorePanels
    public void OnArmorButton()
    {
        CloseAllStorePanels();
        armorInfoPanel.SetActive(true);

        //armorButtonImage.color = new Color(1, 1, 1, 1);
        armorButtonImage.color = new Color(0.3f, 0.3f, 0.3f, 1);
    }

    public void OnWeaponsButton()
    {
        CloseAllStorePanels();
        weaponsInfoPanel.SetActive(true);

        //weaponsButtonImage.color = new Color(1, 1, 1, 1);
        weaponsButtonImage.color = new Color(0.3f, 0.3f, 0.3f, 1);
    }

    public void OnItemsButton()
    {
        CloseAllStorePanels();
        itemsInfoPanel.SetActive(true);

        //itemsButtonImage.color = new Color(1, 1, 1, 1);
        itemsButtonImage.color = new Color(0.3f, 0.3f, 0.3f, 1);
    }

    private void CloseAllStorePanels()
    {
        armorInfoPanel.SetActive(false);
        weaponsInfoPanel.SetActive(false);
        itemsInfoPanel.SetActive(false);

        // UNCOMMENT ONCE BUTTON IMAGES ARE REPLACED
        //armorButtonImage.color = new Color(1, 1, 1, 0);
        //weaponsButtonImage.color = new Color(1, 1, 1, 0);
        //itemsButtonImage.color = new Color(1, 1, 1, 0);

        armorButtonImage.color = new Color(0.3f, 0.3f, 0.3f, 0);
        weaponsButtonImage.color = new Color(0.3f, 0.3f, 0.3f, 0);
        itemsButtonImage.color = new Color(0.3f, 0.3f, 0.3f, 0);
    }
    #endregion

    #region OtherUIInteractions
    public void OnUpgradeArmor(string armorName)
    {
        Armor selectedArmor = Inventory.Instance.GetArmor(armorName);
        int price = selectedArmor.GetArmorData().GetUpgradePrice();

        if (!Inventory.Instance.HasEnoughPlayerGold(price))
        {
            Debug.Log("Not enough money!");
            return;
        }

        selectedArmor.IncreaseArmorLevel();
        //selectedArmor.IncreaseBonusDEF();
        Inventory.Instance.UpdatePlayerGold(-price);
    }

    public void OnUpgradeWeapon(string weaponName)
    {
        Weapon selectedWeapon = Inventory.Instance.GetWeapon(weaponName);
        int price = selectedWeapon.GetWeaponData().GetUpgradePrice();

        if (!Inventory.Instance.HasEnoughPlayerGold(price))
        {
            Debug.Log("Not enough money!");
            return;
        }

        selectedWeapon.IncreaseWeaponLevel();
        //selectedWeapon.IncreaseBonusATK();
        Inventory.Instance.UpdatePlayerGold(-price);
    }

    public void OnSwitchSpecialSkill(string weaponName, int newSkillType, WeaponStoreScript weaponScript)
    {
        Weapon selectedWeapon = Inventory.Instance.GetWeapon(weaponName);
        TempSpecialSkillDataSO newSkill = specialSkillsList.Find(x => (int) x.GetSkillType() == newSkillType);

        selectedWeapon.SwitchSpecialSkill(newSkill);
        weaponScript.UpdateDescription(newSkill);
    }

    public void OnBuyItem(EItemTypes itemType, ItemStoreScript itemScript)
    {
        ItemDataSO itemData = itemDataList.Find(x => x.GetItemType() == itemType);
        int price = itemData.GetBuyPrice();

        if (!Inventory.Instance.HasEnoughPlayerGold(price))
        {
            Debug.Log("Not enough money!");
            return;
        }

        Item selectedItem = Inventory.Instance.GetItem(itemType);
        if (selectedItem != null)
        {
            selectedItem.UpdateQuantity(1);
        }
        else
        {
            selectedItem = new Item(itemData, 1);
            Inventory.Instance.AddItem(selectedItem);
        }

        itemScript.UpdateQuantity(selectedItem.GetItemQuantity());
        Inventory.Instance.UpdatePlayerGold(-price);
    }
    #endregion
}
