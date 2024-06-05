using System;
using UnityEngine;

public class CauldronMixingHandler : MonoBehaviour
{
    [Header("Mixing Required Properties")]
    [SerializeField] private EMixingType requiredMixingType;
    [SerializeField] private int requiredNumberOfMixes;
    [SerializeField] private float requiredMixingDistance;
    [SerializeField] private float maxFastMixTime;
    [SerializeField] private float maxNormalMixTime;

    [Space(10)] [Header("Mixing Tracker Properties")]
    [SerializeField] private bool isLadleSpoonOnRightSide;
    [SerializeField] private Vector2 sideStartingPos;
    [SerializeField] private float dtFromOneSide;
    [SerializeField] private int currentMixes;
    [SerializeField] private int numCorrectMix;

    [Space(10)] [Header("UI Properties")]
    [SerializeField] private RectTransform ladleSpoonTransform;

    public event Action<bool> OnFinishedMixingEvent;


    private void Start()
    {
        GestureManager.Instance.OnDragEvent += OnDrag;
        
        requiredMixingDistance = GetComponent<RectTransform>().rect.width * 1.2f;
        
        ResetTracker();
    }

    private void OnDestroy()
    {
        GestureManager.Instance.OnDragEvent -= OnDrag;
    }

    private void OnDrag(object send, DragEventArgs args)
    {
        if (!this.enabled) return;

        Touch dragFinger = args.DragFinger;

        // Update starting pos IF it is not initialized
        // OR finger goes farther on the left of the starting pos even though it needs to be on the right
        // OR finger goes farther on the right of the starting pos even though it needs to be on the left
        if (sideStartingPos.x == -1 && sideStartingPos.y == -1 ||
            isLadleSpoonOnRightSide && sideStartingPos.x < dragFinger.position.x ||
            !isLadleSpoonOnRightSide && sideStartingPos.x > dragFinger.position.x)
        {
            sideStartingPos = dragFinger.position;
        }

        // Update time to help keep track of the dragging/mixing speed
        dtFromOneSide += Time.deltaTime;

        // Check if the drag/mix is enough to relocate the ladle spoon to the other side
        if (HasFingerReachedTheOtherEnd(dragFinger))
        {
            // Check if the speed is correct
            if (requiredMixingType == EMixingType.Fast && dtFromOneSide < maxFastMixTime ||
                requiredMixingType == EMixingType.Normal && dtFromOneSide > maxFastMixTime && dtFromOneSide < maxNormalMixTime ||
                requiredMixingType == EMixingType.Slow && dtFromOneSide > maxNormalMixTime)
            {
                numCorrectMix++;
            }

            // Reset time tracker
            dtFromOneSide = 0f;

            // Check if player has done the appropriate amount of mixing to end their crafting state
            currentMixes++;
            if (currentMixes == requiredNumberOfMixes)
            {
                bool isMixingSuccessful = numCorrectMix > (int)(requiredNumberOfMixes * 0.25f);
                OnFinishedMixingEvent?.Invoke(isMixingSuccessful);

                ResetTracker();
            }
        }

        // If player lets go of the screen, reset some values
        if (args.DragFinger.phase == TouchPhase.Ended)
        {
            sideStartingPos = new Vector2(-1, -1);
            dtFromOneSide = 0f;
        }
    }

    private bool HasFingerReachedTheOtherEnd(Touch dragFinger)
    {
        bool hasReachedOtherEnd = false;

        if (isLadleSpoonOnRightSide &&
            Mathf.Abs(sideStartingPos.x - dragFinger.position.x) > requiredMixingDistance &&
            sideStartingPos.x > dragFinger.position.x)
        {
            isLadleSpoonOnRightSide = !isLadleSpoonOnRightSide;
            sideStartingPos = new Vector2(-1, -1);
            ladleSpoonTransform.Rotate(new Vector3(0, 180, 0));
            hasReachedOtherEnd = true;
        }
        else if (!isLadleSpoonOnRightSide &&
            Mathf.Abs(sideStartingPos.x - dragFinger.position.x) > requiredMixingDistance &&
            sideStartingPos.x < dragFinger.position.x)
        {
            isLadleSpoonOnRightSide = !isLadleSpoonOnRightSide;
            sideStartingPos = new Vector2(-1, -1);
            ladleSpoonTransform.Rotate(new Vector3(0, 180, 0));
            hasReachedOtherEnd = true;
        }

        return hasReachedOtherEnd;
    }

    private void ResetTracker()
    {
        sideStartingPos = new Vector2(-1, -1);
        dtFromOneSide = 0f;
        currentMixes = 0;
        numCorrectMix = 0;
    }

    public void SetMixingType(EMixingType newType)
    {
        requiredMixingType = newType;
    }
}