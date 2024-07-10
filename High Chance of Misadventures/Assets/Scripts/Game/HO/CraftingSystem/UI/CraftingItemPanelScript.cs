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
    [SerializeField] private List<CraftingEffectPanelScript> effectPanelsList;

    [Header("Item Properties")]
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private ItemDraggable draggableScript;

    public void UpdatePanelInfo(CraftingMaterialSO material)
    {
        itemImage.sprite = material.ItemIcon;
        //itemNumberText.text = "";
        itemNameText.text = material.GetItemName();
        itemDescriptionText.text = material.MaterialDescription;

        for (int i = 0; i < material.ConsumableWeightsList.Count; i++)
        {
            ECraftingEffect effect = material.ConsumableWeightsList[i].CraftingEffect;
            int spriteIndex = (int)effect - 1;

            switch (effect)
            {
                case ECraftingEffect.Worst_Effect:
                case ECraftingEffect.Bad_Effect:
                    effectPanelsList[i].SetNegativeEffectIcon(effectSpitesList[spriteIndex]);
                    break;
                case ECraftingEffect.Good_Effect:
                case ECraftingEffect.Great_Effect:
                    effectPanelsList[i].SetPositiveEffectIcon(effectSpitesList[spriteIndex]);
                    break;
                default:
                    break;
            }
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
