using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HO_AutoBattleManager : MonoBehaviour
{
    [Header("Entity Properties")]
    [SerializeField] private HO_PlayerAI playerScript;
    [SerializeField] private HO_EnemyAI enemyScript;

    [Space(10)]
    [SerializeField] private Vector3 playerBattlePos;
    [SerializeField] private Vector3 enemyBattlePos;
    [SerializeField] private Vector3 distanceOffset;

    [Space(10)] [Header("Region Properties")]
    [SerializeField] private GameObject environmentModel;
    [SerializeField] private HO_RegionSO currentRegion;
    [SerializeField] private List<HO_RegionSO> regionsDataList;

    [Space(10)] [Header("Battle Properties")]
    [SerializeField] private bool isBattleSimulationFinished;
    [SerializeField] private bool didPlayerWin;


    private void Start()
    {
        InitializeEnvironment();
        InitializeEnemy();

        isBattleSimulationFinished = false;
        didPlayerWin = false;

        StartCoroutine(PlayerEnterAnimation());
    }

    private void InitializeEnvironment()
    {
        int currentRegionLevel = HO_ProgressTracker.Instance.GetCurrentRegionLevel();
        currentRegion = regionsDataList[currentRegionLevel - 1];

        environmentModel = GameObject.Instantiate(currentRegion.EnvironmentModel);
    }

    private void InitializeEnemy()
    {
        HO_EnemyDataSO data;

        if (HO_ProgressTracker.Instance.IsBossRoom())
        {
            data = currentRegion.BossEnemy;
        }
        else
        {
            int randIndex = Random.Range(0, currentRegion.EnemiesList.Count);
            data = currentRegion.EnemiesList[randIndex];
        }

        GameObject enemy = GameObject.Instantiate(data.Model);
        enemyScript = enemy.AddComponent<HO_EnemyAI>();
        enemyScript.SetEnemyData(data);

        enemyScript.transform.position = enemyBattlePos;
        enemyScript.transform.Rotate(new Vector3(0, 180, 0));
    }

    private IEnumerator PlayerEnterAnimation()
    {
        playerScript.EnterRoom();
        playerScript.transform.DOMove(playerBattlePos, 3f);

        yield return new WaitForSeconds(3f);

        playerScript.StopMove();
        StartCoroutine(SimulateBattle());
    }

    private IEnumerator SimulateBattle()
    {
        bool isPlayersTurn = true;

        while (!isBattleSimulationFinished)
        {
            HO_EntityAI entityWithTurn = isPlayersTurn ? playerScript : enemyScript;
            HO_EntityAI opposingEntity = isPlayersTurn ? enemyScript : playerScript;

            Vector3 startPos = isPlayersTurn ? playerBattlePos : enemyBattlePos;
            Vector3 attackPos = isPlayersTurn ? enemyBattlePos - distanceOffset : playerBattlePos + distanceOffset;


            entityWithTurn.OnEntityTurn();
            entityWithTurn.TriggerAttackAnimation();
            entityWithTurn.transform.DOJump(attackPos, 1, 1, 0.5f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1f);


            opposingEntity.EntityTakeDamage(entityWithTurn.GetCurrentAttackDamage(), entityWithTurn.IsAttackElemental());
            if (opposingEntity.IsEntityKilled())
            {
                opposingEntity.TriggerDeathAnimation();
                isBattleSimulationFinished = true;
                didPlayerWin = playerScript == entityWithTurn;
            }
            else
            {
                opposingEntity.TriggerHurtAnimation();
            }

            entityWithTurn.transform.DOJump(startPos, 1, 1, 0.5f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1f);

            isPlayersTurn = !isPlayersTurn;
        }

        yield return new WaitForSeconds(2f);

        ReturnToCraftingScene();
    }

    private void ReturnToCraftingScene()
    {
        if (didPlayerWin)
        {
            HO_ProgressTracker.Instance.RoomHasBeenDefeated();
        }

        SceneManager.LoadScene("CraftingScene");
    }
}
