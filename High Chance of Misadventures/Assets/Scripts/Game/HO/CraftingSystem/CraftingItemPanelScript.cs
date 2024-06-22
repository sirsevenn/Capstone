using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftingItemPanelScript : MonoBehaviour
{
    [Header("UI Properties")]
    [SerializeField] private Image itemImage;
    [Tooltip("Level for the Armor, or Quantity for the Potions and Materials")]
    [SerializeField] private TMP_Text itemNumberText;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;

    [Header("Item Properties")]
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private ItemDraggable draggableScript;

    public void UpdatePanelInfo(CraftingMaterialSO material, uint quantity)
    {
        itemImage.sprite = material.ItemIcon;
        itemNumberText.text = quantity.ToString();
        itemNameText.text = material.GetItemName();
        itemDescriptionText.text = material.MaterialDescription;

        itemSO = material;
        draggableScript.SetMaterial(material);
        draggableScript.enabled = true;
    }

    public bool IsTheSameMaterialInPanel(CraftingMaterialSO materialToCheck)
    {
        if (materialToCheck == null) return false;
        if (itemSO is not CraftingMaterialSO) return false;

        CraftingMaterialSO materialInPanel = (CraftingMaterialSO)itemSO;
        return (materialToCheck.MaterialType == materialInPanel.MaterialType);
    }
}
