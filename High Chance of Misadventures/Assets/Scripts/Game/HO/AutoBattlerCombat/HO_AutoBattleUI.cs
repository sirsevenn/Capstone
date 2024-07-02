using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HO_AutoBattleUI : MonoBehaviour
{
    [Header("Inventory UI")]
    [SerializeField] private RectTransform inventoryParent;
    [SerializeField] private GameObject inventoryItemPanelPrefab;
    [SerializeField] List<AutoBattleItemPanelScript> consumablesUIList;

    [Space(10)] [Header("Disintegrating Effect")]
    [SerializeField] private Image disintegratingIcon;
    [SerializeField] private Material disintegratingMaterial;
    [SerializeField] private float disintegratingSpriteRevealDuration;
    [SerializeField] private float disintegrationDuration;

    [Space(10)] [Header("Cutscene UI")]
    [SerializeField] private GameObject speechBubbleCanvas;
    [SerializeField] private TMP_Text speechBubble;
    [SerializeField] private TMP_Text levelIntroductionText;

    [Space(10)] [Header("End Level UI")]
    [SerializeField] private GameObject endLevelPanel;
    [SerializeField] private TMP_Text endLevelMainText;
    [SerializeField] private TMP_Text loadLevelText;
    [SerializeField] private GameObject loadLevelButtonObj;


    #region Initialization
    private void Start()
    {
        if (InventorySystem.Instance.GetConsumablesTotalAmount() == 0)
        {
            DisableUI();
        }
        else
        {
            InitializeItemPanel(EConsumableType.Health_Potion);
            InitializeItemPanel(EConsumableType.Defense_Potion);
            InitializeItemPanel(EConsumableType.Fire_Potion);
            InitializeItemPanel(EConsumableType.Water_Potion);
            InitializeItemPanel(EConsumableType.Earth_Potion);

            // Update disintegratingIcon size to scale with the consumable icons
            RectTransform consumableTransform = consumablesUIList.First().GetComponent<RectTransform>();
            // the follwoing UI methods are important for runtime values
            disintegratingIcon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, LayoutUtility.GetPreferredWidth(consumableTransform) / 2f); 
            disintegratingIcon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, LayoutUtility.GetPreferredHeight(consumableTransform) / 2f);
        }

        disintegratingIcon.gameObject.SetActive(false);
        speechBubbleCanvas.SetActive(false);
        endLevelPanel.SetActive(false);
    }

    public void DisableUI()
    {
        inventoryParent.gameObject.SetActive(false);
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
            consumablesUIList.Add(script);
        }
    }
    #endregion

    #region Cutscene Methods
    public void UpdateLevelIntroduction()
    {
        levelIntroductionText.text = "Level " + HO_GameManager.Instance.GetCurrentLevel().LevelID.ToString();
    }

    public void UpdateSpeechBubble(string newText)
    {
        speechBubble.text = newText; 
    }
    #endregion

    #region Inventory Methods
    public void OnUpdateConsumables(Consumable consumable, bool hasAddedNewConsumable, float turnDuration)
    {
        if (hasAddedNewConsumable || consumable == null) return;

        float durationRatio = turnDuration / (disintegratingSpriteRevealDuration + disintegrationDuration);
        float revealDuration = durationRatio * disintegratingSpriteRevealDuration;
        float disintegrateDuration = durationRatio * disintegrationDuration;

        foreach (var consumableUI in consumablesUIList)
        {
            if (consumableUI.IsSameItem(consumable.ConsumableData))
            {
                // Intialize some values used for the disintegration effect
                disintegratingMaterial.SetTexture("_MainTex", consumable.ConsumableData.ItemIcon.texture);
                //disintegratingMaterial.SetColor("_SpriteColor", consumable.ConsumableData.ItemSpriteColor);
                disintegratingMaterial.SetFloat("_CutoffHeight", disintegratingMaterial.GetFloat("_MaxCutoffHeight"));

                disintegratingIcon.gameObject.SetActive(true);
                disintegratingIcon.transform.position = consumableUI.transform.position;

                // Animate the disintegration effect; make the sprite reveal itself first, then manipulate the material
                float distanceToTravel = disintegratingIcon.rectTransform.rect.width * 2f;
                Vector3 endPos = disintegratingIcon.transform.position + new Vector3(0, distanceToTravel, 0);

                disintegratingIcon.transform.DOMove(endPos, revealDuration).onComplete += () => {
                    disintegratingMaterial.DOFloat(disintegratingMaterial.GetFloat("_MinCutoffHeight"), "_CutoffHeight", disintegrateDuration).onComplete += OnCompleteDisintegration;
                };


                // Update the num value
                int num = InventorySystem.Instance.GetConsumableAmount(consumable.ConsumableData.ConsumableType);
                if (num > 0)
                {
                    consumableUI.UpdateItemNum(num);
                }
                else
                {
                    consumablesUIList.Remove(consumableUI);
                    DestroyImmediate(consumableUI.gameObject);

                    if (InventorySystem.Instance.GetConsumablesTotalAmount() == 0)
                    {
                        DisableUI();
                    }
                }

                break;
            }
        }
    }

    private void OnCompleteDisintegration()
    {
        disintegratingMaterial.SetTexture("_MainTex", null);
        disintegratingMaterial.SetColor("_SpriteColor", Color.white);
        disintegratingMaterial.SetFloat("_CutoffHeight", disintegratingMaterial.GetFloat("_MaxCutoffHeight"));
        disintegratingIcon.gameObject.SetActive(false);
    }
    #endregion

    public void EnableEndLevelPanel(bool didPlayerWin, bool isGameBeaten)
    {
        endLevelPanel.SetActive(true);

        if (isGameBeaten)
        {
            endLevelMainText.text = "Congratulations!\nYou have beaten the game!";
            loadLevelButtonObj.SetActive(false);
        }
        else
        {
            endLevelMainText.text = didPlayerWin ? "Level Complete!" : "Game Over!";
            loadLevelText.text = didPlayerWin ? "Proceed to\nNext Level" : "Retry Level";
        }
    }
}
