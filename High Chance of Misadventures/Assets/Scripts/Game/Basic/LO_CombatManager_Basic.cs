using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LO_CombatManager_Basic : BaseCombatManager
{
    public void PlayerAction(int type)
    {
        //PLAYER ACTION
        switch (type)
        {
            case 0:
                playerAction = ActionType.Heavy;
                break;
            case 1:
                playerAction = ActionType.Light;
                break;
            case 2:
                playerAction = ActionType.Parry;
                break;
        }

        EnemyData data = LO_GameFlow_Basic.Instance.selectedEnemy.GetComponent<Enemy>().GetEnemyData();

        //Enemey Probability
        int total = data.heavyAttackProbability + data.lightAttackProbability + data.parryAttackProbability;
        int random = Random.Range(0, total);

        if(random >= 0 && random < data.heavyAttackProbability)
        {
            enemyAction = ActionType.Heavy;
        }
        else if(random >= data.heavyAttackProbability && random < (data.heavyAttackProbability + data.lightAttackProbability))
        {
            enemyAction = ActionType.Light;
        }
        else if(random >= (data.heavyAttackProbability + data.lightAttackProbability) && random < (data.heavyAttackProbability + data.lightAttackProbability + data.parryAttackProbability))
        {
            enemyAction = ActionType.Parry;
        }

        StartCombat();
    }

    protected override void TriggerEndRoom()
    {
        base.TriggerEndRoom();
        LO_UIManager_Basic.Instance.ResetProbabilityBoard();
        LO_GameFlow_Basic.Instance.EndRoom();
    }

}
