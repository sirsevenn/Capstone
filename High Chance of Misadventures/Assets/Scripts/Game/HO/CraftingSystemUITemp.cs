//using DG.Tweening;
//using System;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class CraftingSystemUITemp : MonoBehaviour
//{
//    [Header("Crafting UI References")]
//    [SerializeField] private List<Image> materialSlotImages;
//    [SerializeField] private List<Image> materialSlotHighlights;
//    [SerializeField] private Image possibleOutputImage;

//    [Space(10)] [Header("UI Default Values")]
//    [SerializeField] private Color defaultSlotColor;
//    [SerializeField] private Sprite defaultOutputIcon;

//    [Space(10)] [Header("Prefabs")]
//    [SerializeField] private GameObject itemPanelPrefab;

//    [Header("Parent References")]
//    [SerializeField] private GameObject craftablesListTab;
//    [SerializeField] private GameObject materialsListTab;
//    [SerializeField] private Transform craftablesListParent;
//    [SerializeField] private Transform materialsListParent;

//    [Space(10)] [Header("Item Panel References")]
//    [SerializeField] private List<ItemPanelScript> armorPanelsList;
//    [SerializeField] private List<ItemPanelScript> potionsPanelsList;
//    [SerializeField] private List<ItemPanelScript> materialsPanelsList;


//    #region Initialization
//    private void Start()
//    {
//        foreach (var highlight in materialSlotHighlights)
//        {
//            highlight.enabled = false;
//        }

//        ResetSelectedSlots();
//        InitializeItemPanels();
//        EnableMaterialsListPanel();

//        possibleOutputImage.gameObject.SetActive(false);
//    }

//    private void InitializeItemPanels()
//    {
//        // Armor
//        armorPanelsList = new List<ItemPanelScript>();

//        List<ArmorSO> armorList = new List<ArmorSO>()
//        {
//            InventorySystem.Instance.GetHelmet(),
//            InventorySystem.Instance.GetChestplate(),
//            InventorySystem.Instance.GetLeggings()
//        };

//        foreach (var armor in armorList)
//        {
//            GameObject newArmorPanel = GameObject.Instantiate(itemPanelPrefab, craftablesListParent);
//            ItemPanelScript script = newArmorPanel.GetComponent<ItemPanelScript>();
//            script.UpdatePanelInfo(armor);
//            armorPanelsList.Add(script);
//        }

//        // Potions
//        potionsPanelsList = new List<ItemPanelScript>();
//        foreach (var potion in InventorySystem.Instance.GetPotionsList())
//        {
//            GameObject newPotionPanel = GameObject.Instantiate(itemPanelPrefab, craftablesListParent);
//            ItemPanelScript script = newPotionPanel.GetComponent<ItemPanelScript>();
//            script.UpdatePanelInfo(potion.potionData, potion.FinalValue);
//            potionsPanelsList.Add(script);
//        }

//        // Materials
//        materialsPanelsList = new List<ItemPanelScript>();
//        foreach (var material in InventorySystem.Instance.GetCraftingMaterialsList())
//        {
//            GameObject newMaterialPanel = GameObject.Instantiate(itemPanelPrefab, materialsListParent);
//            ItemPanelScript script = newMaterialPanel.GetComponent<ItemPanelScript>();
//            script.UpdatePanelInfo(material.MaterialData, material.Amount);
//            materialsPanelsList.Add(script);
//        }
//    }
//    #endregion

//    #region Button-click Methods
//    public void EnableCraftablesListPanel()
//    {
//        craftablesListTab.gameObject.SetActive(true);
//        materialsListTab.gameObject.SetActive(false);
//    }

//    public void EnableMaterialsListPanel()
//    {
//        craftablesListTab.gameObject.SetActive(false);
//        materialsListTab.gameObject.SetActive(true);
//    }
//    #endregion

//    #region Public UI Methods
//    public void ResetSelectedSlots()
//    {
//        foreach (var slot in materialSlotImages)
//        {
//            slot.sprite = null;
//            slot.color = defaultSlotColor;
//        }
//    }

//    public void SetHighlightOnCraftingSlot(int index, bool enabled)
//    {
//        if (index < 0 || index >= materialSlotHighlights.Count) return;

//        materialSlotHighlights[index].enabled = enabled;
//    }

//    public void SetValuesOnSelectedSlot(Sprite materialIcon, int index)
//    {
//        if (index < 0 || index >= materialSlotImages.Count) return;

//        materialSlotImages[index].sprite = (materialIcon != null) ? materialIcon : null;
//        materialSlotImages[index].color = (materialIcon != null) ? Color.white : defaultSlotColor;
//    }

//    public void SetIconOnOutputImage(Sprite outputIcon, bool enabled, float successRate = 0f)
//    {
//        possibleOutputImage.sprite = (outputIcon == null) ? defaultOutputIcon : outputIcon;
//        possibleOutputImage.gameObject.SetActive(enabled);
//    }
//    #endregion

//    #region Update Item Panels Methods
//    public void UpdateArmorPanel(ArmorSO newArmor) // MAKE THIS EVENT BASED METHOD
//    {
//        foreach (var armorPanel in armorPanelsList)
//        {
//            if (armorPanel.IsTheSameItemInPanel(newArmor))
//            {
//                armorPanel.UpdatePanelInfo(newArmor);
//                return;
//            }
//        }
//    }

//    public void UpdatePotionPanel(Potion newPotion) // MAKE THIS EVENT BASED METHOD
//    {
//        foreach (var potionPanel in potionsPanelsList)
//        {
//            if (potionPanel.IsTheSameItemInPanel(newPotion.potionData))
//            {
//                potionPanel.UpdatePanelInfo(newPotion.potionData, newPotion.FinalValue); 
//                return;
//            }
//        }

//        GameObject newPotionPanel = GameObject.Instantiate(itemPanelPrefab, craftablesListParent);
//        ItemPanelScript script = newPotionPanel.GetComponent<ItemPanelScript>();
//        script.UpdatePanelInfo(newPotion.potionData, newPotion.FinalValue);
//        potionsPanelsList.Add(script);
//    }

//    public void UpdateMaterialPanel(CraftingMaterial newMaterial) // MAKE THIS EVENT BASED METHOD
//    {
//        foreach (var materialPanel in materialsPanelsList)
//        {
//            if (newMaterial.Amount > 0 && materialPanel.IsTheSameItemInPanel(newMaterial.MaterialData))
//            {
//                materialPanel.UpdatePanelInfo(newMaterial.MaterialData, newMaterial.Amount);
//                return;
//            }
//            else if (newMaterial.Amount == 0 && materialPanel.IsTheSameItemInPanel(newMaterial.MaterialData))
//            {
//                materialPanel.gameObject.SetActive(false);
//                return;
//            }
//        }
//    }
//    #endregion
//}
