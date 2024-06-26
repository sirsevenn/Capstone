using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LO_GameFlow_PVP : BaseGameFlow
{

    #region singleton
    public static LO_GameFlow_PVP Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;

    }
    #endregion

    [Header("Spinners")]
    [SerializeField] private Spinner enemySpinner;

    [Header("Other Components")]
    public LO_CombatManager_PVP combatManager;
    public LO_UIManager_PVP uiManager;
    public SideBarManager sideBar;

    // Start is called before the first frame update
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
            selectedEnemy = null;
            combatManager.currentEnemy.DeselectEnemy();
        }

        combatManager.currentEnemy = enemy;
        combatManager.currentEnemy.SelectEnemy();

        if (combatManager.currentEnemy != null)
        {
            enemySpinner.ChangeWheel(combatManager.currentEnemy.GetEnemyData());
        }
        
    }

    protected override void AddEnemyToCombatManager(GameObject clone)
    {
        base.AddEnemyToCombatManager(clone);
        combatManager.enemyList.Add(clone.GetComponent<Enemy>());
    }

    public void SwitchPieces()
    {
        if (!combatManager.readyCombat)
        {
            return;
        }

        uiManager.ChangeSlice();
    }

    protected override void OnStartCombatState()
    {
        base.OnStartCombatState();
        StartCoroutine(GameUtilities.DelayFunction(uiManager.ActivatePlayerUI, 2.0f));
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

    protected override void OnEndGame()
    {
        base.OnEndGame();
        LO_UIManager_PVP.Instance.EndGame(true);

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        string level = "Level" + sceneIndex;
        Debug.Log(level);
        PlayerPrefs.SetInt(level, 1);
        PlayerPrefs.Save();

    }


}
