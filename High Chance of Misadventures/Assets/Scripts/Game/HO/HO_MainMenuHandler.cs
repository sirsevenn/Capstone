using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HO_MainMenuHandler : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] private GameObject levelSelectionMenu;
    [SerializeField] private List<GameObject> levelPanelsList;

    [Header("Settings")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject secretOption;
    [SerializeField] private GameObject confirmatoryMenu;

    [Space(10)]
    [SerializeField] private RectTransform secretInputRectTranform;
    [SerializeField] private List<SwipeEventArgs.SwipeDirections> swipesCheatList;
    [SerializeField] private int correctSwipesNum;
 

    private void Start()
    {
        OnReturnToMainMenu();
        UpdateLevelSelectMenu();

        secretOption.SetActive(false);
        confirmatoryMenu.SetActive(false);

        correctSwipesNum = 0;
        GestureManager.Instance.OnSwipeEvent += OnSwipe;
    }

    private void OnDestroy()
    {
        GestureManager.Instance.OnSwipeEvent -= OnSwipe;
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
        levelSelectionMenu.SetActive(true);
    }

    public void OnSettingsButton()
    {
        settingsMenu.SetActive(true);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnReturnToMainMenu()
    {
        levelSelectionMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void OnLevelSelect(int levelID)
    {
        HO_GameManager.Instance.StartLevel(levelID);
    }

    public void OnDeletePlayerData()
    {
        confirmatoryMenu.SetActive(true);
    }

    public void OnConfirmAction()
    {
        HO_GameManager.Instance.ResetPlayerData();
        UpdateLevelSelectMenu();

        confirmatoryMenu.SetActive(false);
    }

    public void OnDeclineAction()
    {
        confirmatoryMenu.SetActive(false);
    }

    private void OnSwipe(object send, SwipeEventArgs args)
    {
        if (secretOption.activeSelf) return;

        if (!RectTransformUtility.RectangleContainsScreenPoint(secretInputRectTranform, args.SwipePos)) return;

        //Debug.Log(args.SwipeDirection);

        if (args.SwipeDirection == swipesCheatList[correctSwipesNum])
        {
            correctSwipesNum++;
            if (correctSwipesNum == swipesCheatList.Count)
            {
                secretOption.SetActive(true);
            }
        }
        else
        {
            correctSwipesNum = (args.SwipeDirection == swipesCheatList.First()) ? 1 : 0;
        }
    }

    public void OnUnlockAllLevels(bool isUnlocked)
    {
        HO_GameManager.Instance.SetUnlockLevelsCheat(isUnlocked);
        UpdateLevelSelectMenu();
    }
}