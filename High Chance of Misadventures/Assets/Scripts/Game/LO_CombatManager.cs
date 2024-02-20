using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class LO_CombatManager : MonoBehaviour
{


    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Spinners")]
    [SerializeField] private float spinDuration;
    [SerializeField] private Spinner playerSpinner;
    [SerializeField] private Spinner enemySpinner;

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
    int queueCount = 0;
    float offsetMultiplier = 0.5f;

    public void EnemyPreCombat()
    {
        readyCombat = true;
        LO_UIManager.Instance.GenerateInventoryPieces();

    }

    public void SpinWheels()
    {
        //
        if (!readyCombat && currentEnemy != null)
        {
            return;
        }

        readyCombat = false;
        playerSpinner.Spin(spinDuration * 0.5f, true);
        enemySpinner.Spin(spinDuration, false);
        StartCoroutine(GameUtilities.DelayFunction(StartCombat, spinDuration + 1.0f));
        //Enemy Spin
        //Delay Start Combat
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
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerHeavyTie);
                }
                else if (enemyAction == ActionType.Light)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerHeavyLose);
                }
                else
                {
                    //queueCount--;
                    currentEnemy.GetComponent<AnimationHandler>().PlayDeathAnimation();
                    enemyList.Remove(currentEnemy);
                    currentEnemy.DeselectEnemy();
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerHeavyWin);
                }

                break;

            case ActionType.Parry:

                if (enemyAction == ActionType.Parry)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerParryTie);
                }
                else if (enemyAction == ActionType.Heavy)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerParryLose);
                }
                else
                {
                    //queueCount--;
                    currentEnemy.GetComponent<AnimationHandler>().PlayDeathAnimation();
                    enemyList.Remove(currentEnemy);
                    currentEnemy.DeselectEnemy();
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerParryWin);
                }

                break;

            case ActionType.Light:

                if (enemyAction == ActionType.Light)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerLightTie);
                }
                else if (enemyAction == ActionType.Parry)
                {
                    currentEnemy.transform.DOJump(enemyStartPos, 1, 1, 0.5f).SetEase(Ease.Linear).SetDelay(2);
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerLightLose);
                }
                else
                {
                    //queueCount--;
                    currentEnemy.GetComponent<AnimationHandler>().PlayDeathAnimation();
                    enemyList.Remove(currentEnemy);
                    currentEnemy.DeselectEnemy();
                    //dialogueManager.ChangeDialogue(CombatResult.PlayerLightWin);
                }

                break;

        }

        StartCoroutine(GameUtilities.WaitForPlayerInput(EndCombat));
    }

    private void EndCombat()
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
            LO_GameFlow.Instance.EndRoom();
           
        }

    }

    public void ResetCombatManager()
    {
        queueCount = 0;
        currentEnemy = null;
        enemyList.Clear();

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
