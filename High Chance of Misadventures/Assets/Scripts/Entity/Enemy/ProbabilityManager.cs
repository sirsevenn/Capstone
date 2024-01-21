using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProbabilityManager : MonoBehaviour
{
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text defenseText;
    [SerializeField] private TMP_Text skillText;

    private void Start()
    {
        
    }

    public ActionType RollProbability()
    {
        int attackValue = Random.Range(0, 100);
        attackText.text = attackValue.ToString() + "%";

        int defenseValue = Random.Range(0, 100 - attackValue);
        defenseText.text = defenseValue.ToString() + "%";

        int skillValue = 100 - attackValue - defenseValue;
        skillText.text = skillValue.ToString() + "%";

        int random = Random.Range(0, 100);
        if (random > 0 && random < attackValue)
        {
            return ActionType.Attack;
        }
        else if (random > attackValue && random < (attackValue + defenseValue))
        {
            return ActionType.Defend;
        }
        else if (random > (attackValue + defenseValue) && random < 100)
        {
            return ActionType.Skill;
        }

        return ActionType.None;

    }
}
