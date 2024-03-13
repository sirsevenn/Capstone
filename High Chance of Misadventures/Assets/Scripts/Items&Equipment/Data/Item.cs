using System;
using UnityEngine;

[Serializable]
public class Item 
{
    [SerializeField] private ItemDataSO itemData;
    [SerializeField] private int itemQuantity;

    public Item(ItemDataSO itemData, int itemQuantity)
    {
        this.itemData = itemData;
        this.itemQuantity = itemQuantity;
    }

    public ItemDataSO GetItemData() => itemData;

    public int GetItemQuantity() => itemQuantity;

    public void UpdateQuantity(int amount)
    {
        itemQuantity += amount;
    }
}
