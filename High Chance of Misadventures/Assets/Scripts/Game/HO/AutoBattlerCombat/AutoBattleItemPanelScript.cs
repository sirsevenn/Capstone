using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoBattleItemPanelScript : MonoBehaviour
{
    [Header("UI Properties")]
    [SerializeField] private RectTransform itemMaskFill;
    [SerializeField] private TMP_Text itemNum;
    [SerializeField] private float fillHeight;
    [SerializeField] private float maxAmount;

    [Header("Item Properties")]
    [SerializeField] private ConsumableSO itemSO;


    private void Start()
    {
        maxAmount = 0;
        UpdateItemAmount(0);
    }

    public void SetItemMaxAmount(int max)
    {
        maxAmount = max;
    }

    public void UpdateItemAmount(int amount)
    {
        itemNum.text = amount.ToString();

        float percent = (maxAmount == 0) ? 0f : (float)amount / (float)maxAmount;
        itemMaskFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fillHeight * percent);
    }

    public bool IsSameItem(EConsumableType consumableType)
    {
        return itemSO.ConsumableType == consumableType;
    }
}
