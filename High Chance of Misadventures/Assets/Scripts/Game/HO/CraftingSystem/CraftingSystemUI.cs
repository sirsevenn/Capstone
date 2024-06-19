using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystemUI : MonoBehaviour
{
    [Header("Crafting Slot Properties")]
    [SerializeField] private int currentHighlightedSlot; // -1 nothing, 0 base, 1 cauldron
    [SerializeField] private Image baseSlotImage;
    [SerializeField] private Image baseSlotHighlight;
    [SerializeField] private Image cauldronHighlight;

    [Space(10)] [Header("Crafting Output References")]
    [SerializeField] private CraftingOutputUI outputScript;

    [Space(10)] [Header("Description Book References")]
    [SerializeField] private CraftingBookUI bookScript;

    [Space(10)] [Header("Dragged Material Properties")]
    [SerializeField] private Image draggedIcon;
    [SerializeField] private RectTransform draggedIconTransform;

    [Space(10)] [Header("UI Default Values")]
    [SerializeField] private Color defaultSlotColor;
    [SerializeField] private Color defaultTabSelectedColor;
    [SerializeField] private Color defaultTabUnselectedColor;

    [Header("Tab UI References")]
    [SerializeField] private Image craftablesListButtonImage;
    [SerializeField] private Image materialsListButtonImage;
    [SerializeField] private GameObject craftablesListTab;
    [SerializeField] private GameObject materialsListTab;
    [SerializeField] private Transform craftablesListParent;
    [SerializeField] private Transform materialsListParent;

    [Space(10)] [Header("Item Panel References")]
    //[SerializeField] private List<CraftingItemPanelScript> armorPanelsList;
    [SerializeField] private List<CraftingItemPanelScript> potionPanelsList;
    [SerializeField] private List<CraftingItemPanelScript> scrollPanelsList;
    [SerializeField] private List<CraftingItemPanelScript> materialPanelsList;

    [Space(10)] [Header("Prefabs")]
    [SerializeField] private GameObject itemPanelPrefab;


    #region Initialization
    private void Start()
    {
        ResetCraftingUI();
        InitializeItemPanels();
        EnableMaterialsListPanel();
        draggedIcon.gameObject.SetActive(false);

        //InventorySystem.Instance.OnUpgradeArmorEvent += UpdateArmorPanel;
        InventorySystem.Instance.OnUpdatePotionsEvent += AddNewPotionPanel;
        InventorySystem.Instance.OnUpdateScrollsEvent += AddNewScrollPanel;
        InventorySystem.Instance.OnUpdateMaterialsEvent += UpdateMaterialPanel;
    }

    private void OnDestroy()
    {
        //InventorySystem.Instance.OnUpgradeArmorEvent -= UpdateArmorPanel;
        InventorySystem.Instance.OnUpdatePotionsEvent -= AddNewPotionPanel;
        InventorySystem.Instance.OnUpdateScrollsEvent -= AddNewScrollPanel;
        InventorySystem.Instance.OnUpdateMaterialsEvent -= UpdateMaterialPanel;
    }

    private void InitializeItemPanels()
    {
        // Armor
        //armorPanelsList = new List<CraftingItemPanelScript>();

        //List<ArmorSO> armorList = new List<ArmorSO>()
        //{
        //    InventorySystem.Instance.GetHelmet(),
        //    InventorySystem.Instance.GetChestplate(),
        //    InventorySystem.Instance.GetLeggings()
        //};

        //foreach (var armor in armorList)
        //{
        //    GameObject newArmorPanel = GameObject.Instantiate(itemPanelPrefab, craftablesListParent);
        //    CraftingItemPanelScript script = newArmorPanel.GetComponent<CraftingItemPanelScript>();
        //    script.UpdatePanelInfo(armor);
        //    armorPanelsList.Add(script);
        //}

        // Potions
        potionPanelsList = new List<CraftingItemPanelScript>();
        foreach (var potion in InventorySystem.Instance.GetPotionsList())
        {
            AddNewPotionPanel(potion, true);
        }

        // Scrolls
        scrollPanelsList = new List<CraftingItemPanelScript>();
        foreach (var scroll in InventorySystem.Instance.GetScrollsList())
        {
            AddNewScrollPanel(scroll, true);
        }

        // Materials
        materialPanelsList = new List<CraftingItemPanelScript>();
        foreach (var material in InventorySystem.Instance.GetCraftingMaterialsList())
        {
            GameObject newMaterialPanel = GameObject.Instantiate(itemPanelPrefab, materialsListParent);
            CraftingItemPanelScript script = newMaterialPanel.GetComponent<CraftingItemPanelScript>();
            script.UpdatePanelInfo(material.MaterialData, material.Amount);
            materialPanelsList.Add(script);
        }
    }
    #endregion

    #region Tab-switching Methods
    public void EnableCraftablesListPanel()
    {
        craftablesListTab.gameObject.SetActive(true);
        materialsListTab.gameObject.SetActive(false);

        craftablesListButtonImage.color = defaultTabSelectedColor;
        materialsListButtonImage.color = defaultTabUnselectedColor;
    }

    public void EnableMaterialsListPanel()
    {
        craftablesListTab.gameObject.SetActive(false);
        materialsListTab.gameObject.SetActive(true);

        craftablesListButtonImage.color = defaultTabUnselectedColor;
        materialsListButtonImage.color = defaultTabSelectedColor;
    }
    #endregion

    #region Dragging Event Methods
    public void OnBeginDragMaterial(Sprite icon)
    {
        draggedIcon.gameObject.SetActive(true);
        draggedIcon.sprite = icon;
    }

    public void OnDragMaterial(Vector2 pos)
    {
        draggedIconTransform.position = pos;

        currentHighlightedSlot = IsDroppedMaterialOnBaseSlot(pos) ? 0 : IsDroppedMaterialOnCauldron(pos) ? 1 : -1;
        baseSlotHighlight.enabled = IsDroppedMaterialOnBaseSlot(pos);
        cauldronHighlight.enabled = IsDroppedMaterialOnCauldron(pos);
    }

    public void OnEndDragMaterial()
    {
        if (currentHighlightedSlot == 0 && baseSlotImage.sprite == null)
        {
            baseSlotImage.sprite = draggedIcon.sprite;
            baseSlotImage.color = Color.white;
        }

        baseSlotHighlight.enabled = false;
        cauldronHighlight.enabled = false;
        currentHighlightedSlot = -1;

        draggedIcon.gameObject.SetActive(false);
        draggedIcon.sprite = null;
    }

    public bool IsDroppedMaterialOnBaseSlot(Vector2 pos)
    {
        RectTransform slotTransform = baseSlotHighlight.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(slotTransform, pos);
    }

    public bool IsDroppedMaterialOnCauldron(Vector2 pos)
    {
        RectTransform cauldronTransform = cauldronHighlight.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(cauldronTransform, pos) && !IsDroppedMaterialOnBaseSlot(pos);
    }
    #endregion

    #region Public UI Methods
    public void ResetCraftingUI()
    {
        baseSlotImage.sprite = null;
        baseSlotImage.color = defaultSlotColor;
        baseSlotHighlight.enabled = false;
        cauldronHighlight.enabled = false;
        currentHighlightedSlot = -1;

        outputScript.ResetCraftingUI();
        bookScript.ResetBook();
    }

    public void SetIconOnOutputImage(Sprite outputIcon) => outputScript.SetIconOnOutputImage(outputIcon);

    public void SetSuccessRateBar(float successRate) => outputScript.SetSuccessRateBar(successRate);

    public void SetCraftingEffectBar(EEffectModifier effect, bool disablePanel = false) => outputScript.SetCraftingEffectBar(effect, disablePanel);

    public void SetMaterialDataToBook(CraftingMaterialSO materialData) => bookScript.SetMaterialDataToBook(materialData);
    #endregion

    #region Update Item Panels Methods
    //private void UpdateArmorPanel(ArmorSO newArmor) 
    //{
    //    if (newArmor == null) return;

    //    foreach (var armorPanel in armorPanelsList)
    //    {
    //        if (armorPanel.IsTheSameArmorInPanel(newArmor))
    //        {
    //            armorPanel.UpdatePanelInfo(newArmor);
    //            return;
    //        }
    //    }
    //}

    private void AddNewPotionPanel(Potion newPotion, bool hasAddedNewPotion) 
    {
        if (!hasAddedNewPotion || newPotion == null) return;

        GameObject newPotionPanel = GameObject.Instantiate(itemPanelPrefab, craftablesListParent);
        CraftingItemPanelScript script = newPotionPanel.GetComponent<CraftingItemPanelScript>();
        script.UpdatePanelInfo(newPotion.PotionData, newPotion.FinalValue);
        potionPanelsList.Add(script);
    }

    private void AddNewScrollPanel(ScrollSpell newScroll, bool hasAddedNewScroll) 
    {
        if (!hasAddedNewScroll || newScroll == null) return;

        GameObject newPotionPanel = GameObject.Instantiate(itemPanelPrefab, craftablesListParent);
        CraftingItemPanelScript script = newPotionPanel.GetComponent<CraftingItemPanelScript>();
        script.UpdatePanelInfo(newScroll.ScrollData, newScroll.FinalValue);
        scrollPanelsList.Add(script);
    }

    private void UpdateMaterialPanel(CraftingMaterial newMaterial, bool hasAddedNewMaterial) 
    {
        if (newMaterial == null) return;

        foreach (var materialPanel in materialPanelsList)
        {
            if (newMaterial.Amount > 0 && materialPanel.IsTheSameMaterialInPanel(newMaterial.MaterialData))
            {
                materialPanel.UpdatePanelInfo(newMaterial.MaterialData, newMaterial.Amount);
                return;
            }
            else if (newMaterial.Amount == 0 && materialPanel.IsTheSameMaterialInPanel(newMaterial.MaterialData))
            {
                //materialPanel.gameObject.SetActive(false);
                DestroyImmediate(materialPanel.gameObject);
                return;
            }
        }
    }
    #endregion
}
