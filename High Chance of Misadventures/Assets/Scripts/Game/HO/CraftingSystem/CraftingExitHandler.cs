using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CraftingExitHandler : MonoBehaviour
{
    [Header("Knight Properties")]
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 receivePos;
    [SerializeField] AnimationHandler knightAnimator;

    [Space(10)] [Header("Animation Properties")]
    [SerializeField] private float enterDuration;
    [SerializeField] private float receiveDuration;
    [SerializeField] private float turnDuration;
    [SerializeField] private float exitDuration;

    [Space(10)] [Header("Other Properties")]
    [SerializeField] private PotionShelfRackManager shelfRackManager;


    private void Start()
    {
        // Reposition model to start position
        Transform knightTransform = knightAnimator.transform;
        knightTransform.position = startPos;

        // Rotate model to face where it is moving
        Vector3 enterDir = receivePos - startPos;
        enterDir.Normalize();
        float angle = Mathf.Rad2Deg * -Mathf.Acos(Vector3.Dot(Vector3.forward, enterDir));

        knightTransform.eulerAngles = new Vector3(0, angle, 0);
    }

    public void OnBeginExitFromCraftingScene()
    {
        StartCoroutine(ExitFromCraftingSceneCoroutine());
    }

    private IEnumerator ExitFromCraftingSceneCoroutine()
    {
        Transform knightTransform = knightAnimator.transform;

        // Animate enter movevment
        knightAnimator.PlayerMove();
        yield return knightTransform.DOMove(receivePos, enterDuration).SetEase(Ease.Linear).WaitForCompletion();

        // Animmate giving potions
        knightAnimator.PlayerStopMove();
        yield return shelfRackManager.AnimateGivingPotions(receivePos, receiveDuration);

        // Animate model turning around 
        Vector3 newRotation = knightTransform.eulerAngles;
        newRotation.y += 180;
        yield return knightTransform.DORotate(newRotation, turnDuration).SetEase(Ease.Linear).WaitForCompletion();

        // Animate exit movement
        knightAnimator.PlayerMove();
        yield return knightTransform.DOMove(startPos, exitDuration).SetEase(Ease.Linear).WaitForCompletion();

        // Trasition to next scene
        HO_GameManager.Instance.TransitionToBattleScene();
    }
}