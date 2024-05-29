using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DicePanel : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;
    [SerializeField] private PotionManager manager;
    [SerializeField] private DiceTypeOld type;


    public void InitializeDice(Sprite sprite, PotionManager m, DiceTypeOld t)
    {
        image.sprite = sprite;
        manager = m;
        type = t;
    }

    public DiceTypeOld GetDiceType()
    {
        return type;
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public void OnDeleteDice()
    {
        manager.DeleteDicePanel(type);
    }
}
