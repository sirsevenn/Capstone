using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.Playables;

public class LO_GameFlow : MonoBehaviour
{

    #region singleton
    public static LO_GameFlow Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;

    }
    #endregion
    enum GameState
    {
        Start,
        Combat,
        Exit
    }

    [Header("Game Settings")]
    [SerializeField] private GameType gameType;
    public LayerMask enemyMask;

    [Header("Spawn Points per Room")]
    public int maxEnemyCountPerRoom = 5;

    [Header("Room Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Other Components")]
    public LO_CombatManager combatManager;
    public LO_UIManager uiManager;

    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform exitTransform;
    [SerializeField] private AnimationHandler playerAnimationHandler;
    [SerializeField] private PlayerManager playerManager;

    [Header("Camera")]
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;

    [Header("Fog Settings")]
    [SerializeField] private float nearFog;
    [SerializeField] private float farFog;
    [SerializeField] private float fogStartDuration;
    [SerializeField] private float fogExitDuration;

    [SerializeField]
    private GameState gameState;

    [Header("Spinners")]
    [SerializeField] private Spinner enemySpinner;

    // Start is called before the first frame update
    void Start()
    {
        StartRoom();
    }

    private void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            
            // Perform a raycast from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits any collider
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyMask))
            {
                // You can do something with the hit information here
                Debug.Log("Hit Object: " + hit.transform.name);
                Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();

                if (combatManager.currentEnemy != null)
                {
                    combatManager.currentEnemy.DeselectEnemy();
                }

                combatManager.currentEnemy = enemy;
                combatManager.currentEnemy.SelectEnemy();
                enemySpinner.ChangeWheel(enemy.enemyProbability);

            }

        }

    }

    private void StartFogEffect(float value)
    {
        RenderSettings.fogDensity = value;
    }

    public void StartRoom()
    {

        Debug.Log("Start Room");
        gameState = GameState.Start;

        DOVirtual.Float(nearFog, farFog, fogStartDuration, StartFogEffect);

        int enemyCount = Random.Range(1, maxEnemyCountPerRoom + 1);

        for (int i = 0; i < enemyCount; i++)
        {
            //decide which enemies to spawn

            int index = -1;
            for (int j = 0; j < spawnPoints.Count; j++)
            {
                if (spawnPoints[j].childCount == 0 && index == -1)
                {
                    index = j;
                }
            }

            GameObject clone = ObjectPool.Instance.GetObject(EnemyType.Goblin, spawnPoints[i].transform);
            combatManager.enemyList.Add(clone.GetComponent<Enemy>());

        }

        player.transform.DOMove(playerPosition.position, 2).OnComplete(StartCombatState);

    }

    public void StartCombatState()
    {
        gameState = GameState.Combat;

        StartCoroutine(GameUtilities.DelayFunction(uiManager.ActivatePlayerUI, 2.0f));

        playerAnimationHandler.ToggleMove();

        camera1.Priority = 1;
        camera2.Priority = 0;
        combatManager.EnemyPreCombat();

        //Turn on combat
    }

    public void EndRoom()
    {
        gameState = GameState.Exit;

        camera1.Priority = 0;
        camera2.Priority = 1;

        playerAnimationHandler.ToggleMove();
        player.transform.DOMove(exitTransform.position, 20).OnComplete(playerAnimationHandler.ToggleMove);

        StartCoroutine(GameUtilities.DelayFunction(ResetRoom, 1.0f));

    }

    

    public void ResetRoom()
    {
        DOVirtual.Float(farFog, nearFog, fogExitDuration, StartFogEffect);
        ObjectPool.Instance.ResetObjectPools();
        combatManager.ResetCombatManager();

        StartCoroutine(GameUtilities.DelayFunction(PlayerReset, 3.0f));
        StartCoroutine(GameUtilities.DelayFunction(StartRoom, 5.0f));

    }

    private void PlayerReset()
    {
        player.transform.DOKill();
        player.transform.position = startTransform.position;
    }


    public void SwitchPieces()
    {
        if (!combatManager.readyCombat)
        {
            return;
        }

        uiManager.ChangeSlice();
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

    public void StartCombat()
    {
        
    }

    //Used in Prototype 1
    /*
    public void PlayerAction(int type)
    {
        switch (type)
        {
            case 0:
                playerAction = ActionType.Heavy;
                break;
            case 1:
                playerAction = ActionType.Parry;
                break;
            case 2:
                playerAction = ActionType.Light;
                break;
        }

        //Start Combat
        combatManager.StartCombat(playerAction);
        playerAction = ActionType.None;
    }
    */
}
