using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class HO_AutoBattleManager : MonoBehaviour
{
    [Header("Entity Properties")]
    [SerializeField] private HO_PlayerAI playerScript;
    [SerializeField] private HO_EnemyAI enemyScript;

    [Space(10)]
    [SerializeField] private Vector3 playerBattlePos;
    [SerializeField] private Vector3 enemyBattlePos;
    [SerializeField] private float meleeDistanceOffset;

    [Space(10)] [Header("Cutscene Properties")]
    [SerializeField] private PlayableDirector cutsceneDirector;
    [SerializeField] private CinemachineVirtualCamera dollyCamera;
    [SerializeField] private int currentSpeechIndex;

    [Space(10)] [Header("Other Properties")]
    [SerializeField] private HO_AutoBattleUI UI;
    [Space(10)]
    [SerializeField] private HO_LevelSO currentLevel;
    [SerializeField] private GameObject environmentModel;
    [Space(10)]
    [SerializeField] private bool isBattleSimulationFinished;
    [SerializeField] private bool didPlayerWin;


    private void Awake()
    {
        dollyCamera.enabled = false;
        cutsceneDirector.enabled = false;
    }

    private void Start()
    {
        // Determine the current level and its phase
        currentLevel = HO_GameManager.Instance.GetCurrentLevel();
        ELevelPhase currentPhase = HO_GameManager.Instance.GetCurrenetLevelPhase();

        // Initialize environment and enemy
        environmentModel = GameObject.Instantiate(currentLevel.Environment, Vector3.zero, Quaternion.identity);
        InitializeEnemy(currentLevel);

        // Initialize character stats
        playerScript.CopyStats(currentLevel.PlayerStats);
        enemyScript.CopyStats(currentLevel.EnemyStats);

        // Set default values
        currentSpeechIndex = 0;
        isBattleSimulationFinished = false;
        didPlayerWin = false;

        // Check phase to determine what happens next
        if (currentPhase == ELevelPhase.Cutscene)
        {
            UI.DisableUI();
            InitializeInventory();

            playerScript.transform.position = playerBattlePos;
            dollyCamera.enabled = true;
            cutsceneDirector.enabled = true;
        }
        else if (currentPhase == ELevelPhase.Battle)
        {
            StartCoroutine(PlayerEnterAnimation());
        }
    }

    private void InitializeEnemy(HO_LevelSO currentLevel)
    {
        // Create Enemy
        HO_EnemyDataSO data = currentLevel.EnemyData;

        GameObject enemy = GameObject.Instantiate(data.Model);
        enemyScript = enemy.AddComponent<HO_EnemyAI>();
        enemyScript.SetEnemyData(data);

        enemyScript.transform.position = enemyBattlePos;
        enemyScript.transform.Rotate(new Vector3(0, 180, 0));
    }

    private void InitializeInventory()
    {
        InventorySystem.Instance.ResetInventory();
        currentLevel.AddMaterialsToInventory();
    }

    #region Cutscene Methods
    public void OnUpdateSpeechBubble()
    {
        UI.UpdateSpeechBubble(currentLevel.SpeechList[currentSpeechIndex]);
        currentSpeechIndex++;
    }

    public void OnCutsceneEnd()
    {
        HO_GameManager.Instance.TransitionToCraftingScene();
    }
    #endregion

    #region Battle Logic
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
            Vector3 attackPos = isPlayersTurn ? enemyBattlePos : playerBattlePos;


            entityWithTurn.OnEntityTurn();
            entityWithTurn.TriggerAttackAnimation(attackPos, meleeDistanceOffset, 0.75f);
            yield return new WaitForSeconds(0.75f);

            opposingEntity.EntityTakeDamage(entityWithTurn.GetCurrentAttackDamage(), entityWithTurn.GetAttackElementalType());
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
            yield return new WaitForSeconds(0.1f);

            entityWithTurn.TriggerEndAttackAnimation(startPos, 0.75f);
            yield return new WaitForSeconds(0.75f);

            isPlayersTurn = !isPlayersTurn;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2f);

        if (didPlayerWin)
        {
            if (HO_GameManager.Instance.IsLastLevel())
            {
                // game over
            }
            else
            {
                HO_GameManager.Instance.TransitionToBattleScene();
            }
        }
        else
        {
            bool isGameOver = HO_GameManager.Instance.DecreasePlayerLife();

            if (isGameOver)
            {
                // game over
            }
            else
            {
                HO_GameManager.Instance.TransitionToCraftingScene();
                InitializeInventory();
            }
        }
    }
    #endregion
}