using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorManager : MonoBehaviour
{
    public SO_Armor Helmet;
    public SO_Armor Cuirass;
    public SO_Armor Greaves;

    public List<GameObject> helmetObjects = new List<GameObject>();
    public List<GameObject> cuirassObjects = new List<GameObject>();
    public List<GameObject> greavesObjects = new List<GameObject>();

    private void Start()
    {
        
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
        int helmetLevel = InventoryManager.Instance.HelmetLevel;
        int cuirassLevel = InventoryManager.Instance.CuirassLevel;
        int greavesLevel = InventoryManager.Instance.GreavesLevel;

        Helmet = ScriptableObjectDatabase.Instance.helmetUpgrades[helmetLevel];
        Cuirass = ScriptableObjectDatabase.Instance.cuirassUpgrades[cuirassLevel];
        Greaves = ScriptableObjectDatabase.Instance.greavesUpgrades[greavesLevel];

        ResetHelmet();
        ResetCuirass();
        ResetGreaves();
        helmetObjects[helmetLevel].SetActive(true);
        cuirassObjects[cuirassLevel].SetActive(true);
        greavesObjects[greavesLevel].SetActive(true);


    }
}
