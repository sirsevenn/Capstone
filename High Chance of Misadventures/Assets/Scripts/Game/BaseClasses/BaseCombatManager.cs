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
        player.transform.DOJump(targetPos - offset * offsetMultiplier, 1, 1, 0.5f).SetEase(Ease.Linear);

        PlayerAnimation(playerAction);
        EnemyAnimation(enemyAction);

        player.transform.DOJump(playerStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);

        switch (playerAction)
        {

            case ActionType.Heavy:

                if (enemyAction == ActionType.Heavy)
                {
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
                        Debug.Log("Go Back");
                        currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    }
                }
                else if (enemyAction == ActionType.Light)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerHeavyLose);
                }
                else
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerHeavyTie);
                }

                break;

            case ActionType.Parry:

                if (enemyAction == ActionType.Parry)
                {
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
                        Debug.Log("Go Back");
                        currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    }
                   
                }
                else if (enemyAction == ActionType.Heavy)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerParryLose);
                }
                else
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerParryTie);
                }

                break;

            case ActionType.Light:

                if (enemyAction == ActionType.Light)
                {
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
                        Debug.Log("Go Back");
                        currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    }
                   
                }
                else if (enemyAction == ActionType.Parry)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerLightLose);
                }
                else
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerLightTie);
                }

                break;

        }

        OnStartCombat();

        StartCoroutine(GameUtilities.WaitForPlayerInput(EndCombat));
    }

    protected virtual void OnStartCombat()
    {

    }

    protected void EndCombat()
    {

        //Check For Enemies
        //currentEnemy.probabilityBoard.SetActive(false);
        //dialogueManager.OpenPlayerButtons();


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
