using UnityEngine;

public class HO_GameFlow_old : BaseGameFlow
{
    #region singleton
    public static HO_GameFlow_old Instance { get; private set; }

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
    [SerializeField] private HO_CombatManager_old combatManager;
    [SerializeField] private HO_UIManager_old uiManager;
    [SerializeField] private JournalManager_old journal;
    private int selectedEnemyIndex;


    public int SelectedEnemyIndex 
    { 
        get
        {
            return selectedEnemyIndex;
        }

        private set
        {
            selectedEnemyIndex = value;
        }
    }

    protected override void Start()
    {
        base.Start();
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
        selectedEnemy = enemy.gameObject;
        selectedEnemyIndex = combatManager.enemyList.IndexOf(enemy);
        journal.UpdateJournalPage();
    }

    public void OnReselectEnemy(Enemy enemy)
    {
        if (combatManager.currentEnemy == enemy)
        {
            return;
        }

        OnSelectedEnemy(enemy);
    }

    protected override void AddEnemyToCombatManager(GameObject clone)
    {
        //base.AddEnemyToCombatManager(clone);
        combatManager.enemyList.Add(clone.GetComponent<Enemy>());
    }

    protected override void OnStartCombatState()
    {
        base.OnStartCombatState();

        OnSelectedEnemy(combatManager.enemyList[0]);

        if (combatManager.currentEnemy != null)
        {
            combatManager.StartRound();
        }

        journal.UpdateJournalPage();
    }

    protected override void OnResetRoom()
    {
        base.OnResetRoom();
        combatManager.ResetCombatManager();
    }

    protected override void OnEndGame()
    {
        journal.SaveJournalData();
    }
}
