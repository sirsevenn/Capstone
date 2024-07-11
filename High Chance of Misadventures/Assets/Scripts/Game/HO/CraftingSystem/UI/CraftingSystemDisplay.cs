using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystemDisplay : MonoBehaviour
{
    [Header("Crafting Slot Properties")]
    [SerializeField] private int currentHighlightedSlotIndex; 
    [SerializeField] private List<Image> dropSlotHighlights;
    [SerializeField] private List<Image> dropSlotIcons;
    [SerializeField] private Sprite defaultSlotIcon;
    [SerializeField] private GameObject blockSelectionPanel;

    [Space(10)] [Header("Dragged Material Properties")]
    [SerializeField] private Image draggedIcon;
    [SerializeField] private RectTransform draggedIconTransform;

    [Space(10)] [Header("Crafting Material Properties")]
    [SerializeField] private Transform materialsListParent;
    [SerializeField] private List<CraftingItemPanelScript> materialPanelsList;
    [SerializeField] private GameObject itemPanelPrefab;

    [Space(10)] [Header("Reminder Properties")]
    [SerializeField] private bool isReminderOpen;
    [SerializeField] private float reminderScaleDuration;
    [SerializeField] private GameObject reminderSpeechBubble;
    [SerializeField] private GameObject reminderAlertIcon;
    [SerializeField] private TMP_Text reminderText;

    [Space(10)] [Header("Camera Settings")]
    [SerializeField] private Camera mainCam;
    [SerializeField] private float defaultScreenRatio;
    [SerializeField] private float minmaxRatioDiff;
    [SerializeField] private float changeInFOV;

    [Space(10)] [Header("Other UI")]
    [SerializeField] private GameObject craftingCanvas;
    [SerializeField] private Button brewButton;
    [SerializeField] private Button deliverButton;


    #region Initialization
    private void Start()
    {
        ResetCraftingUI();
        InitializeItemPanels();
        AdjustCameraFOV();
        draggedIcon.gameObject.SetActive(false);

        UpdateReminderText();
        isReminderOpen = false;
        reminderAlertIcon.SetActive(true);
        reminderSpeechBubble.transform.localScale = Vector3.zero;

        craftingCanvas.SetActive(true);
    }

    private void InitializeItemPanels()
    {
        materialPanelsList = new List<CraftingItemPanelScript>();
        foreach (var material in InventorySystem.Instance.GetCraftingMaterialsList())
        {
            GameObject newMaterialPanel = GameObject.Instantiate(itemPanelPrefab, materialsListParent);
            CraftingItemPanelScript script = newMaterialPanel.GetComponent<CraftingItemPanelScript>();
            script.UpdatePanelInfo(material);
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

    #region Crafting Methods
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

    public void DisplayUsedMaterials(List<CraftingMaterialSO> usedMaterials)
    {
        ResetCraftingUI();
        blockSelectionPanel.SetActive(true);

        for (int i = 0;i < usedMaterials.Count;i++)
        {
            dropSlotIcons[i].sprite = usedMaterials[i].ItemIcon;
        }
    }

    public void EnableBlockSelectionPanel()
    {
        blockSelectionPanel.SetActive(true);
    }

    public void ResetCraftingUI()
    {
        currentHighlightedSlotIndex = -1;
        blockSelectionPanel.SetActive(false);

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

    #region Reminder Methods
    private void UpdateReminderText()
    {
        HO_LevelSO currentLevel = HO_GameManager.Instance.GetCurrentLevel();
        reminderText.text = currentLevel.ReminderText;
    }

    public void OnReminderClick()
    {
        isReminderOpen = !isReminderOpen;

        if (isReminderOpen)
        {
            reminderSpeechBubble.transform.DOScale(1, reminderScaleDuration);
            reminderAlertIcon.SetActive(false);
        }
        else
        {
            reminderSpeechBubble.transform.DOScale(0, reminderScaleDuration);
            reminderAlertIcon.SetActive(true);
        }
    }
    #endregion

    public void OnEnableInputs(bool isEnabled)
    {
        brewButton.interactable = isEnabled;
        deliverButton.interactable = isEnabled;
    }

    public void DisableUI()
    {
        craftingCanvas.SetActive(false);
    }
}
