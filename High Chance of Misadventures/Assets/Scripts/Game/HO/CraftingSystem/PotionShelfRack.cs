using System;
using System.Collections.Generic;
using UnityEngine;

public class PotionShelfRack : MonoBehaviour
{
    [SerializeField] private List<PotionShelf> potionShelvesList;

    [Space(10)] 
    [SerializeField] private List<EConsumableType> potionTypesForEachShelf;

    [Space(10)]
    [SerializeField] private List<int> numActivePotionsList;

    [Space(10)]
    [SerializeField] private bool isDisplayDisabled;


    private void Start()
    {
        foreach (var shelf in potionShelvesList)
        {
            foreach (var potion in shelf.PotionsList)
            {
                potion.SetActive(false);
            }

            numActivePotionsList.Add(0);
        }

        isDisplayDisabled = (potionShelvesList.Count != potionTypesForEachShelf.Count);
    }

    public void UpdateShelfRack(Dictionary<EConsumableType,int> newNumForEachShelf)
    {
        if (isDisplayDisabled) return;

        for (int i = 0; i < potionShelvesList.Count; i++)
        {
            EConsumableType potionTypeToUpdate = potionTypesForEachShelf[i];

            List<GameObject> shelf = potionShelvesList[i].PotionsList;
            int currentActive = numActivePotionsList[i];
            int newNumToActivate = newNumForEachShelf[potionTypeToUpdate];
            numActivePotionsList[i] = newNumToActivate;

            int diff = newNumToActivate - currentActive;
            int begin = (diff >= 0) ? currentActive : newNumToActivate;
            int end = (diff >= 0) ? newNumToActivate : currentActive;

            for (int j = begin; j < end; j++)
            {
                if (j >= shelf.Count) break;

                shelf[j].SetActive(diff >= 0);
            }
        }
    }

    public Dictionary<EConsumableType, int> GetNumActivePotions()
    {
        if (isDisplayDisabled) return null;

        Dictionary<EConsumableType, int> returnDictionary = new();

        for (int i = 0; i < potionTypesForEachShelf.Count; i++)
        {
            returnDictionary.Add(potionTypesForEachShelf[i], numActivePotionsList[i]);
        }

        return returnDictionary;
    }
}
