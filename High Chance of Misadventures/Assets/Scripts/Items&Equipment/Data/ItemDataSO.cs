using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Inventory/Item")]
public class ItemDataSO : ScriptableObject
{
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private EItemTypes itemType;
    [SerializeField] private string itemName;
    [TextArea(4, 10)][SerializeField] private string itemDescription;
    [SerializeField] private int buyPrice;

    public Sprite GetItemIcon() => itemIcon;

    public EItemTypes GetItemType() => itemType;

    public string GetItemName() => itemName;

    public string GetItemDescription() => itemDescription;

    public int GetBuyPrice() => buyPrice;
}