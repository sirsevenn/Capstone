using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorManager : MonoBehaviour
{
    public bool inGame = false;

    public SO_Armor Helmet;
    public SO_Armor Cuirass;
    public SO_Armor Greaves;

    public List<GameObject> helmetObjects = new List<GameObject>();
    public List<GameObject> cuirassObjects = new List<GameObject>();
    public List<GameObject> greavesObjects = new List<GameObject>();

    public BaseCombatManager combatManager;
    private void Start()
    {
        InitializeArmor();
    }

    private void ResetHelmet()
    {
        for (int i = 0; i < helmetObjects.Count; i++)
        {
            helmetObjects[i].SetActive(false);
        }
    }

    private void ResetCuirass()
    {
        for (int i = 0; i < cuirassObjects.Count; i++)
        {
            cuirassObjects[i].SetActive(false);
        }
    }

    private void ResetGreaves()
    {
        for (int i = 0; i < greavesObjects.Count; i++)
        {
            greavesObjects[i].SetActive(false);
        }
    }

    private void InitializeArmor()
    {
        UpdateHelmet();
        UpdateCuirass();
        UpdateGreaves();

        if (inGame && combatManager != null)
        {
            int defenseTotal = Helmet.defenseValue + Cuirass.defenseValue + Greaves.defenseValue;
            combatManager.SetPlayerValues(10, defenseTotal);
        }
      
    }

    public void UpdateHelmet()
    {
        int helmetLevel = InventoryManager.Instance.HelmetLevel;
        Helmet = ScriptableObjectDatabase.Instance.helmetUpgrades[helmetLevel];
        ResetHelmet();
        helmetObjects[helmetLevel].SetActive(true);
    }

    public void UpdateCuirass()
    {
        int cuirassLevel = InventoryManager.Instance.CuirassLevel;
        Cuirass = ScriptableObjectDatabase.Instance.cuirassUpgrades[cuirassLevel];
        ResetCuirass();
        cuirassObjects[cuirassLevel].SetActive(true);
    }

    public void UpdateGreaves()
    {
        int greavesLevel = InventoryManager.Instance.GreavesLevel;
        Greaves = ScriptableObjectDatabase.Instance.greavesUpgrades[greavesLevel];
        ResetGreaves();
        greavesObjects[greavesLevel].SetActive(true);
    }
}
