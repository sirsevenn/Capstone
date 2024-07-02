using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PotionShelfRack : Util_APoolable
{
    [SerializeField] private bool isShelfEmpty;
    [SerializeField] private bool isDisplayDisabled;

    [Space(10)]
    [SerializeField] private List<PotionShelf> potionShelvesList;

    [Space(10)] 
    [SerializeField] private List<EConsumableType> potionTypesForEachShelf;

    [Space(10)]
    [SerializeField] private List<int> numActivePotionsList;


    private void Start()
    {
        numActivePotionsList = new();
        ResetShelfRack();
    }

    #region Shelf Rack Methods
    public void AddPotionsToShelfRack(Dictionary<EConsumableType,int> newNumForEachShelf, float potionRevealDelay)
    {
        if (isDisplayDisabled || !isShelfEmpty) return;

        // Update shelf rack with new num values
        for (int i = 0; i < potionShelvesList.Count; i++)
        {
            EConsumableType potionTypeToUpdate = potionTypesForEachShelf[i];
            numActivePotionsList[i] = newNumForEachShelf[potionTypeToUpdate];
        }

        // Animation to reveal potions column-by-column
        StartCoroutine(AddPotionsCoroutine(newNumForEachShelf.Values.Max(), potionRevealDelay));

        // Update status
        isShelfEmpty = false;
    }

    private IEnumerator AddPotionsCoroutine(int highestAmount, float potionRevealDelay)
    {
        for (int i = 0; i < highestAmount; i++)
        {
            for (int j = 0; j < potionShelvesList.Count; j++)
            {
                List<GameObject> shelf = potionShelvesList[j].PotionsList;

                if (i < numActivePotionsList[j])
                {
                    shelf[i].SetActive(true);
                }
            }

            yield return new WaitForSeconds(potionRevealDelay);
        }
        CraftingSystem.Instance.EnableInputs(true);
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

    public bool IsShelfRackEmpty()
    {
        return isShelfEmpty;
    }

    private void ResetShelfRack()
    {
        for (int i = 0; i < potionShelvesList.Count; i++)
        {
            foreach (var potion in potionShelvesList[i].PotionsList)
            {
                potion.SetActive(false);
            }

            if (numActivePotionsList.Count == potionShelvesList.Count)
            {
                numActivePotionsList[i] = 0;
            }
            else
            {
                numActivePotionsList.Add(0);
            }
        }

        isShelfEmpty = true;
        isDisplayDisabled = (potionShelvesList.Count != potionTypesForEachShelf.Count);
    }
    #endregion


    #region Object Pooling
    public override void Initialize()
    {
        ResetShelfRack();
    }

    public override void OnActivate()
    {

    }

    public override void Release()
    {
        ResetShelfRack();
        transform.localPosition = Vector3.zero;
    }
    #endregion
}