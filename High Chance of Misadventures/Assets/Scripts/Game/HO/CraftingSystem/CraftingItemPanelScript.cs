using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItemPanelScript : MonoBehaviour
{
    [Header("General UI Properties")]
    [SerializeField] private Image itemImage;
    [Tooltip("Level for the Armor, or Quantity for the Potions and Materials")]
    [SerializeField] private TMP_Text itemNumberText;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;

    [Space(10)]
    [Header("Weight Details Properties")]
    [SerializeField] private List<Sprite> effectSpitesList;
    [SerializeField] private List<Image> effectIconUIList;

    [Header("Item Properties")]
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private ItemDraggable draggableScript;

    public void UpdatePanelInfo(CraftingMaterialSO material, uint quantity)
    {
        itemImage.sprite = material.ItemIcon;
        itemNumberText.text = quantity.ToString();
        itemNameText.text = material.GetItemName();
        itemDescriptionText.text = material.MaterialDescription;

        for (int i = 0; i < effectIconUIList.Count; i++)
        {
            int index = (int)material.ConsumableWeightsList[i].CraftingEffect - 1;
            effectIconUIList[i].sprite = effectSpitesList[index];
        }

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
