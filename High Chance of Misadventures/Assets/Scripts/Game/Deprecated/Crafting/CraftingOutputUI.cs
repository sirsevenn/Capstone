using UnityEngine;
using UnityEngine.UI;

public class CraftingOutputUI : MonoBehaviour
{
    [Header("Crafting Output References")]
    [SerializeField] private GameObject outputPanel;
    [SerializeField] private Image possibleOutputImage;

    [Space(20)]
    [SerializeField] private RectTransform successRateRedPortion;
    [SerializeField] private RectTransform successRateGreenPortion;

    [Space(20)]
    [SerializeField] private GameObject craftingEffectPanel;
    [SerializeField] private RectTransform craftingEffectTransform;
    [SerializeField] private RectTransform craftingEffectRedPortion;
    [SerializeField] private RectTransform craftingEffectGreenPortion;

    [Space(10)] [Header("UI Default Values")]
    [SerializeField] private Sprite defaultOutputIcon;


    public void ResetCraftingUI()
    {
        possibleOutputImage.sprite = defaultOutputIcon;
        outputPanel.gameObject.SetActive(false);

        SetSuccessRateBar(0f);
        SetCraftingEffectBar(ECraftingEffect.Unknown, true);
    }

    public void SetIconOnOutputImage(Sprite outputIcon)
    {
        possibleOutputImage.sprite = (outputIcon == null) ? defaultOutputIcon : outputIcon;
        outputPanel.gameObject.SetActive(true);
    }

    public void SetSuccessRateBar(float successRate)
    {
        float height = successRateRedPortion.rect.height;
        successRateGreenPortion.sizeDelta = new Vector2(successRateGreenPortion.rect.width, height * -(1 - successRate));
    }

    public void SetCraftingEffectBar(ECraftingEffect effect, bool disablePanel = false)
    {
        craftingEffectPanel.gameObject.SetActive(!disablePanel);
        craftingEffectRedPortion.sizeDelta = new Vector2(-craftingEffectTransform.rect.width, craftingEffectRedPortion.rect.height);
        craftingEffectGreenPortion.sizeDelta = new Vector2(-craftingEffectTransform.rect.width, craftingEffectRedPortion.rect.height);

        if (disablePanel) return;

        switch (effect)
        {
            case ECraftingEffect.Worst_Effect:
                craftingEffectRedPortion.sizeDelta = new Vector2(-craftingEffectTransform.rect.width / 2f, craftingEffectRedPortion.rect.height);
                break;

            case ECraftingEffect.Bad_Effect:
                craftingEffectRedPortion.sizeDelta = new Vector2(-craftingEffectTransform.rect.width * 3 / 4f, craftingEffectRedPortion.rect.height);
                break;

            case ECraftingEffect.Good_Effect:
                craftingEffectGreenPortion.sizeDelta = new Vector2(-craftingEffectTransform.rect.width * 3 / 4f, craftingEffectGreenPortion.rect.height);
                break;

            case ECraftingEffect.Great_Effect:
                craftingEffectGreenPortion.sizeDelta = new Vector2(-craftingEffectTransform.rect.width / 2f, craftingEffectGreenPortion.rect.height);
                break;

            default:
                break;
        }
    }
}
