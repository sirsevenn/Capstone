using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    [Header("Item Properties")]
    [SerializeField] protected Sprite itemIcon;


    public Sprite ItemIcon
    {
        get { return itemIcon; }
        private set { itemIcon = value; }
    }

    public abstract string GetItemName();
}
