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


    private void Start()
    {
        diceCollider.enabled = false;
        diceRB.isKinematic = true;

        //foreach (var n in diceModelObj.GetComponent<MeshFilter>().mesh.normals)
        //{
        //    Debug.Log(n);
        //}
    }

    private void Update()
    {

    }

    public int PerformDiceRoll()
    {
        // PERSPECTIVE CAMERA
        //float aspectRatio = (float)Screen.width / Screen.height;
        //float boxHeight = 2.0f * distanceFromCamera * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        //float boxWidth = boxHeight * aspectRatio;

        // ORTHOGRAPHIC CAMERA
        float boxHeight = 2f * cam.orthographicSize;
        float boxWidth = boxHeight * cam.aspect;

        transform.position = cam.transform.position + new Vector3(-boxWidth / 2f - 2f, -19.5f, 0f);
        transform.rotation = Quaternion.identity;

        // Determine animation
        int randAnimNumber = Random.Range(1, diceData.PrerecordedRollsList.Count + 1);
        diceAnimator.SetTrigger(diceData.AnimationTriggerPrefix + randAnimNumber);
        
        // Get real and prerecorded results
        int diceRollResult = Random.Range(1, (int)diceData.Sides + 1);
        int animationResult = (int)diceData.PrerecordedRollsList[randAnimNumber - 1].DiceRollResult;

        // Rotate dice to get the real result
        Vector3 currentVector = diceData.DiceNumbersToVectorsList[diceRollResult - 1];
        Vector3 desiredVector = diceData.DiceNumbersToVectorsList[animationResult - 1];
        diceModelObj.transform.rotation = Quaternion.FromToRotation(currentVector, desiredVector);

        return diceRollResult;

        //Debug.Log("dice roll result:" + diceRollResult);
    }

    public void OnFinishDiceRollAnimation()
    {
        diceAnimator.SetTrigger(diceData.IdleTrigger);

        //potionManager.OnFinishedRoll(this);

        Debug.Log("done");
    }
}