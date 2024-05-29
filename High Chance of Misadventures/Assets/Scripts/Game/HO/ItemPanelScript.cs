using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanelScript : MonoBehaviour
{
    [Header("UI Properties")]
    [SerializeField] private Image itemImage;
    [Tooltip("Level for the Armor, or Quantity for the Potions and Materials")]
    [SerializeField] private TMP_Text itemNumberText;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private Button itemPanelButton;

    [Header("Item Propterties")]
    [SerializeField] private ScriptableObject itemSO;
    

    public bool IsTheSameItemInPanel(ScriptableObject itemToCheck)
    {
        if (itemToCheck == null) return false;

        if (itemToCheck is ArmorSO && itemSO is ArmorSO)
        {
            ArmorSO armorToCheck = (ArmorSO)itemToCheck;
            ArmorSO armorInPanel = (ArmorSO)itemSO; 
            return (armorToCheck.ArmorType == armorInPanel.ArmorType);
        }
        else if (itemToCheck is PotionSO && itemSO is PotionSO)
        {
            PotionSO potionToCheck = (PotionSO)itemToCheck;
            PotionSO potionInPanel = (PotionSO)itemSO;
            return (potionToCheck.PotionType == potionInPanel.PotionType);
        }
        else if (itemToCheck is CraftingMaterialSO && itemSO is CraftingMaterialSO)
        {
            CraftingMaterialSO materialToCheck = (CraftingMaterialSO)itemToCheck;
            CraftingMaterialSO materialInPanel = (CraftingMaterialSO)itemSO;
            return (materialToCheck.MaterialType == materialInPanel.MaterialType);
        }

        return false;
    }

    public void UpdatePanelInfo(ArmorSO armor)
    {
        itemImage.sprite = armor.CraftableIcon;
        itemNumberText.text = armor.ArmorLevel.ToString();
        itemNameText.text = armor.GetCraftableName();
        itemDescriptionText.gameObject.SetActive(false);
        itemSO = armor;

        itemPanelButton.enabled = false;
    }

    public void UpdatePanelInfo(PotionSO potion, uint quantity)
    {
        itemImage.sprite = potion.CraftableIcon;
        itemNumberText.text = quantity.ToString();
        itemNameText.text = potion.GetCraftableName();
        itemDescriptionText.text = potion.PotionDescription;
        itemSO = potion;

        itemPanelButton.enabled = false;
    }

    public void UpdatePanelInfo(CraftingMaterialSO material, uint quantity)
    {
        itemImage.sprite = material.MaterialIcon;
        itemNumberText.text = quantity.ToString();
        itemNameText.text = material.GetMaterialName();
        itemDescriptionText.text = material.MaterialDescription;
        itemSO = material;

        itemPanelButton.enabled = true;
        itemPanelButton.onClick.AddListener(() => CraftingSystem.Instance.OnClickMaterial(material));
    }
}
