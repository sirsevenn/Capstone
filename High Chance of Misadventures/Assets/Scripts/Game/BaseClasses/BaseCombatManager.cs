using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class BaseCombatManager : MonoBehaviour
{
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

    //numbers
    protected int queueCount = 0;
    protected float offsetMultiplier = 0.5f;

    public void EnemyPreCombat()
    {
        readyCombat = true;
        OnEnemyPreCombat();
        
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

    protected void CombatProper(Vector3 playerStartPos, Vector3 enemyStartPos)
    {
        switch (playerAction)
        {

            case ActionType.Heavy:

                if (enemyAction == ActionType.Heavy)
                {
                    player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);

                    Health health = currentEnemy.GetComponent<Health>();
                    health.ApplyDamage((int)(testDamage * 1.5f));

                    if (health.GetHP() <= 0)
                    {
                        currentEnemy.Die();
                        enemyList.Remove(currentEnemy);
                        currentEnemy.DeselectEnemy();
                    }
                    else
                    {
                        currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    }
                }
                else if (enemyAction == ActionType.Light)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);

                    Health health = LO_GameFlow_PVP.Instance.health;
                    health.ApplyDamage(currentEnemy.GetEnemyData().attackDamage);

                    if (health.GetHP() <= 0)
                    {
                        //playerdie
                        //changescene
                        player.gameObject.GetComponent<AnimationHandler>().PlayDeathAnimation();
                        LO_UIManager_PVP.Instance.EndGame(false);
                    }
                    else
                    {
                        player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                       
                    }

                    //dialogueManager.ChangeDialogue(CombatResult.PlayerHeavyLose);
                }
                else
                {
                    player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);

                    Health health = currentEnemy.GetComponent<Health>();
                    health.ApplyDamage(testDamage);

                    if (health.GetHP() <= 0)
                    {
                        currentEnemy.Die();
                        enemyList.Remove(currentEnemy);
                        currentEnemy.DeselectEnemy();
                    }
                    else
                    {
                        player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                        currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    }
                }

                break;

            case ActionType.Parry:

                if (enemyAction == ActionType.Parry)
                {
                    player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    Health health = currentEnemy.GetComponent<Health>();
                    health.ApplyDamage((int)(testDamage * 1.5f));

                    if (health.GetHP() <= 0)
                    {
                        currentEnemy.Die();
                        enemyList.Remove(currentEnemy);
                        currentEnemy.DeselectEnemy();
                    }
                    else
                    {
                        currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    }

                }
                else if (enemyAction == ActionType.Heavy)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);

                    Health health = LO_GameFlow_PVP.Instance.health;
                    health.ApplyDamage(currentEnemy.GetEnemyData().attackDamage);

                    if (health.GetHP() <= 0)
                    {
                        //playerdie
                        //changescene
                        player.gameObject.GetComponent<AnimationHandler>().PlayDeathAnimation();
                        LO_UIManager_PVP.Instance.EndGame(false);
                    }
                    else
                    {
                        player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    }
                }
                else
                {
                    player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);

                    Health health = currentEnemy.GetComponent<Health>();
                    health.ApplyDamage(testDamage);

                    if (health.GetHP() <= 0)
                    {
                        currentEnemy.Die();
                        enemyList.Remove(currentEnemy);
                        currentEnemy.DeselectEnemy();
                    }
                    else
                    {
                        currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    }
                }

                break;

            case ActionType.Light:

                if (enemyAction == ActionType.Light)
                {
                    player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    Health health = currentEnemy.GetComponent<Health>();
                    health.ApplyDamage((int)(testDamage * 1.5f));

                    if (health.GetHP() <= 0)
                    {
                        currentEnemy.Die();
                        enemyList.Remove(currentEnemy);
                        currentEnemy.DeselectEnemy();
                    }
                    else
                    {
                        currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    }

                }
                else if (enemyAction == ActionType.Parry)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);

                    Health health = LO_GameFlow_PVP.Instance.health;
                    health.ApplyDamage(currentEnemy.GetEnemyData().attackDamage);

                    if (health.GetHP() <= 0)
                    {
                        //playerdie
                        //changescene
                        player.gameObject.GetComponent<AnimationHandler>().PlayDeathAnimation();
                        LO_UIManager_PVP.Instance.EndGame(false);
                    }
                    else
                    {
                        player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                        
                    }
                }
                else
                {
                    player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    Health health = currentEnemy.GetComponent<Health>();
                    health.ApplyDamage(testDamage);

                    if (health.GetHP() <= 0)
                    {
                        currentEnemy.Die();
                        enemyList.Remove(currentEnemy);
                        currentEnemy.DeselectEnemy();
                    }
                    else
                    {
                        currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    }
                }

                break;

        }

        OnStartCombat();

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
            EnemyPreCombat();
        }
        else if (enemyList.Count == 0)
        {
            //Exit Room
            TriggerEndRoom();
        }

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
