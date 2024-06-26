using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProbabilityManager : MonoBehaviour
{
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text defenseText;
    [SerializeField] private TMP_Text skillText;

    [Space(20)]
    [SerializeField] private Image attackCircle;
    [SerializeField] private Image defendCircle;
    [SerializeField] private Image skillCircle;

    public ActionType RollProbability()
    {
        int heavyValue = Random.Range(0, 100);
        attackText.text = heavyValue.ToString() + "%";

        int lightValue = Random.Range(0, 100 - heavyValue);
        defenseText.text = lightValue.ToString() + "%";

        int parryValue = 100 - heavyValue - lightValue;
        skillText.text = parryValue.ToString() + "%";

        attackCircle.fillAmount = 1.0f;
        defendCircle.fillAmount = (lightValue * 0.01f) + (parryValue * 0.01f);
        skillCircle.fillAmount = parryValue * 0.01f;

       

        int random = Random.Range(0, 100);

        Debug.Log("random value: " + random + ", heavy value: " + heavyValue + " parry Value: " + parryValue + " light Value: " + lightValue);
        if (random >= 0 && random < heavyValue)
        {
            return ActionType.Fire;
        }
        else if (random >= heavyValue && random < (heavyValue + lightValue))
        {
            return ActionType.Earth;
        }
        else if (random >= (heavyValue + lightValue) && random < 100)
        {
            return ActionType.Water;
        }

        return ActionType.None;

    }

}
