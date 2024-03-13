using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStoreScript : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private TMP_Text itemQuantity;
    [SerializeField] private Button buyButton;

    public void SetupItemStoreTemplate(ItemDataSO itemData, int quantity)
    {
        itemImage.sprite = itemData.GetItemIcon();
        itemName.text = itemData.GetItemName();
        itemDescription.text = itemData.GetItemDescription();
        itemQuantity.text = quantity.ToString();

        buyButton.onClick.AddListener(() => StoreManager.Instance.OnBuyItem(itemData.GetItemType(), this));
    }

    public void UpdateQuantity(int amount)
    {
        itemQuantity.text = amount.ToString();
    }
}
