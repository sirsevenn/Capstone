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
    [SerializeField] private float turnDuration;

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
    [SerializeField] private bool isGameOver;


    #region Initialization
    private void Awake()
    {
        dollyCamera.enabled = false;
        cutsceneDirector.enabled = false;
    }

    private void Start()
    {
        // Initialize environment and enemy
        currentLevel = HO_GameManager.Instance.GetCurrentLevel();
        environmentModel = GameObject.Instantiate(currentLevel.Environment, Vector3.zero, Quaternion.identity);
        InitializeEnemy(currentLevel);

        // Initialize character stats
        playerScript.CopyStats(currentLevel.PlayerStats);
        enemyScript.CopyStats(currentLevel.EnemyStats);

        // Set default values
        currentSpeechIndex = 0;
        isBattleSimulationFinished = false;
        didPlayerWin = false;
        turnDuration = (turnDuration == 0) ? 0.55f : turnDuration;

        // Check phase to determine what happens next
        ELevelPhase currentPhase = HO_GameManager.Instance.GetCurrenetLevelPhase();

        if (currentPhase == ELevelPhase.Cutscene)
        {
            UI.DisableUI();
            InventorySystem.Instance.ResetConsumables();
            currentLevel.AddMaterialsToInventory();

            playerScript.transform.position = playerBattlePos;
            dollyCamera.enabled = true;
            cutsceneDirector.enabled = true;
        }
        else if (currentPhase == ELevelPhase.Battle)
        {
            StartCoroutine(PlayerEnterAnimation());
        }

        // Subscription to some events
        InventorySystem.Instance.OnUpdateConsumablesEvent += OnUpdateConsumables;
    }

    private void OnDestroy()
    {
        InventorySystem.Instance.OnUpdateConsumablesEvent -= OnUpdateConsumables;
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
    #endregion

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

            float halfTurnDuration = turnDuration / 2f;


            entityWithTurn.OnEntityTurn(opposingEntity.WeakToElement, opposingEntity.ResistantToElement);
            entityWithTurn.TriggerAttackAnimation(attackPos, meleeDistanceOffset, halfTurnDuration);
            yield return new WaitForSeconds(halfTurnDuration);

            
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


            entityWithTurn.TriggerEndAttackAnimation(startPos, halfTurnDuration);
            yield return new WaitForSeconds(halfTurnDuration);


            isPlayersTurn = !isPlayersTurn;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2f);


        isGameOver = HO_GameManager.Instance.DecreasePlayerLife();

        if (didPlayerWin || isGameOver)
        {
            UI.EnableEndLevelPanel(didPlayerWin, HO_GameManager.Instance.IsLastLevel());
        }
        else // if lost but not yet game over
        {
            HO_GameManager.Instance.TransitionToCraftingScene();
            InventorySystem.Instance.ResetConsumables();
        }
    }

    private void OnUpdateConsumables(Consumable consumable, bool hasAddedNewConsumable) => UI.OnUpdateConsumables(consumable, hasAddedNewConsumable, turnDuration);
    #endregion

    #region End Level Methods
    public void TransitionToNextLevel()
    {
        if ((didPlayerWin && !HO_GameManager.Instance.IsLastLevel()) ||  // player wins but hasnt beaten the game yet
            (!didPlayerWin && isGameOver))                               // player lost completely
        {
            HO_GameManager.Instance.TransitionToBattleScene();

        }
    }

    public void TransitionToMainMenu()
    {

    }
    #endregion
}