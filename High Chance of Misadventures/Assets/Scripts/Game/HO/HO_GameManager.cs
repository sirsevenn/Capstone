using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HO_GameManager : MonoBehaviour
{
    [Header("Level Properties")]
    [SerializeField] private List<HO_LevelSO> levelsList;
    [SerializeField] private HO_LevelSO currentLevel;
    [SerializeField] private int currentLevelIndex;
    [SerializeField] private ELevelPhase currentLevelPhase;
    [SerializeField] private uint highestUnlockedLevelID;
    private bool areAllLevelsUnlocked;

    [Space(10)] [Header("Scene Properties")]
    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private string craftingSceneName;

    [Space(10)] [Header("Progress Tracking")]
    [SerializeField] private int currentPlayerLives;
    [SerializeField] private int maxPlayerLives;
    [SerializeField] private bool hasFinishedTutorial;

    [Space(10)] [Header("Player Data")]
    [SerializeField] private string highLevelIDKey;
    [SerializeField] private string tutorialKey;


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
            InitializeGameManager();
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

    public void InitializeGameManager()
    {
        currentLevelIndex = 0;
        currentLevelPhase = ELevelPhase.Unknown;
        currentPlayerLives = maxPlayerLives;
        currentLevel = levelsList[currentLevelIndex];
        highestUnlockedLevelID = (uint)PlayerPrefs.GetInt(highLevelIDKey);
        highestUnlockedLevelID = (highestUnlockedLevelID == 0) ? levelsList.First().LevelID : highestUnlockedLevelID;

        areAllLevelsUnlocked = false;
        hasFinishedTutorial = (PlayerPrefs.GetInt(tutorialKey) == 1);
    }

    public HO_LevelSO GetCurrentLevel()
    {
        return currentLevel;
    }

    public uint GetCurrentLevelID()
    {
        return currentLevel.LevelID;
    }

    public ELevelPhase GetCurrentLevelPhase()
    {
        return currentLevelPhase;
    }

    public uint GetHighestUnlockedLevelID()
    {
        return (areAllLevelsUnlocked) ? levelsList.Last().LevelID : highestUnlockedLevelID;
    }

    public bool IsLastLevel()
    {
        return currentLevelIndex == levelsList.Count - 1;
    }

    public void SetUnlockLevelsCheat(bool isUnlocked)
    {
        areAllLevelsUnlocked = isUnlocked;
    }

    public void StartLevel(int levelID)
    {
        if (levelID > highestUnlockedLevelID && !areAllLevelsUnlocked) return;

        currentLevelIndex = levelID - 1;
        currentLevelPhase = ELevelPhase.Cutscene;
        currentPlayerLives = maxPlayerLives;
        currentLevel = levelsList[currentLevelIndex];
        SceneManager.LoadScene(currentLevel.RegionName);
    }

    public bool DecreasePlayerLife()
    {
        currentPlayerLives--;
        return (currentPlayerLives == 0);
    }

    public bool HasFinishedTutorial()
    {
        return (hasFinishedTutorial || areAllLevelsUnlocked);
    }

    public void OnFinishedTutorial()
    {
        hasFinishedTutorial = true;
        PlayerPrefs.SetInt(tutorialKey, 1);
    }

    public void TransitionToBattleScene()
    {
        if (currentLevelPhase == ELevelPhase.Unknown) return;

        if (currentLevelPhase == ELevelPhase.Battle)
        {
            if (currentPlayerLives != 0)
            {
                currentLevelIndex++;
                currentLevel = levelsList[currentLevelIndex];
                highestUnlockedLevelID = (currentLevel.LevelID > highestUnlockedLevelID) ? currentLevel.LevelID : highestUnlockedLevelID;
            }

            currentLevelPhase = ELevelPhase.Cutscene;
            currentPlayerLives = maxPlayerLives;
        }
        else if (currentLevelPhase == ELevelPhase.Crafting)
        {
            currentLevelPhase++;
        }

        SceneManager.LoadScene(currentLevel.RegionName);
    }

    public void TransitionToCraftingScene()
    {
        if (currentLevelPhase == ELevelPhase.Unknown) return;

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

    public void TransitionToMainMenuScene()
    {
        currentLevelPhase = ELevelPhase.Unknown;
        currentPlayerLives = maxPlayerLives;
        SceneManager.LoadScene(mainMenuSceneName);
        PlayerPrefs.SetInt(highLevelIDKey, (int)highestUnlockedLevelID);
    }

    public void ResetPlayerData()
    {
        currentLevelIndex = 0;
        highestUnlockedLevelID = levelsList.First().LevelID;
        currentLevel = levelsList[currentLevelIndex];
        hasFinishedTutorial = false;

        PlayerPrefs.SetInt(highLevelIDKey, (int)highestUnlockedLevelID);
        PlayerPrefs.SetInt(tutorialKey, 0);
    }

    public void ExitApplication()
    {
        PlayerPrefs.SetInt(highLevelIDKey, (int)highestUnlockedLevelID);
        PlayerPrefs.SetInt(tutorialKey, hasFinishedTutorial ? 1 : 0);
        Application.Quit();
    }
}