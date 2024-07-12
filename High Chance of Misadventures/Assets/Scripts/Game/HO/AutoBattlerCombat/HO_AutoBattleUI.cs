using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HO_AutoBattleUI : MonoBehaviour
{
    [Header("Inventory UI")]
    [SerializeField] private RectTransform inventoryParent;
    [SerializeField] List<AutoBattleItemPanelScript> consumablesUIList;

    [Space(10)]
    [SerializeField] private GameObject imagePoolParent;
    [SerializeField] private float addPotionsDuration;
    [SerializeField] private float addPotionsDelay;
    private WaitForSeconds addPotionsDelayToSeconds;
    [SerializeField] private AudioClip popSound;

    [Space(20)] [Header("Disintegrating Effect")]
    [SerializeField] private Image disintegratingIcon;
    [SerializeField] private Material disintegratingMaterial;
    [SerializeField] private float disintegratingSpriteRevealDuration;
    [SerializeField] private float disintegrationDuration;

    [Space(20)] [Header("Cutscene UI")]
    [SerializeField] private TMP_Text levelIntroductionText;
    [SerializeField] private GameObject speechBubblePanel;
    [SerializeField] private TMP_Text speechBubble;
    [SerializeField] private float displaySpeechDuration;

    [Space(20)] [Header("End Level UI")]
    [SerializeField] private GameObject endLevelPanel;
    [SerializeField] private TMP_Text endLevelMainText;
    [SerializeField] private TMP_Text loadLevelText;
    [SerializeField] private GameObject loadLevelButtonObj;


    #region Initialization
    private void Start()
    {
        // Update Item Panel
        foreach (var type in Enum.GetValues(typeof(EConsumableType)))
        {
            EConsumableType consumableType = (EConsumableType)type;
            if (consumableType != EConsumableType.Unknown)
            {
                ResetItemPanelNumbersToZero(consumableType);
            }
        }

        // Update disintegratingIcon size to scale with the consumable icons
        RectTransform consumableTransform = consumablesUIList.First().GetComponent<RectTransform>();

        // the follwoing UI methods are important for runtime values
        disintegratingIcon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, LayoutUtility.GetPreferredWidth(consumableTransform) / 2f); 
        disintegratingIcon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, LayoutUtility.GetPreferredHeight(consumableTransform) / 2f);

        disintegratingIcon.gameObject.SetActive(false);
        endLevelPanel.SetActive(false);
        DisableSpeechBubble();
    }

    private void ResetItemPanelNumbersToZero(EConsumableType consumableType)
    {
        AutoBattleItemPanelScript itemPanel = consumablesUIList.Find(x => x.IsSameItem(consumableType));
        itemPanel.UpdateItemAmount(0);
    }
    #endregion

    #region Cutscene Methods
    public void UpdateLevelIntroduction()
    {
        levelIntroductionText.text = "Level " + HO_GameManager.Instance.GetCurrentLevel().LevelID.ToString();
    }

    public void SetSpeechBubblePosition(Vector3 worldPos)
    {
        speechBubblePanel.transform.position = Camera.main.WorldToScreenPoint(worldPos);
    }

    public IEnumerator DisplaySpeechBubble(string newText)
    {
        speechBubble.text = newText;

        if (!speechBubblePanel.activeSelf)
        {
            speechBubblePanel.SetActive(true);
        }

        speechBubblePanel.transform.localScale = Vector3.zero;
        yield return speechBubblePanel.transform.DOScale(Vector3.one, displaySpeechDuration).SetEase(Ease.OutCubic).WaitForCompletion();
    }

    public void DisableSpeechBubble()
    {
        speechBubblePanel.SetActive(false);
    }
    #endregion

    #region Inventory Methods
    public void DisableUI()
    {
        inventoryParent.gameObject.SetActive(false);
    }

    public IEnumerator AnimateAddPotionsToInventory(Vector3 knightPos)
    {
        addPotionsDelayToSeconds = new WaitForSeconds(addPotionsDelay);
        int id = 1;

        int totalAmount = InventorySystem.Instance.GetConsumablesTotalAmount();
        float remainingDuration = (addPotionsDuration - (InventorySystem.Instance.GetConsumablesTotalAmount()) * addPotionsDelay);
        remainingDuration = (totalAmount == 0) ? 0f : remainingDuration;

        foreach (var type in Enum.GetValues(typeof(EConsumableType)))
        {
            // Get consumable type and then retrieve it from the inventory
            EConsumableType consumableType = (EConsumableType)type;
            if (consumableType == EConsumableType.Unknown) continue;

            int consumableAmount = InventorySystem.Instance.GetConsumableAmountOfType(consumableType);
            Consumable consumable = InventorySystem.Instance.GetOneConsumableOfType(consumableType);

            // Get reference to consumable UI then initialize; also determine the screen position of the knight
            AutoBattleItemPanelScript itemPanel = consumablesUIList.Find(x => x.IsSameItem(consumableType));
            itemPanel.SetItemMaxAmount(consumableAmount);

            Vector2 knightScreenPos = Camera.main.WorldToScreenPoint(knightPos);

            // Create empty objects with images to animate
            for (int i = 1; i <= consumableAmount; i++) 
            {
                GameObject newObj = new GameObject("PotionImage" + id);
                newObj.layer = LayerMask.NameToLayer("UI");

                RectTransform newObjTransform = newObj.AddComponent<RectTransform>();
                newObjTransform.SetParent(imagePoolParent.transform, true);
                newObjTransform.position = knightScreenPos;
                newObjTransform.localScale = new Vector2(0.65f, 0.65f);
                newObjTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, LayoutUtility.GetPreferredWidth(itemPanel.transform as RectTransform) * 0.6f);
                newObjTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, LayoutUtility.GetPreferredHeight(itemPanel.transform as RectTransform) * 0.6f);

                Image img = newObj.AddComponent<Image>();
                img.sprite = consumable.ConsumableData.ConsumableModelSprite;

                int currentAmount = i; // value of i will change after the animation, so store it in another local var
                
                newObjTransform.DOMove(itemPanel.transform.position, remainingDuration).SetEase(Ease.Linear).OnComplete(() => { 
                    newObj.SetActive(false); 
                    itemPanel.UpdateItemAmount(currentAmount);
                    SoundEffectManager.Instance.PlaySoundEffect(popSound);
                });
                newObjTransform.DOScale(Vector2.one, remainingDuration * 0.45f);

                yield return addPotionsDelayToSeconds;
            }
        }

        yield return new WaitForSeconds(remainingDuration + 1f);
    }

    public void OnUpdateConsumables(Consumable consumable, bool hasAddedNewConsumable, float turnDuration)
    {
        if (hasAddedNewConsumable || consumable == null) return;

        float durationRatio = turnDuration / (disintegratingSpriteRevealDuration + disintegrationDuration);
        float revealDuration = durationRatio * disintegratingSpriteRevealDuration;
        float disintegrateDuration = durationRatio * disintegrationDuration;

        foreach (var consumableUI in consumablesUIList)
        {
            if (consumableUI.IsSameItem(consumable.ConsumableData.ConsumableType))
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
                    disintegratingMaterial.DOFloat(disintegratingMaterial.GetFloat("_MinCutoffHeight"), "_CutoffHeight", disintegrateDuration).OnComplete(OnCompleteDisintegration);
                };


                // Update the amount value
                int amount = InventorySystem.Instance.GetConsumableAmountOfType(consumable.ConsumableData.ConsumableType);
                consumableUI.UpdateItemAmount(amount);

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

        if (isGameBeaten && didPlayerWin)
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
