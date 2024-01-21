using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class CombatManager : MonoBehaviour
{
    #region singleton
    public static CombatManager Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;

    }
    #endregion

    [SerializeField] private ActionType enemyAction;

    [SerializeField] private GameObject player;
    [SerializeField] private Enemy currentEnemy;

    public List<Enemy> enemyList = new List<Enemy>();

    //numbers
    int queueCount = 0;
    float offsetMultiplier = 0.5f;
    

    private void Start()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].probabilityBoard.SetActive(false);
        }

        EnemyPreCombat();
    }

    public void EnemyPreCombat()
    {
        currentEnemy = enemyList[queueCount];

        enemyAction = currentEnemy.probabilityManager.RollProbability();
        currentEnemy.probabilityBoard.SetActive(true);

    }

    public void StartComabat(ActionType playerAction)
    {
        Vector3 playerStartPos;
        Vector3 enemyStartPos;

        Vector3 targetPos;
        Vector3 offset;

        playerStartPos = player.transform.position;
        enemyStartPos = currentEnemy.transform.position;

        targetPos = (currentEnemy.transform.position + player.transform.position) * 0.5f;
        offset = targetPos - player.transform.position;
        offset.Normalize();

        currentEnemy.transform.DOJump(targetPos + offset * offsetMultiplier, 1, 1, 0.5f).SetEase(Ease.Linear);
        player.transform.DOJump(targetPos - offset * offsetMultiplier, 1, 1, 0.5f).SetEase(Ease.Linear);

        PlayerAnimation(playerAction);
        EnemyAnimation(enemyAction);

        player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2).OnComplete(EndCombat);

        switch (playerAction)
        {

            case ActionType.Attack:

                if (enemyAction == ActionType.Attack || enemyAction == ActionType.Defend)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                }
                else
                {
                    queueCount--;
                    currentEnemy.GetComponent<AnimationHandler>().PlayDeathAnimation();
                    enemyList.Remove(currentEnemy);
                }

                break;

            case ActionType.Defend:

                if (enemyAction == ActionType.Skill || enemyAction == ActionType.Defend)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                }
                else
                {
                    queueCount--;
                    currentEnemy.GetComponent<AnimationHandler>().PlayDeathAnimation();
                    enemyList.Remove(currentEnemy);
                }

                break;

            case ActionType.Skill:

                if (enemyAction == ActionType.Attack || enemyAction == ActionType.Skill)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                }
                else
                {
                    queueCount--;
                    currentEnemy.GetComponent<AnimationHandler>().PlayDeathAnimation();
                    enemyList.Remove(currentEnemy);
                }

                break;

        }
    }

    private void EndCombat()
    {
        //Check For Enemies
        currentEnemy.probabilityBoard.SetActive(false);
       
        //If there are more go next combat
        if (enemyList.Count > 0)
        {
            queueCount++;

            if (queueCount == enemyList.Count)
            {
                queueCount = 0;
            }

            EnemyPreCombat();
        }
    }

    private void PlayerAnimation(ActionType action)
    {
        AnimationHandler handler = player.gameObject.GetComponent<AnimationHandler>();
        handler.PlayAnimation(action);
    }

    private void EnemyAnimation(ActionType action)
    {
        AnimationHandler handler = currentEnemy.gameObject.GetComponent<AnimationHandler>();
        handler.PlayAnimation(action);
    }
}
