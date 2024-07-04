using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SideBarManager : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private GameObject sideBar;
    [SerializeField] private GameObject arrow;
    [SerializeField] private bool isOpen = false;

    [Header("Other Settings")]
    [SerializeField] private Transform closePos;
    [SerializeField] private Transform openPos;
    [SerializeField] private float travelDuration = 1;

    [Header("Inventory Settings")]
    [SerializeField] private TMP_Text redPieceText;
    [SerializeField] private TMP_Text greenPieceText;
    [SerializeField] private TMP_Text bluePieceText;
    [SerializeField] private TMP_Text healthPotionsText;

    [Header("Page Settings")]
    [SerializeField] private GameObject InventoryPage;
    [SerializeField] private GameObject GuidePage;
    [SerializeField] private GameObject SettingsPage;

    [Header("Page Icons")]
    [SerializeField] private Image inventoryIcon;
    [SerializeField] private Image guideIcon;
    [SerializeField] private Image settingsIcon;

    [Header("Color Settings")]
    [SerializeField] Color defaultColor;
    [SerializeField] Color highlightedColor;

    private void Start()
    {
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        redPieceText.text = InventoryManager.Instance.RedPieces.ToString();
        greenPieceText.text = InventoryManager.Instance.GreenPieces.ToString();
        bluePieceText.text = InventoryManager.Instance.BluePieces.ToString();
        healthPotionsText.text = InventoryManager.Instance.HealthPotions.ToString();
    }

    public void ToggleSideBar()
    {
        if (!isOpen)
        {
            sideBar.transform.DOMoveX(openPos.position.x, travelDuration, true).SetEase(Ease.InSine);
            arrow.transform.localScale = new Vector3(-1, 1, 1);
            isOpen = !isOpen;
        }
        else if (isOpen)
        {
            sideBar.transform.DOMoveX(closePos.position.x, travelDuration, true).SetEase(Ease.InSine);
            arrow.transform.localScale = new Vector3(1, 1, 1);
            isOpen = !isOpen;
        }
    }

    private void CloseAllPages()
    {
        inventoryIcon.color = defaultColor;
        guideIcon.color = defaultColor;
        settingsIcon.color = defaultColor;

        InventoryPage.SetActive(false);
        GuidePage.SetActive(false);
        SettingsPage.SetActive(false);
    }

    public void OpenInventory()
    {
        CloseAllPages();
        inventoryIcon.color = highlightedColor;
        InventoryPage.SetActive(true);
    }

    public void OpenGuide()
    {
        CloseAllPages();
        guideIcon.color = highlightedColor;
        GuidePage.SetActive(true);
    }

    public void OpenSettings()
    {
        CloseAllPages();
        settingsIcon.color = highlightedColor;
        SettingsPage.SetActive(true);
    }

    public void UsePotion()
    {
        if (InventoryManager.Instance.HealthPotions >= 1)
        {
            InventoryManager.Instance.DeductItem(3);
            LO_GameFlow_PVP.Instance.health.Heal(10);
            UpdateInventory();
        }
    }

}
