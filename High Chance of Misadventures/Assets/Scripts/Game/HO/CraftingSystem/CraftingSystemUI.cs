using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystemUI : MonoBehaviour
{
    [Header("Crafting UI References")]
    [SerializeField] private int currentHighlightedSlot;
    [SerializeField] private List<Image> materialSlotImages;
    [SerializeField] private List<Image> materialSlotHighlights;

    [Space(10)]
    [SerializeField] private GameObject outputPanel;
    [SerializeField] private Image possibleOutputImage;

    [Space(10)]
    [SerializeField] private RectTransform successRateRedPortion;
    [SerializeField] private RectTransform successRateGreenPortion;

    [Space(10)]
    [SerializeField] private GameObject craftingEffectPanel;
    [SerializeField] private RectTransform craftingEffectTransform;
    [SerializeField] private RectTransform craftingEffectRedPortion;
    [SerializeField] private RectTransform craftingEffectGreenPortion;

    [Space(10)] [Header("Dragged Material Properties")]
    [SerializeField] private Image draggedIcon;
    [SerializeField] private RectTransform draggedIconTransform;

    [Space(10)] [Header("UI Default Values")]
    [SerializeField] private Color defaultSlotColor;
    [SerializeField] private Sprite defaultOutputIcon;

    [Space(10)] [Header("Prefabs")]
    [SerializeField] private GameObject itemPanelPrefab;

    [Header("Parent References")]
    [SerializeField] private GameObject craftablesListTab;
    [SerializeField] private GameObject materialsListTab;
    [SerializeField] private Transform craftablesListParent;
    [SerializeField] private Transform materialsListParent;

    [Space(10)] [Header("Item Panel References")]
    [SerializeField] private List<CraftingItemPanelScript> armorPanelsList;
    [SerializeField] private List<CraftingItemPanelScript> potionPanelsList;
    [SerializeField] private List<CraftingItemPanelScript> scrollPanelsList;
    [SerializeField] private List<CraftingItemPanelScript> materialPanelsList;


    #region Initialization
    private void Start()
    {
        ResetCraftingUI();
        InitializeItemPanels();
        EnableMaterialsListPanel();
        draggedIcon.gameObject.SetActive(false);

        InventorySystem.Instance.OnUpgradeArmorEvent += UpdateArmorPanel;
        InventorySystem.Instance.OnUpdatePotionsEvent += AddNewPotionPanel;
        InventorySystem.Instance.OnUpdateScrollsEvent += AddNewScrollPanel;
        InventorySystem.Instance.OnUpdateMaterialsEvent += UpdateMaterialPanel;
    }

    private void OnDestroy()
    {
        InventorySystem.Instance.OnUpgradeArmorEvent -= UpdateArmorPanel;
        InventorySystem.Instance.OnUpdatePotionsEvent -= AddNewPotionPanel;
        InventorySystem.Instance.OnUpdateScrollsEvent -= AddNewScrollPanel;
        InventorySystem.Instance.OnUpdateMaterialsEvent -= UpdateMaterialPanel;
    }

    private void InitializeItemPanels()
    {
        // Armor
        armorPanelsList = new List<CraftingItemPanelScript>();

        List<ArmorSO> armorList = new List<ArmorSO>()
        {
            InventorySystem.Instance.GetHelmet(),
            InventorySystem.Instance.GetChestplate(),
            InventorySystem.Instance.GetLeggings()
        };

        foreach (var armor in armorList)
        {
            GameObject newArmorPanel = GameObject.Instantiate(itemPanelPrefab, craftablesListParent);
            CraftingItemPanelScript script = newArmorPanel.GetComponent<CraftingItemPanelScript>();
            script.UpdatePanelInfo(armor);
            armorPanelsList.Add(script);
        }

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
    }

    public void EnableMaterialsListPanel()
    {
        craftablesListTab.gameObject.SetActive(false);
        materialsListTab.gameObject.SetActive(true);
    }
    #endregion

    #region Public UI Methods
    public void OnBeginDragMaterial(Sprite icon)
    {
        draggedIcon.gameObject.SetActive(true);
        draggedIcon.sprite = icon;
    }

    public void OnDragMaterial(Vector2 pos)
    {
        draggedIconTransform.position = pos;

        if (currentHighlightedSlot != -1)
        {
            materialSlotHighlights[currentHighlightedSlot].enabled = false;
        }

        int index = IsDroppedMaterialOnBaseSlot(pos) ? 0 : GetSupplementarySlotIndexFromHoveredMaterial(pos);
        if (index != -1)
        {
            materialSlotHighlights[index].enabled = true;
            currentHighlightedSlot = index;
        }
    }

    public void OnEndDragMaterial(int index)
    {
        if (index != -1)
        {
            materialSlotImages[index].sprite = draggedIcon.sprite;
            materialSlotImages[index].color = Color.white;
        }

        if (currentHighlightedSlot != -1)
        {
            materialSlotHighlights[currentHighlightedSlot].enabled = false;
        }

        currentHighlightedSlot = -1;
        draggedIcon.gameObject.SetActive(false);
        draggedIcon.sprite = null;
    }

    public bool IsDroppedMaterialOnBaseSlot(Vector2 pos)
    {
        RectTransform slotTransform = materialSlotHighlights[0].GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(slotTransform, pos);
    }

    public int GetSupplementarySlotIndexFromHoveredMaterial(Vector2 pos)
    {
        for (int i = 1; i < materialSlotHighlights.Count; i++)
        {
            RectTransform slotTransform = materialSlotHighlights[i].GetComponent<RectTransform>();

            if (RectTransformUtility.RectangleContainsScreenPoint(slotTransform, pos)) return i;
        }

        return -1;
    }

    public void ResetCraftingUI()
    {
        foreach (var highlight in materialSlotHighlights)
        {
            highlight.enabled = false;
        }

        foreach (var slot in materialSlotImages)
        {
            slot.sprite = null;
            slot.color = defaultSlotColor;
        }

        currentHighlightedSlot = -1;

        possibleOutputImage.sprite = defaultOutputIcon;
        outputPanel.gameObject.SetActive(false);

        SetSuccessRateBar(0f);
        SetCraftingEffectBar(EEffectModifier.Unknown, true);
    }

    public void SetIconOnOutputImage(Sprite outputIcon)
    {
        possibleOutputImage.sprite = (outputIcon == null) ? defaultOutputIcon : outputIcon;
        outputPanel.gameObject.SetActive(true);
    }

    public void SetSuccessRateBar(float successRate)
    {
        float height = successRateRedPortion.rect.height;
        successRateGreenPortion.sizeDelta = new Vector2(successRateGreenPortion.rect.width, height * -(1 - successRate));
    }

    public void SetCraftingEffectBar(EEffectModifier effect, bool disablePanel = false)
    {
        craftingEffectPanel.gameObject.SetActive(!disablePanel);
        craftingEffectRedPortion.sizeDelta = new Vector2(-craftingEffectTransform.rect.width, craftingEffectRedPortion.rect.height);
        craftingEffectGreenPortion.sizeDelta = new Vector2(-craftingEffectTransform.rect.width, craftingEffectRedPortion.rect.height);

        if (disablePanel) return;

        switch (effect)
        {
            case EEffectModifier.Bad_Effect:
                craftingEffectRedPortion.sizeDelta = new Vector2(-craftingEffectTransform.rect.width / 2f, craftingEffectRedPortion.rect.height);
                break;

            case EEffectModifier.No_Effect:
                craftingEffectRedPortion.sizeDelta = new Vector2(-craftingEffectTransform.rect.width * 3 / 4f, craftingEffectRedPortion.rect.height);
                break;

            case EEffectModifier.Good_Effect:
                craftingEffectGreenPortion.sizeDelta = new Vector2(-craftingEffectTransform.rect.width * 3 / 4f, craftingEffectGreenPortion.rect.height);
                break;

            case EEffectModifier.Strong_Effect:
                craftingEffectGreenPortion.sizeDelta = new Vector2(-craftingEffectTransform.rect.width / 2f, craftingEffectGreenPortion.rect.height);
                break;

            default:
                break;
        }
    }
    #endregion

    #region Update Item Panels Methods
    private void UpdateArmorPanel(ArmorSO newArmor) 
    {
        if (newArmor == null) return;

        foreach (var armorPanel in armorPanelsList)
        {
            if (armorPanel.IsTheSameArmorInPanel(newArmor))
            {
                armorPanel.UpdatePanelInfo(newArmor);
                return;
            }
        }
    }

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
