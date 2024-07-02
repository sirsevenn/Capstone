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

    [Space(10)] [Header("Shelf Racks Dynamic Properties")]
    [SerializeField] private int middleShelfRackIndex;
    [SerializeField] private List<PotionShelfRack> shelfRacksList;
    [SerializeField] private PotionShelfRack shelfRackToDestroy;


    #region Initialization and Events
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

        Util_APoolable middleRack = shelfRacksPool.RequestPoolable();
        shelfRacksList.Add((PotionShelfRack)middleRack);
        middleShelfRackIndex = 0;

        Util_APoolable rightRack = shelfRacksPool.RequestPoolable();
        rightRack.transform.localPosition += new Vector3(shelfMoveDistance, 0, 0);
        shelfRacksList.Add((PotionShelfRack)rightRack);
    }
    #endregion

    #region Movement Methods
    public bool CanMoveShelfRacksToTheRight(bool isGoingLeft)
    {
        PotionShelfRack farthestRackToTheLeft = (shelfRacksList.Count > 0) ? shelfRacksList[0] : null;
        return (!isGoingLeft && farthestRackToTheLeft != shelfRacksList[middleShelfRackIndex]);
    }

    public void MoveShelfRacks(bool isGoingLeft)
    {
        // Check if the movement is valid
        CanMoveShelfRacksToTheRight(isGoingLeft);

        // Create new shelf if going left
        if (isGoingLeft && !(middleShelfRackIndex == 0 && shelfRacksList.Count >= 3))
        {
            Util_APoolable newRack = shelfRacksPool.RequestPoolable();
            newRack.transform.localPosition += spawnLocalPos;
            shelfRacksList.Add((PotionShelfRack)newRack);
        }

        // Simulate movement
        PotionShelfRack farthestRackToTheLeft = (shelfRacksList.Count > 0) ? shelfRacksList[0] : null;

        if (farthestRackToTheLeft != shelfRacksList[middleShelfRackIndex] || isGoingLeft)
        {
            foreach (var shelfRack in shelfRacksList)
            {
                Vector3 endPos = shelfRack.transform.localPosition;
                endPos.x += (isGoingLeft) ? -shelfMoveDistance : shelfMoveDistance;
                shelfRack.transform.DOLocalMove(endPos, shelfMoveDuration);
            }
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

        // Animate lever
        Vector3 origEulerAngles = leverHandleTransform.rotation.eulerAngles;
        Vector3 endRotation = origEulerAngles;
        endRotation.x += (isGoingLeft) ? -leverRotationAngle : leverRotationAngle;

        leverHandleTransform.DORotate(endRotation, shelfMoveDuration / 3f).onComplete += () => {
            leverHandleTransform.DORotate(origEulerAngles, shelfMoveDuration / 3f);
        };

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
            shelfRackToDestroy.transform.DOLocalMove(posAwayFromCam, 0.35f);

            yield return new WaitForSeconds(0.35f);
            shelfRacksPool.ReleasePoolable(shelfRackToDestroy);
            shelfRackToDestroy = null;
        }

        CraftingSystem.Instance.EnableInputs(true);
    }
    #endregion

    #region Fill Up Methods
    public bool IsMiddleShelfRackEmmpty()
    {
        return shelfRacksList[middleShelfRackIndex].IsShelfRackEmpty();
    }

    public void FillUpMiddleShelfRack(Dictionary<EConsumableType, int> numPotions)
    {
        PotionShelfRack middleShelfRack = shelfRacksList[middleShelfRackIndex];

        if (!middleShelfRack.IsShelfRackEmpty()) return;

        middleShelfRack.AddPotionsToShelfRack(numPotions, potionRevealDelay);
    }
    #endregion

    public Dictionary<EConsumableType, int> GetNumPotionsFromMiddleShelfRack() => shelfRacksList[middleShelfRackIndex].GetNumActivePotions();
}