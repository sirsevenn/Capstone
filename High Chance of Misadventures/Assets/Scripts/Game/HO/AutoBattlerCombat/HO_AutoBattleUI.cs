using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HO_AutoBattleUI : MonoBehaviour
{
    [Header("Inventory UI")]
    [SerializeField] private RectTransform inventoryParent;
    [SerializeField] private GameObject inventoryItemPanelPrefab;
    [SerializeField] List<AutoBattleItemPanelScript> consumablesList;

    [Space(10)] [Header("Cutscene UI")]
    [SerializeField] private TMP_Text speechBubble;


    private void Start()
    {
        InitializeItemPanel(EConsumableType.Health_Potion);
        InitializeItemPanel(EConsumableType.Defense_Potion);
        InitializeItemPanel(EConsumableType.Fire_Potion);
        InitializeItemPanel(EConsumableType.Water_Potion);
        InitializeItemPanel(EConsumableType.Earth_Potion);

        InventorySystem.Instance.OnUpdateConsumablesEvent += OnUpdateConsumables;
    }

    private void InitializeItemPanel(EConsumableType consumableType)
    {
        int numItem = InventorySystem.Instance.GetConsumableAmount(consumableType);
        if (numItem > 0)
        {
            Consumable sampleConsumable = InventorySystem.Instance.GetOneConsumableOfType(consumableType);

            GameObject newItem = GameObject.Instantiate(inventoryItemPanelPrefab, inventoryParent);
            AutoBattleItemPanelScript script = newItem.GetComponent<AutoBattleItemPanelScript>();
            script.SetItemSO(sampleConsumable.ConsumableData);
            script.UpdateItemNum(numItem);
            consumablesList.Add(script);
        }
    }

    public void DisableUI()
    {
        inventoryParent.gameObject.SetActive(false);
    }

    public void UpdateSpeechBubble(string newText)
    {
        speechBubble.text = newText; 
    }

    private void OnUpdateConsumables(Consumable consumable, bool hasAddedNewConsumable)
    {
        if (hasAddedNewConsumable || consumable == null) return;

        foreach (var consumableUI in consumablesList)
        {
            if (consumableUI.IsSameItem(consumable.ConsumableData))
            {
                int num = InventorySystem.Instance.GetConsumableAmount(consumable.ConsumableData.ConsumableType);
                if (num > 0)
                {
                    consumableUI.UpdateItemNum(num);
                }
                else
                {
                    DestroyImmediate(consumableUI.gameObject);
                }

                break;
            }
        }
    }
}
