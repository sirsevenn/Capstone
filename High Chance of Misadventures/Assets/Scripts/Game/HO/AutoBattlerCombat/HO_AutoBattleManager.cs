using DG.Tweening;
using System.Collections;
using UnityEngine;

public class HO_AutoBattleManager : MonoBehaviour
{
    [Header("Battle Properties")]
    [SerializeField] private HO_PlayerAI playerScript;
    [SerializeField] private HO_EnemyAI enemyScript;

    [Space(10)]
    [SerializeField] private Vector3 playerBattlePos;
    [SerializeField] private Vector3 enemyBattlePos;
    [SerializeField] private float meleeDistanceOffset;
    [SerializeField] private float turnDuration;

    [Space(10)]
    [SerializeField] private bool isBattleSimulationFinished;
    [SerializeField] private bool didPlayerWin;
    [SerializeField] private bool isGameOver;

    [Space(20)] [Header("Other References")]
    [SerializeField] private HO_AutoBattleUI UI;
    [SerializeField] private HO_CutsceneManager cutsceneScript;
    [Space(10)]
    [SerializeField] private HO_LevelSO currentLevel;

    private WaitForSeconds halfTurnDurationInSeconds;
    private WaitForSeconds shortDelayInSeconds;
    private WaitForSeconds delayAfterBattleInSeconds;


    #region Initialization
    private void Start()
    {
        // Initialize enemy
        currentLevel = HO_GameManager.Instance.GetCurrentLevel();
        InitializeEnemy(currentLevel);

        // Initialize character stats
        playerScript.CopyStats(currentLevel.PlayerStats);
        enemyScript.CopyStats(currentLevel.EnemyStats);

        // Set default values
        turnDuration = (turnDuration == 0) ? 0.55f : turnDuration;
        isBattleSimulationFinished = false;
        didPlayerWin = false;

        halfTurnDurationInSeconds = new WaitForSeconds(turnDuration / 2f);
        shortDelayInSeconds = new WaitForSeconds(0.1f); 
        delayAfterBattleInSeconds = new WaitForSeconds(2f);

        // Check phase to determine what happens next
        ELevelPhase currentPhase = HO_GameManager.Instance.GetCurrentLevelPhase();

        if (currentPhase == ELevelPhase.Cutscene)
        {
            InventorySystem.Instance.ResetConsumables();
            currentLevel.AddMaterialsToInventory();
            UI.DisableUI();

            if (HO_GameManager.Instance.ShouldSkipCutscenes())
            {
                HO_GameManager.Instance.TransitionToCraftingScene();
            }
            else
            {
                cutsceneScript.EnableCutscene();
                playerScript.transform.position = playerBattlePos;
            }
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

    #region Battle Logic
    private IEnumerator PlayerEnterAnimation()
    {
        playerScript.PlayerMove();
        yield return playerScript.transform.DOMove(playerBattlePos, 3f).WaitForCompletion();

        playerScript.StopMove();
        yield return UI.AnimateAddPotionsToInventory(playerBattlePos);

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


            entityWithTurn.OnEntityTurn(opposingEntity.ElementalEffects);
            entityWithTurn.TriggerAttackAnimation(attackPos, meleeDistanceOffset, halfTurnDuration);
            yield return halfTurnDurationInSeconds;


            bool hasArmorPierce = (entityWithTurn == enemyScript) ? currentLevel.EnemyHasArmorPierce : false;
            opposingEntity.EntityTakeDamage(entityWithTurn.GetCurrentAttackDamage(), entityWithTurn.GetAttackElementalType(), hasArmorPierce);

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
            yield return shortDelayInSeconds;


            entityWithTurn.TriggerEndAttackAnimation(startPos, halfTurnDuration);
            yield return halfTurnDurationInSeconds;


            isPlayersTurn = !isPlayersTurn;
            yield return shortDelayInSeconds;
        }

        yield return delayAfterBattleInSeconds;


        isGameOver = HO_GameManager.Instance.DecreasePlayerLife();

        if (didPlayerWin || isGameOver)
        {
            if (didPlayerWin) HO_GameManager.Instance.TryUnlockNextLevel();

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
            InventorySystem.Instance.ResetInventory();
        }
    }

    public void TransitionToMainMenu()
    {
        HO_GameManager.Instance.TransitionToMainMenuScene();
    }
    #endregion
}