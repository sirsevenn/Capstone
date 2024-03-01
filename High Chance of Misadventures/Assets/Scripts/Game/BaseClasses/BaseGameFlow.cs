using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.Playables;


public class BaseGameFlow : MonoBehaviour
{
    protected enum GameState
    {
        Start,
        Combat,
        Exit
    }
    [Header("Important Entities")]
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject selectedEnemy;

    [Header("Game Settings")]
    [SerializeField] private GameType gameType;
    public LayerMask enemyMask;

    [Header("Spawn Points per Room")]
    public int maxEnemyCountPerRoom = 5;

    [Header("Room Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Player Settings")]
    [SerializeField] protected Transform playerPosition;
    [SerializeField] protected Transform startTransform;
    [SerializeField] protected Transform exitTransform;
    [SerializeField] protected AnimationHandler playerAnimationHandler;

    [Header("Camera")]
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;

    [Header("Fog Settings")]
    [SerializeField] protected float nearFog;
    [SerializeField] protected float farFog;
    [SerializeField] protected float fogStartDuration;
    [SerializeField] protected float fogExitDuration;

    [SerializeField] protected GameState gameState;

    protected virtual void Update()
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
                selectedEnemy = hit.transform.gameObject;
                Enemy enemy = selectedEnemy.GetComponent<Enemy>();
                

                OnSelectedEnemy(enemy);

            }

        }

    }

    protected virtual void OnSelectedEnemy(Enemy enemy)
    {
    }

    protected void StartFogEffect(float value)
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
            AddEnemyToCombatManager(clone);

        }

        player.transform.DOMove(playerPosition.position, 2).OnComplete(StartCombatState);

    }

    protected virtual void AddEnemyToCombatManager(GameObject clone)
    {

    }

    public void StartCombatState()
    {
        gameState = GameState.Combat;

        OnStartCombatState();
       

        playerAnimationHandler.ToggleMove();

        camera1.Priority = 1;
        camera2.Priority = 0;
       

        //Turn on combat
    }

    protected virtual void OnStartCombatState()
    {

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
        OnResetRoom();

        StartCoroutine(GameUtilities.DelayFunction(PlayerReset, 3.0f));
        StartCoroutine(GameUtilities.DelayFunction(StartRoom, 5.0f));

    }

    protected virtual void OnResetRoom()
    {

    }

    private void PlayerReset()
    {
        player.transform.DOKill();
        player.transform.position = startTransform.position;
    }

}
