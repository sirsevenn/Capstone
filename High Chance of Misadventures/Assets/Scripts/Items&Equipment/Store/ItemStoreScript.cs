using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStoreScript : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private TMP_Text itemQuantity;


    // TODO: replace SO to class
    public void InitializeItemStoreTemplate(ItemDataSO data)
    {
        itemImage.sprite = data.GetItemIcon();
        itemName.text = data.GetItemName();
        itemDescription.text = data.GetItemDescription();
        // itemQuantity.text = 
    }
}
