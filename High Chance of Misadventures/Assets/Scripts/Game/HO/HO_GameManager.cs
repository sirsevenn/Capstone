using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HO_GameManager : MonoBehaviour
{
    [Header("Level Properties")]
    [SerializeField] private List<HO_LevelSO> levelsList;
    [SerializeField] private HO_LevelSO currentLevel;
    [SerializeField] private int currentLevelIndex;
    [SerializeField] private ELevelPhase currentLevelPhase;

    [Space(10)] [Header("Scene Properties")]
    [SerializeField] private string battleSceneName;
    [SerializeField] private string craftingSceneName;

    [Space(10)] [Header("Progress Tracking")]
    [SerializeField] private int currentPlayerLives;
    [SerializeField] private int maxPlayerLives;


    #region Singleton
    public static HO_GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance != null && Instance == this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private void Start()
    {
        currentLevelIndex = 0;
        currentLevelPhase = ELevelPhase.Cutscene;
        currentPlayerLives = maxPlayerLives;
        currentLevel = levelsList[currentLevelIndex];
    }

    public HO_LevelSO GetCurrentLevel()
    {
        return levelsList[currentLevelIndex];
    }

    public ELevelPhase GetCurrenetLevelPhase()
    {
        return currentLevelPhase;
    }

    public bool IsLastLevel()
    {
        return currentLevelIndex == levelsList.Count - 1;
    }

    public bool DecreasePlayerLife()
    {
        currentPlayerLives--;
        return (currentPlayerLives == 0);
    }

    public void TransitionToBattleScene()
    {
        if (currentLevelPhase == ELevelPhase.Crafting)
        {
            currentLevelPhase++;
        }
        else if (currentLevelPhase == ELevelPhase.Battle)
        {
            currentLevelIndex++;
            currentLevel = levelsList[currentLevelIndex];
            currentLevelPhase = ELevelPhase.Cutscene;
            currentPlayerLives = maxPlayerLives;
        }

        SceneManager.LoadScene(battleSceneName);
    }

    public void TransitionToCraftingScene()
    {
        if (currentLevelPhase == ELevelPhase.Cutscene)
        {
            currentLevelPhase++;
        }
        else if (currentLevelPhase == ELevelPhase.Battle)
        {
            currentLevelPhase--;
        }

        SceneManager.LoadScene(craftingSceneName);
    }
}