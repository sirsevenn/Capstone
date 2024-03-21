using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Stats")]
    [SerializeField] private int gold;
    [SerializeField] private int helmetLevel;
    [SerializeField] private int cuirassLevel;
    [SerializeField] private int greavesLevel;
    [Space(10)]
    [SerializeField] private int redPieces;
    [SerializeField] private int greenPieces;
    [SerializeField] private int bluePieces;
    [SerializeField] private int healthPotions;

    private static string goldKey = "GOLD";

    private static string helmetKey = "HELMET_LEVEL";
    private static string cuirassKey = "CUIRASS_LEVEL";
    private static string greavesKey = "GREAVES_KEY";

    private static string redKey = "RED_PIECES";
    private static string greenKey = "GREEN_PIECES";
    private static string blueKey = "BLUE_PIECES";
    private static string healthPotionKey = "HEALTH_POTIONS";

    #region singleton
    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
           
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

    }
    #endregion

    #region Getters
    public int Gold { get { return gold; } }
    public int HelmetLevel { get { return helmetLevel; } }
    public int CuirassLevel { get { return cuirassLevel; } }
    public int GreavesLevel { get { return greavesLevel; } }
    public int RedPieces { get { return redPieces; } }
    public int GreenPieces { get { return greenPieces; } }
    public int BluePieces { get { return bluePieces; } }
    public int HealthPotions { get { return healthPotions; } }
    #endregion

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // This function will be called when a scene is loaded
        Debug.Log("Scene loaded: " + scene.name);
        // Call your function here
        InitializeInventory();
    }

    void OnSceneUnloaded(Scene scene)
    {
        // This function will be called when a scene is unloaded
        Debug.Log("Scene unloaded: " + scene.name);
        SaveInventory();
        
    }

    private void InitializeInventory()
    {
        gold = PlayerPrefs.GetInt(goldKey, 100);

        helmetLevel = PlayerPrefs.GetInt(helmetKey, 0);
        cuirassLevel = PlayerPrefs.GetInt(cuirassKey, 0);
        greavesLevel = PlayerPrefs.GetInt(greavesKey, 0);

        redPieces = PlayerPrefs.GetInt(redKey, 10);
        greenPieces = PlayerPrefs.GetInt(greenKey, 10);
        bluePieces = PlayerPrefs.GetInt(blueKey, 10);
        healthPotions = PlayerPrefs.GetInt(healthPotionKey, 0);
    }

    public void SaveInventory()
    {
        PlayerPrefs.SetInt(goldKey, gold);

        PlayerPrefs.SetInt(helmetKey, helmetLevel);
        PlayerPrefs.SetInt(cuirassKey, cuirassLevel);
        PlayerPrefs.SetInt(greavesKey, greavesLevel);

        PlayerPrefs.SetInt(redKey, redPieces);
        PlayerPrefs.SetInt(greenKey, greenPieces);
        PlayerPrefs.SetInt(blueKey, bluePieces);
        PlayerPrefs.SetInt(healthPotionKey, healthPotions);

    }

    public void DeductGold(int value)
    {
        gold = Mathf.Max(gold - value, 0);
    }

    public void AddItem(int index)
    {
        switch (index)
        {
            case 0:
                redPieces++;
                break;
            case 1:
                greenPieces++;
                break;
            case 2:
                bluePieces++;
                break;
            case 3:
                healthPotions++;
                break;
        }
    }

    public void DeductItem(int index)
    {
        switch (index)
        {
            case 0:
                redPieces--;
                break;
            case 1:
                greenPieces--;
                break;
            case 2:
                bluePieces--;
                break;
            case 3:
                healthPotions--;
                break;
        }
    }
}
