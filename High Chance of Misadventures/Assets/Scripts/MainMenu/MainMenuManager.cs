using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainMenuCamera;
    [SerializeField] private CinemachineVirtualCamera levelSelectCamera;

    [Header("UIs")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject levelSelectUI;

    [Header("Levels")]
    [SerializeField] private bool[] LevelUnlocked;
    [SerializeField] private LevelSelectButton[] levelButtons;

    private void Start()
    {
        mainMenuUI.SetActive(true);
        levelSelectUI.SetActive(false);

        for (int i = 0; i < LevelUnlocked.Length; i++)
        {
            if (LevelUnlocked[i] == true)
            {
                levelButtons[i].UnlockLevel();
            }
            else if (LevelUnlocked[i] == false)
            {
                levelButtons[i].LockLevel();
            }
        }
    }

    public void OnPlay()
    {
        //Go to level Select
        mainMenuCamera.Priority = 0;
        levelSelectCamera.Priority = 1;

        StartCoroutine(GameUtilities.DelayFunction(ShowLevelSelectUI, 2));
    }

    public void OnOptions()
    {
        // Open Options
     
    }

    public void onExit()
    {
        //Exit Game
        Application.Quit();
    }

    public void BacktoMainMenu()
    {
        //Go Back to Main Menu
        mainMenuCamera.Priority = 1;
        levelSelectCamera.Priority = 0;

        levelSelectUI.SetActive(false);
        StartCoroutine(GameUtilities.DelayFunction(ShowMainMenuUI, 2));
    }

    private void ShowMainMenuUI()
    {
        mainMenuUI.SetActive(true);
    }

    private void ShowLevelSelectUI()
    {
        mainMenuUI.SetActive(false);
        levelSelectUI.SetActive(true);
    }

    public void LoadTestLevel()
    {
        SceneManager.LoadScene(1);
    }

}
