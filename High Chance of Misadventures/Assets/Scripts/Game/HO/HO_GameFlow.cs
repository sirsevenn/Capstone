using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HO_GameFlow : BaseGameFlow
{
    #region singleton
    public static HO_GameFlow Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;

    }
    #endregion

    [Header("Other Components")]
    public HO_CombatManager combatManager;

    void Start()
    {
        StartRoom();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnSelectedEnemy(Enemy enemy)
    {
        if (combatManager.currentEnemy != null)
        {
            combatManager.currentEnemy.DeselectEnemy();
            combatManager.currentEnemy = null;
        }

        combatManager.currentEnemy = enemy;
        combatManager.currentEnemy.SelectEnemy();

        HO_UIManager.Instance.SetProbabiltyBoard(enemy.data.heavyAttackProbability * 10, enemy.data.lightAttackProbability * 10, enemy.data.parryAttackProbability * 10);

    }

    protected override void AddEnemyToCombatManager(GameObject clone)
    {
        base.AddEnemyToCombatManager(clone);
        combatManager.enemyList.Add(clone.GetComponent<Enemy>());
    }

    protected override void OnStartCombatState()
    {
        base.OnStartCombatState();
        combatManager.EnemyPreCombat();
    }

    protected override void OnResetRoom()
    {
        base.OnResetRoom();
        combatManager.ResetCombatManager();
    }

    public void SetActions(ActionType action, bool isPlayer)
    {
        if (isPlayer)
        {
            combatManager.playerAction = action;
        }
        else if (!isPlayer)
        {
            combatManager.enemyAction = action;
        }

    }
}
