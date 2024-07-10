using UnityEngine;
using UnityEngine.UI;

public class CraftingEffectPanelScript : MonoBehaviour
{
    [SerializeField] private Image positiveEffectImage;
    [SerializeField] private Image negativeEffectImage;


    private void Awake()
    {
        positiveEffectImage.enabled = false;
        negativeEffectImage.enabled = false;
    }

    public void SetPositiveEffectIcon(Sprite icon)
    {
        positiveEffectImage.enabled = true;
        positiveEffectImage.sprite = icon;
    }

    public void SetNegativeEffectIcon(Sprite icon)
    {
        negativeEffectImage.enabled = true;
        negativeEffectImage.sprite = icon;
    }
}