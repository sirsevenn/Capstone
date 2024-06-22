using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystemDisplay : MonoBehaviour
{
    [Header("Crafting Slot Properties")]
    [SerializeField] private int currentHighlightedSlotIndex; 
    [SerializeField] private List<Image> dropSlotHighlights;
    [SerializeField] private List<Image> dropSlotIcons;

    [Space(10)] [Header("Dragged Material Properties")]
    [SerializeField] private Image draggedIcon;
    [SerializeField] private RectTransform draggedIconTransform;

    [Space(10)] [Header("UI Default Values")]
    [SerializeField] private Color defaultSlotColor;

    [Header("Tab UI References")]
    [SerializeField] private Transform materialsListParent;

    [Space(10)] [Header("Item Panel References")]
    [SerializeField] private List<CraftingItemPanelScript> materialPanelsList;

    [Space(10)] [Header("Prefabs")]
    [SerializeField] private GameObject itemPanelPrefab;


    #region Initialization
    private void Start()
    {
        ResetCraftingUI();
        InitializeItemPanels();
        draggedIcon.gameObject.SetActive(false);

        InventorySystem.Instance.OnUpdateMaterialsEvent += UpdateMaterialPanel;
    }

    private void OnDestroy()
    {
        InventorySystem.Instance.OnUpdateMaterialsEvent -= UpdateMaterialPanel;
    }

    private void InitializeItemPanels()
    {
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

    #region Dragging Event Methods
    public void OnBeginDragMaterial(Sprite icon)
    {
        draggedIcon.gameObject.SetActive(true);
        draggedIcon.sprite = icon;
    }

    public void OnDragMaterial(Vector2 pos, int slotIndex, bool isHighlighted)
    {
        draggedIconTransform.position = pos;

        if (currentHighlightedSlotIndex != -1)
        {
            dropSlotHighlights[currentHighlightedSlotIndex].enabled = false;
        }

        currentHighlightedSlotIndex = slotIndex;

        if (slotIndex != -1)
        {
            dropSlotHighlights[slotIndex].enabled = isHighlighted;
        }
    }

    public void OnEndDragMaterial(bool isValidDropMaterial)
    {
        if (isValidDropMaterial)
        {
            dropSlotIcons[currentHighlightedSlotIndex].sprite = draggedIcon.sprite;
            dropSlotIcons[currentHighlightedSlotIndex].color = Color.white;
        }

        if (currentHighlightedSlotIndex != -1)
        {
            dropSlotHighlights[currentHighlightedSlotIndex].enabled = false;
        }

        currentHighlightedSlotIndex = -1;

        draggedIcon.gameObject.SetActive(false);
        draggedIcon.sprite = null;
    }

    public int GetSlotIndexFromHoveredMaterial(Vector2 pos)
    {
        for (int i = 0; i < dropSlotHighlights.Count; i++)
        {
            RectTransform rt = dropSlotHighlights[i].GetComponent<RectTransform>();

            if (RectTransformUtility.RectangleContainsScreenPoint(rt, pos)) return i;
        }

        return -1;
    }
    #endregion

    #region Public UI Methods
    public void ResetCraftingUI()
    {
        currentHighlightedSlotIndex = -1;

        foreach (var highlight in dropSlotHighlights)
        {
            highlight.enabled = false;
        }

        foreach (var slotIcon in dropSlotIcons)
        {
            slotIcon.sprite = null;
            slotIcon.color = defaultSlotColor;
        }
    }
    #endregion

    #region Inventory Event Methods
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
