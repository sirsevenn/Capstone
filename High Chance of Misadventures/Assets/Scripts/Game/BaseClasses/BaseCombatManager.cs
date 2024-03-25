using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class BaseCombatManager : MonoBehaviour
{

    enum MatchResult
    {
        None,
        Win,
        Lose,
        Draw
    }

    [Header("Debug")]
    [SerializeField] private int testDamage = 10;

    [Header("Player")]
    [SerializeField] protected GameObject player;

    [Header("Actions")]
    public ActionType playerAction;
    public ActionType enemyAction;

    [Header("Enemy Settings")]
    public Enemy currentEnemy = null;

    [Header("Game Components")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("Settings")]
    public bool readyCombat = false;

    [Space(10)]
    public List<Enemy> enemyList = new List<Enemy>();

    [Header("Scene Managers")]
    [SerializeField] private BaseGameFlow gameFlow;


    //numbers
    protected int queueCount = 0;
    protected float offsetMultiplier = 0.5f;

    public void EnemyPreCombat()
    {
        readyCombat = true;
        OnEnemyPreCombat();
    }

    public virtual void StartRound()
    {

    }

    protected virtual void OnEnemyPreCombat()
    {

    }

    public void StartCombat()
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
        player.transform.DOJump(targetPos - offset * offsetMultiplier, 1, 1, 0.5f).SetEase(Ease.Linear).OnComplete(() => { CombatProper(playerStartPos, enemyStartPos); } );

        PlayerAnimation(playerAction);
        EnemyAnimation(enemyAction);

    }
    protected virtual void OnEnemyDeath()
    {

    }

    protected virtual void OnPlayerDeath()
    {

    }

    protected void CombatProper(Vector3 playerStartPos, Vector3 enemyStartPos)
    {
        MatchResult result = MatchResult.None;
        switch (playerAction)
        {

            case ActionType.Heavy:

                if (enemyAction == ActionType.Heavy)
                {
                    result = MatchResult.Draw;
                }
                else if (enemyAction == ActionType.Light)
                {
                    result = MatchResult.Lose;
                }
                else
                {
                    result = MatchResult.Win;
                }
                break;

            case ActionType.Parry:

                if (enemyAction == ActionType.Parry)
                {
                    result = MatchResult.Draw;
                }
                else if (enemyAction == ActionType.Heavy)
                {
                    result = MatchResult.Lose;
                }
                else
                {
                    result = MatchResult.Win;
                }

                break;

            case ActionType.Light:

                if (enemyAction == ActionType.Light)
                {
                    result = MatchResult.Draw;
                }
                else if (enemyAction == ActionType.Parry)
                {
                    result = MatchResult.Lose;
                }
                else
                {
                    result = MatchResult.Win;
                }

                break;

        }

        Health health;
        switch (result)
        {
            case MatchResult.None:
                Debug.Log("Error on Match Result");
                break;
            case MatchResult.Win:
                player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                health = currentEnemy.GetComponent<Health>();
                health.ApplyDamage(testDamage);

                if (health.GetHP() <= 0)
                {
                    currentEnemy.Die();
                    currentEnemy.DeselectEnemy();
                    enemyList.Remove(currentEnemy);
                    currentEnemy = null;
                    OnEnemyDeath();
                }
                else
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                }
                break;

            case MatchResult.Lose:
                currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);

                //health = LO_GameFlow_PVP.Instance.health;
                health = gameFlow.health;
                health.ApplyDamage(currentEnemy.GetEnemyData().attackDamage);

                if (health.GetHP() <= 0)
                {
                    player.gameObject.GetComponent<AnimationHandler>().PlayDeathAnimation();
                    OnPlayerDeath();
                    //LO_UIManager_PVP.Instance.EndGame(false);
                }
                else
                {
                    player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);

                }
                break;

            case MatchResult.Draw:
                player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                health = currentEnemy.GetComponent<Health>();
                health.ApplyDamage((int)(testDamage * 1.5f));

                if (health.GetHP() <= 0)
                {
                    currentEnemy.Die();
                    currentEnemy.DeselectEnemy();
                    enemyList.Remove(currentEnemy);
                    currentEnemy = null;
                    OnEnemyDeath();
                }
                else
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                }
                break;
            default:
                break;
        }

        //OnStartCombat();
        StartCoroutine(GameUtilities.DelayFunction(EndCombat, 2));
    }

    protected virtual void OnStartCombat()
    {

    }

    protected void EndCombat()
    {
        //If there are more go next combat
        if (enemyList.Count > 0)
        {
            StartCoroutine(GameUtilities.DelayFunction(EnemyPreCombat, 2));
            EndRound();
        }
        else if (enemyList.Count == 0)
        {
            //Exit Room
            StartCoroutine(GameUtilities.DelayFunction(TriggerEndRoom, 2));
        }

    }

    protected virtual void EndRound()
    {

    }

    protected virtual void TriggerEndRoom()
    {

    }

    public void ResetCombatManager()
    {
        queueCount = 0;
        currentEnemy = null;
        enemyList.Clear();
    }

    protected void PlayerAnimation(ActionType action)
    {
        AnimationHandler handler = player.gameObject.GetComponent<AnimationHandler>();
        handler.PlayAnimation(action);
    }

    protected void EnemyAnimation(ActionType action)
    {
        AnimationHandler handler = currentEnemy.gameObject.GetComponent<AnimationHandler>();
        handler.PlayAnimation(action);
    }
}
