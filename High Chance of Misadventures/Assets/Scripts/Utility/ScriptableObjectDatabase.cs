using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectDatabase : MonoBehaviour
{
    #region singleton
    public static ScriptableObjectDatabase Instance { get; private set; }

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }
    #endregion

    [Header("Armor List For Sale")]
    public List<SO_Armor> helmetUpgrades = new List<SO_Armor>();
    public List<SO_Armor> cuirassUpgrades = new List<SO_Armor>();
    public List<SO_Armor> greavesUpgrades = new List<SO_Armor>();
}
