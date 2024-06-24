using System.Collections.Generic;
using UnityEngine;

public class PotionDisplay : MonoBehaviour
{
    [Header("Health Potions")]
    [SerializeField] private List<GameObject> healthPotionsList;
    [SerializeField] private int numActiveHealthPotions;

    [Space(10)] [Header("Defense Potions")]
    [SerializeField] private List<GameObject> defensePotionsList;
    [SerializeField] private int numActiveDefensePotions;

    [Space(10)] [Header("Fire Scrolls")]
    [SerializeField] private List<GameObject> firePotionsList;
    [SerializeField] private int numActiveFirePotions;

    [Space(10)] [Header("Water Scrolls")]
    [SerializeField] private List<GameObject> waterPotionsList;
    [SerializeField] private int numActiveWaterPotions;

    [Space(10)] [Header("Earth Scrolls")]
    [SerializeField] private List<GameObject> earthPotionsList;
    [SerializeField] private int numActiveEarthPotions;


    private void Start()
    {
        ResetDisplay();
        numActiveHealthPotions = 0;
        numActiveDefensePotions = 0;
        numActiveFirePotions = 0;
        numActiveWaterPotions = 0;
        numActiveEarthPotions = 0;
    }

    public void ResetDisplay()
    {
        foreach (var healthPotion in healthPotionsList)
        {
            healthPotion.SetActive(false);
        }

        foreach (var defensePotion in defensePotionsList)
        {
            defensePotion.SetActive(false);
        }

        foreach (var fireScroll in firePotionsList)
        { 
            fireScroll.SetActive(false); 
        }

        foreach (var waterScroll in waterPotionsList)
        {
            waterScroll.SetActive(false);
        }

        foreach (var  earthScroll in earthPotionsList)
        {
            earthScroll.SetActive(false);
        }
    }

    public void UpdateItemDisplay(EConsumableType consumableToEnable)
    {
        List<GameObject> itemList;
        int currentActive;
        int newNumToActivate = InventorySystem.Instance.GetConsumableAmount(consumableToEnable);

        switch (consumableToEnable)
        {
            case EConsumableType.Health_Potion:
                itemList = healthPotionsList;
                currentActive = numActiveHealthPotions;
                break;

            case EConsumableType.Defense_Potion:
                itemList = defensePotionsList;
                currentActive = numActiveDefensePotions;
                break;

            case EConsumableType.Fire_Potion:
                itemList = firePotionsList;
                currentActive = numActiveFirePotions;
                break;

            case EConsumableType.Water_Potion:
                itemList = waterPotionsList;
                currentActive = numActiveWaterPotions;
                break;

            case EConsumableType.Earth_Potion:
                itemList = earthPotionsList;
                currentActive = numActiveEarthPotions;
                break;

            default:
                return;
        }

        int diff = newNumToActivate - currentActive;
        int begin = (diff >= 0) ? currentActive : newNumToActivate;
        int end = (diff >= 0) ? newNumToActivate : currentActive;

        for (int i = begin; i < end; i++)
        {
            if (i >= itemList.Count) break;

            itemList[i].SetActive(diff >= 0);
        }
    }
}
