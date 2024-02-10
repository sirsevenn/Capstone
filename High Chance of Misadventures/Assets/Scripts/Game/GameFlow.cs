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
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform exitTransform;
    [SerializeField] private AnimationHandler playerAnimationHandler;

    [Space(10)]
    [SerializeField] private ActionType playerAction;

    [Header("Camera")]
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;

    [Header("Fog Settings")]
    [SerializeField] private float nearFog;
    [SerializeField] private float farFog;
    [SerializeField] private float fogStartDuration;
    [SerializeField] private float fogExitDuration;

    //Delegate
    public delegate void DelayDelegate();

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

    private void StartFogEffect(float value)
    {
        RenderSettings.fogDensity = value;
    }

    public void StartRoom()
    {

        Debug.Log("Start Room");
        DOVirtual.Float(nearFog, farFog, fogStartDuration, StartFogEffect);
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

        camera1.Priority = 0;
        camera2.Priority = 1;

        playerAnimationHandler.ToggleMove();
        player.transform.DOMove(exitTransform.position, 20).OnComplete(playerAnimationHandler.ToggleMove);

        StartCoroutine(DelayFunction(ResetRoom, 1.0f));

        //Turn off combat
        //Exit Room
        //Play Animation
    }

    public IEnumerator DelayFunction(DelayDelegate function, float delay)
    {
        yield return new WaitForSeconds(delay);
        function();
    }

    public IEnumerator WaitForPlayerInput(DelayDelegate function)
    {
        bool inputReceived = false;
        while (!inputReceived)
        {
            if (Input.GetMouseButtonDown(0))
            {
                inputReceived = true;
            }
            yield return null; // Wait for the next frame
        }

        // Code continues here after input is received
        function();
    }

    public void ResetRoom()
    {
        DOVirtual.Float(farFog, nearFog, fogExitDuration, StartFogEffect);
        ObjectPool.Instance.ResetObjectPools();
        combatManager.ResetCombatManager();

        StartCoroutine(DelayFunction(PlayerReset, 3.0f));

        StartCoroutine(DelayFunction(StartRoom, 5.0f));

    }

    private void PlayerReset()
    {
        player.transform.DOKill();
        player.transform.position = startTransform.position;
    }

   

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

        combatManager.StartComabat(playerAction);
        playerAction = ActionType.None;
    }
}
