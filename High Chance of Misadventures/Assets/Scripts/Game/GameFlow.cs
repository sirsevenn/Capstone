using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class GameFlow : MonoBehaviour
{

    #region singleton
    public static GameFlow Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;

    }
    #endregion

    [Header("Spawn Points per Room")]
    public int maxEnemyCountPerRoom = 5;

    [Header("Room Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Other Components")]
    public CombatManager combatManager;

    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform exitTransform;
    [SerializeField] private AnimationHandler playerAnimationHandler;

    [Space(10)]
    [SerializeField] private ActionType playerAction;

    [Header("Camera")]
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;

    enum GameState
    {
        Start,
        Combat,
        Exit
    }

    [SerializeField]
    private GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        StartRoom();
    }

    public void StartRoom()
    {
        gameState = GameState.Start;

        camera1.Priority = 0;
        camera2.Priority = 1;

        int enemyCount = Random.Range(1, maxEnemyCountPerRoom);

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

        
        player.transform.DOMove(playerPosition.position, 2).OnComplete(StartCombat);

        //Player do tween go to start point
        //Play animation
    }

    public void StartCombat()
    {
        gameState = GameState.Combat;

        playerAnimationHandler.ToggleMove();

        camera1.Priority = 1;
        camera2.Priority = 0;
        combatManager.EnemyPreCombat();

        //Turn on combat
    }

    public void EndRoom()
    {
        gameState = GameState.Exit;

        playerAnimationHandler.ToggleMove();

        camera1.Priority = 0;
        camera2.Priority = 1;

        player.transform.DOMove(exitTransform.position, 10).SetDelay(2);

        //Turn off combat
        //Exit Room
        //Play Animation
    }

   

    public void PlayerAction(int type)
    {
        switch (type)
        {
            case 0:
                playerAction = ActionType.Attack;
                break;
            case 1:
                playerAction = ActionType.Defend;
                break;
            case 2:
                playerAction = ActionType.Skill;
                break;
        }

        //Start Combat

        combatManager.StartComabat(playerAction);
        playerAction = ActionType.None;
    }
}
