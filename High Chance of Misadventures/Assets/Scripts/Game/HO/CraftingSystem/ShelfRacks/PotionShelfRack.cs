using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PotionShelfRack : Util_APoolable
{
    [Header("General Properties")]
    [SerializeField] private bool isShelfEmpty;
    [SerializeField] private bool isDisplayDisabled;

    [Space(10)] [Header("Potion Model Properties")]
    [SerializeField] private List<PotionShelf> potionShelvesList;

    [Space(10)] 
    [SerializeField] private List<EConsumableType> potionTypesForEachShelf;

    [Space(10)]
    [SerializeField] private List<int> numActivePotionsList;

    [Space(20)] [Header("Crafting Properties")]
    [SerializeField] private List<CraftingMaterialSO> usedMaterialsList;

    [Space(20)] [Header("Other Properties")]
    [SerializeField] TMP_Text headerSignText;


    private void Awake()
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

            WaitForSeconds potionRevealDelayInSeconds = new WaitForSeconds(potionRevealDelay);
            yield return potionRevealDelayInSeconds;
        }

        CraftingSystem.Instance.EnableInputs();
    }

    public void UpdateUsedMaterials(List<CraftingMaterialSO> list)
    {
        if (!isShelfEmpty) return;

        usedMaterialsList = new(list);
    }

    public List<CraftingMaterialSO> GetUsedMaterialsList()
    {
        return usedMaterialsList;
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
        headerSignText.text = "";
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


    public void UpdateShelfRackSign(string text)
    {
        headerSignText.text = text;
    }

    public IEnumerator AnimateGivingPotions(Vector3 receivePos, float receiveDuration)
    {
        if (isShelfEmpty || isDisplayDisabled) yield break;

        int highestAmount = numActivePotionsList.Max();
        float giveDelay = 0.15f;
        float giveDuration = (receiveDuration - (highestAmount) * giveDelay);
        WaitForSeconds giveDelayInSeconds = new WaitForSeconds(giveDelay);

        receivePos.y += 2f;

        for (int i = 0; i < highestAmount; i++)
        {
            for (int j = 0; j < potionShelvesList.Count; j++)
            {
                List<GameObject> shelf = potionShelvesList[j].PotionsList;

                if (i < numActivePotionsList[j])
                {
                    GameObject shelfObj = shelf[i];
                    shelfObj.transform.DOJump(receivePos, 4.5f, 1, giveDuration).SetEase(Ease.OutCubic).OnComplete(() => {
                        shelfObj.SetActive(false);
                    });
                }
            }

            yield return giveDelayInSeconds;
        }

        WaitForSeconds afterGiveDelayInSeconds = new WaitForSeconds(giveDuration - giveDelay);
        yield return afterGiveDelayInSeconds;
    }
}