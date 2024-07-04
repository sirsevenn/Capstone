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
    [SerializeField] private GameObject normalButtons;
    [SerializeField] private GameObject optionsPanel;

    [Header("Levels")]
    [SerializeField] private int[] LevelUnlocked;
    [SerializeField] private LevelSelectButton[] levelButtons;

    

    private void Start()
    {
        int levelSelect = PlayerPrefs.GetInt("LevelSelect", 0);
        if (levelSelect == 1)
        {
            Debug.Log("From Level");
            mainMenuUI.SetActive(false);
            levelSelectUI.SetActive(true);
            mainMenuCamera.Priority = 0;
            levelSelectCamera.Priority = 1;
        }
        else
        {
            Debug.Log("Not from Level");
            mainMenuUI.SetActive(true);
            levelSelectUI.SetActive(false);
            mainMenuCamera.Priority = 1;
            levelSelectCamera.Priority = 0;
        }

        PlayerPrefs.SetInt("LevelSelect", 0);
        PlayerPrefs.Save();

        InitializeLevels();
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
        if (optionsPanel.activeSelf)
        {
            optionsPanel.SetActive(false);
            normalButtons.SetActive(true);
        }
        else
        {
            optionsPanel.SetActive(true);
            normalButtons.SetActive(false);
        }
     
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

    public void LoadLevel(int index)
    {
        PlayerPrefs.SetInt("CurrentLevel", index);
        SceneManager.LoadScene(1);
    }

    public void ResetLevels()
    {
        PlayerPrefs.SetInt("Level1", 1);
        PlayerPrefs.SetInt("Level2", 0);
        PlayerPrefs.SetInt("Level3", 0);
        PlayerPrefs.SetInt("Level4", 0);
        PlayerPrefs.SetInt("Level5", 0);
        PlayerPrefs.SetInt("Level6", 0);

        PlayerPrefs.Save();

        InitializeLevels();
    }

    private void InitializeLevels()
    {
        LevelUnlocked[0] = PlayerPrefs.GetInt("Level1", 1);
        LevelUnlocked[1] = PlayerPrefs.GetInt("Level2", 0);
        LevelUnlocked[2] = PlayerPrefs.GetInt("Level3", 0);
        LevelUnlocked[3] = PlayerPrefs.GetInt("Level4", 0);
        LevelUnlocked[4] = PlayerPrefs.GetInt("Level5", 0);
        LevelUnlocked[5] = PlayerPrefs.GetInt("Level6", 0);

        for (int i = 0; i < LevelUnlocked.Length; i++)
        {
            if (LevelUnlocked[i] == 1)
            {
                levelButtons[i].UnlockLevel();
            }
            else if (LevelUnlocked[i] == 0)
            {
                levelButtons[i].LockLevel();
            }
        }
    }

    public void UnlockLevels()
    {

        PlayerPrefs.SetInt("Level1", 1);
        PlayerPrefs.SetInt("Level2", 1);
        PlayerPrefs.SetInt("Level3", 1);
        PlayerPrefs.SetInt("Level4", 1);
        PlayerPrefs.SetInt("Level5", 1);
        PlayerPrefs.SetInt("Level6", 1);

        PlayerPrefs.Save();

        LevelUnlocked[0] = PlayerPrefs.GetInt("Level1", 1);
        LevelUnlocked[1] = PlayerPrefs.GetInt("Level2", 0);
        LevelUnlocked[2] = PlayerPrefs.GetInt("Level3", 0);
        LevelUnlocked[3] = PlayerPrefs.GetInt("Level4", 0);
        LevelUnlocked[4] = PlayerPrefs.GetInt("Level5", 0);
        LevelUnlocked[5] = PlayerPrefs.GetInt("Level6", 0);

        for (int i = 0; i < LevelUnlocked.Length; i++)
        {
            if (LevelUnlocked[i] == 1)
            {
                levelButtons[i].UnlockLevel();
            }
            else if (LevelUnlocked[i] == 0)
            {
                levelButtons[i].LockLevel();
            }
        }
    }

}
