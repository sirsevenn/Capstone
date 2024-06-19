using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftingItemPanelScript : MonoBehaviour, IPointerClickHandler
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


    //public void UpdatePanelInfo(ArmorSO armor)
    //{
    //    itemImage.sprite = armor.ItemIcon;
    //    itemNumberText.text = armor.TierLevel.ToString();
    //    itemNameText.text = armor.GetItemName();
    //    itemDescriptionText.gameObject.SetActive(false);

    //    itemSO = armor;
    //    draggableScript.enabled = false;
    //}

    public void UpdatePanelInfo(PotionSO potion, int healValue)
    {
        itemImage.sprite = potion.ItemIcon;
        itemNumberText.text = potion.TierLevel.ToString();
        itemNameText.text = potion.GetItemName();

        string description = "";
        switch (potion.PotionType)
        {
            case EPotionType.Health_Potion:
                description = "Can heal for " + healValue.ToString() + " health points";
                break;

            case EPotionType.Attack_Potion:
                description = "Can increase damage for " + healValue.ToString() + " points";
                break;

            case EPotionType.Defense_Potion:
                description = "Can increase defense for " + healValue.ToString() + " points";
                break;

            default:
                break;
        }
        itemDescriptionText.text = description;

        itemSO = potion;
        draggableScript.enabled = false;
    }

    public void UpdatePanelInfo(ScrollSpellSO scroll, int atkValue)
    {
        itemImage.sprite = scroll.ItemIcon;
        itemNumberText.text = scroll.TierLevel.ToString();
        itemNameText.text = scroll.GetItemName();
        itemDescriptionText.text = "Can deal " + atkValue.ToString() + " damage";

        itemSO = scroll;
        draggableScript.enabled = false;
    }

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

    //public bool IsTheSameArmorInPanel(ArmorSO armorToCheck)
    //{
    //    if (armorToCheck == null) return false;
    //    if (itemSO is not ArmorSO) return false;

    //    ArmorSO armorInPanel = (ArmorSO)itemSO;
    //    return (armorToCheck.ArmorType == armorInPanel.ArmorType);
    //}

    public bool IsTheSameMaterialInPanel(CraftingMaterialSO materialToCheck)
    {
        if (materialToCheck == null) return false;
        if (itemSO is not CraftingMaterialSO) return false;

        CraftingMaterialSO materialInPanel = (CraftingMaterialSO)itemSO;
        return (materialToCheck.MaterialType == materialInPanel.MaterialType);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemSO is not CraftingMaterialSO) return;

        CraftingSystem.Instance.OnUpdateBookDisplay((CraftingMaterialSO)itemSO);
    }
}
