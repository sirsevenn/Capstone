using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingBookUI : MonoBehaviour
{
    [Header("Description Book References")]
    [SerializeField] private Image materialIcon;
    [SerializeField] private TMP_Text materialName;
    [SerializeField] private TMP_Text outputItemName;
    [SerializeField] private TMP_Text baseProbabilityText;
    [SerializeField] private TMP_Text givenOutcomesText;
    [SerializeField] private TMP_Text craftingEffectText;
    [SerializeField] private TMP_Text materialDescription;


    public void ResetBook()
    {
        materialIcon.color = new Color(1, 1, 1, 0);
        materialName.text = "";
        outputItemName.text = "";
        baseProbabilityText.text = "";
        givenOutcomesText.text = "";
        craftingEffectText.text = "";
        materialDescription.text = "";
    }

    public void SetMaterialDataToBook(CraftingMaterialSO materialData)
    {
        materialIcon.color = new Color(1, 1, 1, 1);
        materialIcon.sprite = materialData.ItemIcon;
        materialName.text = materialData.GetItemName();
        outputItemName.text = InventorySystem.Instance.IsCraftableDiscovered(materialData.ItemToCraft) ? materialData.ItemToCraft.GetItemName() : "???";
        baseProbabilityText.text = materialData.BaseProbabilityValue.ToString();
        givenOutcomesText.text = materialData.SupplementaryAmount.ToString();
        craftingEffectText.text = materialData.CraftingEffect.ToString().Replace("_", " ");
        materialDescription.text = materialData.MaterialDescription;
    }
}