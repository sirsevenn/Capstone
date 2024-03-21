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
        InitializeStore();
    }

    public void ProceedToFight()
    {
        SceneLoader.ChangeScene(0);
    }

    private void InitializeStore()
    {
        goldText.text = InventoryManager.Instance.Gold.ToString();
        itemHolders[0].InitializeItemHolder(InventoryManager.Instance.RedPieces);
        itemHolders[1].InitializeItemHolder(InventoryManager.Instance.GreenPieces);
        itemHolders[2].InitializeItemHolder(InventoryManager.Instance.BluePieces);
        itemHolders[3].InitializeItemHolder(InventoryManager.Instance.HealthPotions);
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
