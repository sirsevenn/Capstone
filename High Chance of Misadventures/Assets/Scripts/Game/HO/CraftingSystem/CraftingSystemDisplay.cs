using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystemDisplay : MonoBehaviour
{
    [Header("Crafting Slot Properties")]
    [SerializeField] private int currentHighlightedSlotIndex; 
    [SerializeField] private List<Image> dropSlotHighlights;
    [SerializeField] private List<Image> dropSlotIcons;
    [SerializeField] private Sprite defaultSlotIcon;

    [Space(10)] [Header("Dragged Material Properties")]
    [SerializeField] private Image draggedIcon;
    [SerializeField] private RectTransform draggedIconTransform;

    [Space(10)] [Header("Other UI Properties")]
    [SerializeField] private Transform materialsListParent;
    [SerializeField] private List<CraftingItemPanelScript> materialPanelsList;
    [SerializeField] private GameObject itemPanelPrefab;

    [Space(10)] [Header("Camera Settings")]
    [SerializeField] private Camera mainCam;
    [SerializeField] private float defaultScreenRatio;
    [SerializeField] private float minmaxRatioDiff;
    [SerializeField] private float changeInFOV;


    #region Initialization
    private void Start()
    {
        ResetCraftingUI();
        InitializeItemPanels();
        AdjustCameraFOV();
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

    private void AdjustCameraFOV()
    {
        float currentScreenRatio = (float)Screen.width / (float)Screen.height;
        float diff = Mathf.Clamp(defaultScreenRatio - currentScreenRatio, -minmaxRatioDiff, minmaxRatioDiff);
        diff *= changeInFOV / minmaxRatioDiff;
        mainCam.fieldOfView += diff;
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
            slotIcon.sprite = defaultSlotIcon;
            slotIcon.color = Color.white;
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
