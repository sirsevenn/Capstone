using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HO_MainMenuHandler : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] private GameObject levelSelectionMenu;
    [SerializeField] private GameObject levelPanelPrefab;
    [SerializeField] private RectTransform levelSelectionWindowParent;
    [SerializeField] private List<GameObject> levelPanelsList;

    [Header("Settings")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject unlockLevelsOption;
    [SerializeField] private GameObject skipCutsceneOption;
    [SerializeField] private GameObject confirmatoryMenu;

    [Space(10)]
    [SerializeField] private RectTransform secretInputRectTranform;
    [SerializeField] private List<SwipeEventArgs.SwipeDirections> swipesCheatList;
    [SerializeField] private int correctSwipesNum;
 

    private void Start()
    { 
        levelSelectionMenu.SetActive(false);
        InitializeLevelPanels();
        UpdateLevelSelectMenu();

        settingsMenu.SetActive(false);
        unlockLevelsOption.SetActive(false);
        skipCutsceneOption.SetActive(false);
        confirmatoryMenu.SetActive(false);

        correctSwipesNum = 0;
        GestureManager.Instance.OnSwipeEvent += OnSwipe;
    }

    private void OnDestroy()
    {
        GestureManager.Instance.OnSwipeEvent -= OnSwipe;
    }

    private void InitializeLevelPanels()
    {
        levelPanelsList = new();
        foreach (var levelData in HO_GameManager.Instance.GetAllLevels())
        {
            GameObject levelPanel = GameObject.Instantiate(levelPanelPrefab, levelSelectionWindowParent);
            levelPanel.GetComponent<HO_LevelPanel>().InitiailizeLevelPanel(levelData, OnLevelSelect);
            levelPanelsList.Add(levelPanel);
        }
    }

    private void UpdateLevelSelectMenu()
    {
        uint highestLevelID = HO_GameManager.Instance.GetHighestUnlockedLevelID();
        for (int i = 0; i < levelPanelsList.Count; i++)
        {
            levelPanelsList[i].SetActive(i < highestLevelID);
        }
    }

    public void OnPlayButton()
    {
        SoundEffectManager.Instance.PlayClick();
        levelSelectionMenu.SetActive(true);
    }

    public void OnSettingsButton()
    {
        SoundEffectManager.Instance.PlayClick();
        settingsMenu.SetActive(true);
    }

    public void OnQuitButton()
    {
        SoundEffectManager.Instance.PlayClick();
        Application.Quit();
    }

    public void OnReturnToMainMenu()
    {
        SoundEffectManager.Instance.PlayClick();
        levelSelectionMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    private void OnLevelSelect(int levelID)
    {
        HO_GameManager.Instance.StartLevel(levelID);
    }

    public void OnDeletePlayerData()
    {
        SoundEffectManager.Instance.PlayClick();
        confirmatoryMenu.SetActive(true);
    }

    public void OnConfirmAction()
    {
        SoundEffectManager.Instance.PlayClick();
        HO_GameManager.Instance.ResetPlayerData();
        UpdateLevelSelectMenu();

        confirmatoryMenu.SetActive(false);
    }

    public void OnDeclineAction()
    {
        SoundEffectManager.Instance.PlayClick();
        confirmatoryMenu.SetActive(false);
    }

    private void OnSwipe(object send, SwipeEventArgs args)
    {
        if (unlockLevelsOption.activeSelf && skipCutsceneOption.activeSelf) return;

        if (!RectTransformUtility.RectangleContainsScreenPoint(secretInputRectTranform, args.SwipePos)) return;

        //Debug.Log(args.SwipeDirection);

        CheckCheatInput(args.SwipeDirection);
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CheckCheatInput(SwipeEventArgs.SwipeDirections.UP);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CheckCheatInput(SwipeEventArgs.SwipeDirections.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CheckCheatInput(SwipeEventArgs.SwipeDirections.DOWN);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CheckCheatInput(SwipeEventArgs.SwipeDirections.RIGHT);
        }
    }

    private void CheckCheatInput(SwipeEventArgs.SwipeDirections dir)
    {
        if (dir == swipesCheatList[correctSwipesNum])
        {
            correctSwipesNum++;
            if (correctSwipesNum == swipesCheatList.Count)
            {
                unlockLevelsOption.SetActive(true);
                skipCutsceneOption.SetActive(true);
            }
        }
        else
        {
            correctSwipesNum = (dir == swipesCheatList.First()) ? 1 : 0;
        }
    }

    public void OnUnlockAllLevels(bool isUnlocked)
    {
        HO_GameManager.Instance.SetUnlockLevelsCheat(isUnlocked);
        UpdateLevelSelectMenu();
    }

    public void OnSkipCutscenes(bool willSkip)
    {
        HO_GameManager.Instance.SetSkipCutscenese(willSkip);
    }
}