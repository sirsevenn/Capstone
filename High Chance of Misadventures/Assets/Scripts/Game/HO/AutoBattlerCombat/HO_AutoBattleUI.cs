using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HO_AutoBattleUI : MonoBehaviour
{
    [Header("Player UI")]
    [SerializeField] private Slider playerHPBar;

    [Space(10)] [Header("Inventory UI")]
    [SerializeField] private RectTransform inventoryParent;
    [SerializeField] private GameObject inventoryItemPanelPrefab;

    [Space(10)] [Header("Item Panels Tracker")]
    [SerializeField] List<AutoBattleItemPanelScript> potionsList;
    [SerializeField] List<AutoBattleItemPanelScript> scrollsList;


    #region Singleton
    public static HO_AutoBattleUI Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance != null && Instance == this)
        {
            Destroy(this.gameObject);

            InventorySystem.Instance.OnUpdatePotionsEvent -= OnUpdatePotions;
            InventorySystem.Instance.OnUpdateScrollsEvent -= OnUpdateScrolls;
        }
    }
    #endregion


    private void Start()
    {
        InitializePotionPanel(EPotionType.Health_Potion);
        InitializePotionPanel(EPotionType.Attack_Potion);
        InitializePotionPanel(EPotionType.Defense_Potion);
        InitializeScrollPanel(EElementalAttackType.Fire);
        InitializeScrollPanel(EElementalAttackType.Water);
        InitializeScrollPanel(EElementalAttackType.Earth);

        InventorySystem.Instance.OnUpdatePotionsEvent += OnUpdatePotions;
        InventorySystem.Instance.OnUpdateScrollsEvent += OnUpdateScrolls;
    }

    private void InitializePotionPanel(EPotionType type)
    {
        int numItem = InventorySystem.Instance.GetPotionAmount(type);
        if (numItem > 0)
        {
            Potion samplePotion = InventorySystem.Instance.GetOnePotionOfType(type);

            GameObject newItem = GameObject.Instantiate(inventoryItemPanelPrefab, inventoryParent);
            AutoBattleItemPanelScript script = newItem.GetComponent<AutoBattleItemPanelScript>();
            script.SetItemSO(samplePotion.PotionData);
            script.UpdateItemNum(numItem);
            potionsList.Add(script);
        }
    }

    private void InitializeScrollPanel(EElementalAttackType type)
    {
        int numItem = InventorySystem.Instance.GetScrollAmount(type);
        if (numItem > 0)
        {
            ScrollSpell sampleScroll = InventorySystem.Instance.GetOneScrollOfType(type);

            GameObject newItem = GameObject.Instantiate(inventoryItemPanelPrefab, inventoryParent);
            AutoBattleItemPanelScript script = newItem.GetComponent<AutoBattleItemPanelScript>();
            script.SetItemSO(sampleScroll.ScrollData);
            script.UpdateItemNum(numItem);
            scrollsList.Add(script);
        }
    }

    public void UpdatePlayerHP(float value)
    {
        if (value < 0 || value > 1) return;

        playerHPBar.value = value;
    }

    private void OnUpdatePotions(Potion potion, bool hasAddedNewPotion)
    {
        if (hasAddedNewPotion || potion == null) return;

        foreach (var potionUI in potionsList)
        {
            if (potionUI.IsSameItem(potion.PotionData))
            {
                int num = InventorySystem.Instance.GetPotionAmount(potion.PotionData.PotionType);
                if (num > 0)
                {
                    potionUI.UpdateItemNum(num);
                }
                else
                {
                    DestroyImmediate(potionUI.gameObject);
                }

                break;
            }
        }
    }

    private void OnUpdateScrolls(ScrollSpell scroll, bool hasAddedNewScroll)
    {
        if (hasAddedNewScroll || scroll == null) return;

        foreach (var scrollUI in scrollsList)
        {
            if (scrollUI.IsSameItem(scroll.ScrollData))
            {
                int num = InventorySystem.Instance.GetScrollAmount(scroll.ScrollData.ElementalAttackType);
                if (num > 0)
                {
                    scrollUI.UpdateItemNum(num);
                }
                else
                {
                    DestroyImmediate(scrollUI.gameObject);
                }

                break;
            }
        }
    }
}
