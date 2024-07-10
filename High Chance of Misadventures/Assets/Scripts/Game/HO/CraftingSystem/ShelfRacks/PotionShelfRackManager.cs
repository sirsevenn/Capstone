using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionShelfRackManager : MonoBehaviour
{
    [Header("Shelf Racks Static Properties")]
    [SerializeField] private float shelfMoveDistance;
    [SerializeField] private float shelfMoveDuration;
    [SerializeField] private float potionRevealDelay;

    [Space(10)]
    [SerializeField] private Vector3 spawnLocalPos;
    [SerializeField] private GameObject shelfRackPrefab;
    [SerializeField] private Transform shelfRackParent;
    [SerializeField] private Util_GameObjectPool shelfRacksPool;

    [Space(10)]
    [SerializeField] private float leverRotationAngle;
    [SerializeField] private Transform leverHandleTransform;

    [Space(10)][Header("Shelf Racks Dynamic Properties")]
    [SerializeField] private int middleShelfRackIndex;
    [SerializeField] private List<PotionShelfRack> shelfRacksList;
    [SerializeField] private PotionShelfRack shelfRackToDestroy;


    #region Initialization
    private void Start()
    {
        InitializeInitialSetup();
        shelfMoveDistance = (shelfMoveDistance == 0) ? 9 : shelfMoveDistance;
        shelfMoveDuration = (shelfMoveDuration <= 0) ? 1f : shelfMoveDuration;
        shelfRackToDestroy = null;
    }

    private void InitializeInitialSetup()
    {
        shelfRacksPool.Initialize();

        Util_APoolable middlePoolable = shelfRacksPool.RequestPoolable();
        PotionShelfRack middleRack = (PotionShelfRack)middlePoolable;
        middleRack.UpdateShelfRackSign("For Delivery");
        shelfRacksList.Add(middleRack);
        middleShelfRackIndex = 0;

        Util_APoolable rightPoolable = shelfRacksPool.RequestPoolable();
        rightPoolable.transform.localPosition += new Vector3(shelfMoveDistance, 0, 0);
        shelfRacksList.Add((PotionShelfRack)rightPoolable);
    }
    #endregion

    #region Movement Methods
    public bool CanMoveShelfRacksToTheRight()
    {
        PotionShelfRack farthestRackToTheLeft = (shelfRacksList.Count > 0) ? shelfRacksList[0] : null;
        return (farthestRackToTheLeft != shelfRacksList[middleShelfRackIndex]);
    }

    public void MoveShelfRacks(bool isGoingLeft)
    {
        // Check if the movement is valid
        if (!isGoingLeft && !CanMoveShelfRacksToTheRight()) return;

        // Create new shelf if going left
        if (isGoingLeft && (middleShelfRackIndex != 0 || shelfRacksList.Count < 3))
        {
            Util_APoolable newRack = shelfRacksPool.RequestPoolable();
            newRack.transform.localPosition += spawnLocalPos;
            shelfRacksList.Add((PotionShelfRack)newRack);
        }

        // Simulate movement
        PotionShelfRack farthestRackToTheLeft = (shelfRacksList.Count > 0) ? shelfRacksList[0] : null;

        foreach (var shelfRack in shelfRacksList)
        {
            Vector3 endPos = shelfRack.transform.localPosition;
            endPos.x += (isGoingLeft) ? -shelfMoveDistance : shelfMoveDistance;
            shelfRack.transform.DOLocalMove(endPos, shelfMoveDuration);
        }

        // Update middle index
        if (isGoingLeft)
        {
            middleShelfRackIndex++;

            if (middleShelfRackIndex == 2)
            {
                shelfRackToDestroy = farthestRackToTheLeft;
                shelfRacksList.RemoveAt(0);
                middleShelfRackIndex--;
            }
        }
        else if (!isGoingLeft && farthestRackToTheLeft != shelfRacksList[middleShelfRackIndex])
        {
            middleShelfRackIndex--;
        }

        // Update the signs above the shelf racks
        for (int i = 0; i < shelfRacksList.Count; i++)
        {
            if (i == middleShelfRackIndex)
            {
                shelfRacksList[i].UpdateShelfRackSign("For Delivery");
            }
            else if (i < middleShelfRackIndex)
            {
                shelfRacksList[i].UpdateShelfRackSign("For Disposal");
            }
            else
            {
                shelfRacksList[i].UpdateShelfRackSign("On Standby");
            }
        }

        // Animate lever
        Vector3 origEulerAngles = leverHandleTransform.rotation.eulerAngles;
        Vector3 endRotation = origEulerAngles;
        endRotation.x += (isGoingLeft) ? -leverRotationAngle : leverRotationAngle;

        leverHandleTransform.DORotate(endRotation, shelfMoveDuration / 3f).OnComplete(() => {
            leverHandleTransform.DORotate(origEulerAngles, shelfMoveDuration / 3f);
        });

        // Cleanup after the movement animation
        StartCoroutine(WaitForMovementToFinish());
    }

    private IEnumerator WaitForMovementToFinish()
    {
        yield return new WaitForSeconds(shelfMoveDuration);

        if (shelfRackToDestroy != null)
        {
            Vector3 posAwayFromCam = shelfRackToDestroy.transform.localPosition;
            posAwayFromCam.x -= shelfMoveDistance / 2f;
            yield return shelfRackToDestroy.transform.DOLocalMove(posAwayFromCam, 0.35f).WaitForCompletion();

            shelfRacksPool.ReleasePoolable(shelfRackToDestroy);
            shelfRackToDestroy = null;
        }

        CraftingSystem.Instance.EnableInputs();
    }
    #endregion

    #region Middle Shelf Rack Methods
    public void FillUpMiddleShelfRack(Dictionary<EConsumableType, int> numPotions)
    {
        PotionShelfRack middleShelfRack = shelfRacksList[middleShelfRackIndex];

        if (!middleShelfRack.IsShelfRackEmpty()) return;

        middleShelfRack.AddPotionsToShelfRack(numPotions, potionRevealDelay);
    }

    public void UpdateUsedMaterialsForMiddleShelfRack(List<CraftingMaterialSO> usedMaterials)
    {
        PotionShelfRack middleShelfRack = shelfRacksList[middleShelfRackIndex];

        if (!middleShelfRack.IsShelfRackEmpty()) return;

        for (int i = usedMaterials.Count - 1; i >= 0; i--)
        {
            if (usedMaterials[i] == null)
            {
                usedMaterials.RemoveAt(i);
            }
        }

        middleShelfRack.UpdateUsedMaterials(usedMaterials);
    }

    public bool IsMiddleShelfRackEmmpty() => shelfRacksList[middleShelfRackIndex].IsShelfRackEmpty();

    public Dictionary<EConsumableType, int> GetNumPotionsFromMiddleShelfRack() => shelfRacksList[middleShelfRackIndex].GetNumActivePotions();

    public List<CraftingMaterialSO> GetUsedMaterialsFromMiddleShelfRack() => shelfRacksList[middleShelfRackIndex].GetUsedMaterialsList();

    public IEnumerator AnimateGivingPotions(Vector3 receivePos, float receiveDuration)
    {
        yield return shelfRacksList[middleShelfRackIndex].AnimateGivingPotions(receivePos, receiveDuration);
    }
    #endregion
}