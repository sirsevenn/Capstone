using System;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [Header("Dice Main Properties")]
    [SerializeField] private GameObject diceModelObj;
    [SerializeField] private DiceSO diceData;

    [Header("Extra References")]
    [SerializeField] private Collider diceCollider;
    [SerializeField] private Rigidbody diceRB;
    [SerializeField] private Animator diceAnimator;
    [SerializeField] private Camera cam;
    //[SerializeField] private PotionManager potionManager;

    public event Action OnFinishedDiceRollAnimationEvent;


    private void Start()
    {
        diceCollider.enabled = false;
        diceRB.isKinematic = true;

        //foreach (var n in diceModelObj.GetComponent<MeshFilter>().mesh.normals)
        //{
        //    Debug.Log(n);
        //}
    }

    public DiceType GetDiceType()
    {
        return diceData.DiceType;
    }

    public int PerformDiceRoll()
    {
        if (diceAnimator == null || cam == null) return -1;

        //// PERSPECTIVE CAMERA
        ////float aspectRatio = (float)Screen.width / Screen.height;
        ////float boxHeight = 2.0f * distanceFromCamera * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        ////float boxWidth = boxHeight * aspectRatio;

        //// ORTHOGRAPHIC CAMERA
        //float boxHeight = 2f * cam.orthographicSize;
        //float boxWidth = boxHeight * cam.aspect;

        //transform.position = cam.transform.position + new Vector3(-boxWidth / 2f - 2f, -19.5f, 0f);
        //transform.rotation = Quaternion.identity;

        //// Determine animation
        //int randAnimNumber = UnityEngine.Random.Range(1, diceData.PrerecordedRollsList.Count + 1);
        //diceAnimator.SetTrigger(diceData.AnimationTriggerPrefix + randAnimNumber);

        //// Get real and prerecorded results
        //int diceRollResult = UnityEngine.Random.Range(1, (int)diceData.Sides + 1);
        //int animationResult = (int)diceData.PrerecordedRollsList[randAnimNumber - 1].DiceRollResult;

        //// Rotate dice to get the real result
        //Vector3 currentVector = diceData.DiceNumbersToVectorsList[diceRollResult - 1];
        //Vector3 desiredVector = diceData.DiceNumbersToVectorsList[animationResult - 1];
        //diceModelObj.transform.rotation = Quaternion.FromToRotation(currentVector, desiredVector);

        //return diceRollResult;


        // Determine animation
        int randAnimNumber = UnityEngine.Random.Range(1, diceData.PrerecordedRollsList.Count + 1);
        diceAnimator.SetTrigger(diceData.AnimationTriggerPrefix + randAnimNumber);

        // Predetermine dice result
        int diceRollResult = UnityEngine.Random.Range(1, (int)diceData.Sides + 1);
        Debug.Log("dice roll result:" + diceRollResult);

        return diceRollResult;
    }

    public void OnFinishDiceRollAnimation()
    {
        diceAnimator.SetTrigger(diceData.IdleTrigger);
        OnFinishedDiceRollAnimationEvent?.Invoke();

        //potionManager.OnFinishedRoll(this);
    }

    public void RemoveAllListenersInEvent()
    {
        OnFinishedDiceRollAnimationEvent = null;
    }
}