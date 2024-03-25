using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HO_CombatManager : BaseCombatManager
{
    [SerializeField] private HO_UIManager uiManager;
    private bool isOnCombat;


    public void OnPlayerAction(int type)
    {
        if (isOnCombat)
        {
            return;
        }

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

        EnemyData data = HO_GameFlow.Instance.selectedEnemy.GetComponent<Enemy>().GetEnemyData();

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

        if (uiManager != null)
        {
            uiManager.SetActiveAttackButtons(false);
            uiManager.ActivatePopUp(enemyAction, currentEnemy.transform.position);
        }

        isOnCombat = true;
        StartCombat();
    }

    public override void StartRound()
    {
        if (uiManager != null)
        {
            uiManager.SetActiveAttackButtons(true);
        }

        isOnCombat = false; 
    }

    protected override void EndRound()
    {
        StartCoroutine(GameUtilities.DelayFunction(StartRound, 1));
    }

    protected override void OnEnemyDeath()
    {
        if (enemyList.Count > 0)
        {
            HO_GameFlow.Instance.OnReselectEnemy(enemyList[0]);
        }
    }

    protected override void TriggerEndRoom()
    {
        base.TriggerEndRoom();
        //HO_UIManager.Instance.ResetProbabilityBoard();
        HO_GameFlow.Instance.EndRoom();
    }

}
