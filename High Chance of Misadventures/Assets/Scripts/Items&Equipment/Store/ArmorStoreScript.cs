using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmorStoreScript : MonoBehaviour
{
    [SerializeField] private Image armorImage;
    [SerializeField] private TMP_Text armorNameText;
    [SerializeField] private Button upgradeButton;

    public void SetupArmorStoreTemplate(Armor armor)
    {
        armorImage.sprite = armor.GetArmorData().GetArmorIcon();
        armorNameText.text = armor.GetArmorData().GetArmorName();
        upgradeButton.onClick.AddListener(() => StoreManager.Instance.OnUpgradeArmor(armor.GetArmorData().GetArmorName()));
    }
}
