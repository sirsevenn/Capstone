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
    [SerializeField] private List<GameObject> fireScrollsList;
    [SerializeField] private int numActiveFireScrolls;

    [Space(10)] [Header("Water Scrolls")]
    [SerializeField] private List<GameObject> waterScrollsList;
    [SerializeField] private int numActiveWaterScrolls;

    [Space(10)] [Header("Earth Scrolls")]
    [SerializeField] private List<GameObject> earthScrollsList;
    [SerializeField] private int numActiveEarthScrolls;


    private void Start()
    {
        ResetDisplay();
        numActiveHealthPotions = 0;
        numActiveDefensePotions = 0;
        numActiveFireScrolls = 0;
        numActiveWaterScrolls = 0;
        numActiveEarthScrolls = 0;
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

        foreach (var fireScroll in fireScrollsList)
        { 
            fireScroll.SetActive(false); 
        }

        foreach (var waterScroll in waterScrollsList)
        {
            waterScroll.SetActive(false);
        }

        foreach (var  earthScroll in earthScrollsList)
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
                itemList = fireScrollsList;
                currentActive = numActiveFireScrolls;
                break;

            case EConsumableType.Water_Potion:
                itemList = waterScrollsList;
                currentActive = numActiveWaterScrolls;
                break;

            case EConsumableType.Earth_Potion:
                itemList = earthScrollsList;
                currentActive = numActiveEarthScrolls;
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
