using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_Text goldText;

    [Header("ItemStore")]
    [SerializeField] private List<ItemHolder> itemHolders = new List<ItemHolder>();
    [SerializeField] private int pieceCost = 5;

    [Header("Page Settings")]
    [SerializeField] private GameObject armorPage;
    [SerializeField] private GameObject weaponPage;
    [SerializeField] private GameObject itemPage;

    [Header("Armor Settings")]
    [SerializeField] private ArmorManager armorManager;
    [Space(10)]
    [SerializeField] private SO_Armor currentHelmet;
    [SerializeField] private SO_Armor currentCuirass;
    [SerializeField] private SO_Armor currentGreaves;
    [Space(20)]
    [SerializeField] private Image HelmetImage;
    [SerializeField] private TMP_Text HelmetName;
    [SerializeField] private TMP_Text HelmetDefense;
    [SerializeField] private TMP_Text HelmetCost;
    [Space(10)]
    [SerializeField] private Image ArmorImage;
    [SerializeField] private TMP_Text ArmorName;
    [SerializeField] private TMP_Text ArmorDefense;
    [SerializeField] private TMP_Text ArmorCost;
    [Space(10)]
    [SerializeField] private Image LegsImage;
    [SerializeField] private TMP_Text LegsName;
    [SerializeField] private TMP_Text LegsDefense;
    [SerializeField] private TMP_Text LegsCost;






    #region PageManagement
    private void CloseAllPages()
    {
        armorPage.SetActive(false);
        weaponPage.SetActive(false);
        itemPage.SetActive(false);
    }

    public void OpenArmorPage()
    {
        CloseAllPages();
        armorPage.SetActive(true);
    }

    public void OpenWeaponPage()
    {
        CloseAllPages();
        weaponPage.SetActive(true);
    }

    public void OpenItemPage()
    {
        CloseAllPages();
        itemPage.SetActive(true);
    }
    #endregion

    private void Start()
    {
        InitializeItemPage();
        InitializeArmorPage();
    }

    public void ProceedToFight()
    {
        SceneLoader.ChangeScene(0);
    }

    private void InitializeItemPage()
    {
        goldText.text = InventoryManager.Instance.Gold.ToString();
        itemHolders[0].InitializeItemHolder(InventoryManager.Instance.RedPieces);
        itemHolders[1].InitializeItemHolder(InventoryManager.Instance.GreenPieces);
        itemHolders[2].InitializeItemHolder(InventoryManager.Instance.BluePieces);
        itemHolders[3].InitializeItemHolder(InventoryManager.Instance.HealthPotions);
    }

    public void InitializeArmorPage()
    {
        InitializeHelmetBar();
        InitializeCuirassBar();
        InitializeGreavesBar();
    }

    private void InitializeHelmetBar()
    {
        int helmetLevel = InventoryManager.Instance.HelmetLevel;
        currentHelmet = ScriptableObjectDatabase.Instance.helmetUpgrades[helmetLevel];
        HelmetImage.sprite = currentHelmet.sprite;
        HelmetName.text = currentHelmet.armorName;
        HelmetDefense.text = "Defense:" + currentHelmet.defenseValue.ToString();
        HelmetCost.text = currentHelmet.upgradeCost.ToString();
    }

    private void InitializeCuirassBar()
    {
        int armorLevel = InventoryManager.Instance.CuirassLevel;
        currentCuirass = ScriptableObjectDatabase.Instance.cuirassUpgrades[armorLevel];
        ArmorImage.sprite = currentCuirass.sprite;
        ArmorName.text = currentCuirass.armorName;
        ArmorDefense.text = "Defense:" + currentCuirass.defenseValue.ToString();
        ArmorCost.text = currentCuirass.upgradeCost.ToString();
    }

    private void InitializeGreavesBar()
    {
        int legsLevel = InventoryManager.Instance.GreavesLevel;
        currentGreaves = ScriptableObjectDatabase.Instance.greavesUpgrades[legsLevel];
        LegsImage.sprite = currentGreaves.sprite;
        LegsName.text = currentGreaves.armorName;
        LegsDefense.text = "Defense:" + currentGreaves.defenseValue.ToString();
        LegsCost.text = currentGreaves.upgradeCost.ToString();
    }

    public void BuyItem(int index)
    {

        switch (index)
        {
            case 0:
                if (!CanPurchase(pieceCost)) { return; }
                break;
            case 1:
                if (!CanPurchase(pieceCost)) { return; }
                break;
            case 2:
                if (!CanPurchase(pieceCost)) { return; }
                break;
            case 3:
                if (!CanPurchase(10)) { return; }
                break;
        }

        switch (index)
        {
            case 0:
                InventoryManager.Instance.AddItem(0);
                itemHolders[0].InitializeItemHolder(InventoryManager.Instance.RedPieces);
                break;
            case 1:
                InventoryManager.Instance.AddItem(1);
                itemHolders[1].InitializeItemHolder(InventoryManager.Instance.GreenPieces);
                break;
            case 2:
                InventoryManager.Instance.AddItem(2);
                itemHolders[2].InitializeItemHolder(InventoryManager.Instance.BluePieces);
                break;
            case 3:
                InventoryManager.Instance.AddItem(3);
                itemHolders[3].InitializeItemHolder(InventoryManager.Instance.HealthPotions);
                break;
        }

        switch (index)
        {
            case 0:
                DeductGold(pieceCost);
                break;
            case 1:
                DeductGold(pieceCost);
                break;
            case 2:
                DeductGold(pieceCost);
                break;
            case 3:
                DeductGold(10);
                break;
        }

        goldText.text = InventoryManager.Instance.Gold.ToString();

    }

    public void BuyArmor(int index)
    {
        switch (index)
        {
            case 0:
                if (!CanPurchase(currentHelmet.upgradeCost)) { return; }
                DeductGold(currentHelmet.upgradeCost);
                InventoryManager.Instance.UpgradeArmor(0);
                InitializeHelmetBar();
                armorManager.UpdateHelmet();
                break;
            case 1:
                if (!CanPurchase(currentCuirass.upgradeCost)) { return; }
                DeductGold(currentCuirass.upgradeCost);
                InventoryManager.Instance.UpgradeArmor(1);
                InitializeCuirassBar();
                armorManager.UpdateCuirass();
                break;
            case 2:
                if (!CanPurchase(currentGreaves.upgradeCost)) { return; }
                DeductGold(currentGreaves.upgradeCost);
                InventoryManager.Instance.UpgradeArmor(2);
                InitializeGreavesBar();
                armorManager.UpdateGreaves();
                break;
        }

        goldText.text = InventoryManager.Instance.Gold.ToString();
    }

  


    private bool CanPurchase(int cost)
    {
        if (cost > InventoryManager.Instance.Gold)
        {
            return false;
        }

        return true;
    }

    private void DeductGold(int value)
    {
        InventoryManager.Instance.DeductGold(value);
    }
}
