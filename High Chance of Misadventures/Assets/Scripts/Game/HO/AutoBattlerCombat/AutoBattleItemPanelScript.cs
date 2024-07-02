using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoBattleItemPanelScript : MonoBehaviour
{
    [Header("UI Properties")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemNum;

    [Header("Item Properties")]
    [SerializeField] private ConsumableSO itemSO;


    public void SetItemSO(ConsumableSO itemData)
    {
        itemSO = itemData;
        itemIcon.sprite = itemSO.ItemIcon;
    }

    public void UpdateItemNum(int num)
    {
        itemNum.text = num.ToString();
    }

    public bool IsSameItem(ItemSO item)
    {
        return itemSO.GetItemName() == item.GetItemName();
    }
}
