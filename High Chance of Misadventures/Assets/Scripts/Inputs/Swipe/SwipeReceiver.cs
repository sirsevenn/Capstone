using UnityEngine;

public class SwipeReceiver : MonoBehaviour
{
    //private PlayerScript playerScript;

    void Start()
    {
        GestureManager.Instance.OnSwipeEvent += OnSwipe;

        //playerScript = GameManager.Instance.GetPlayerScript();
    }

    void OnDisable()
    {
        GestureManager.Instance.OnSwipeEvent -= OnSwipe;
    }

    private void OnSwipe(object send, SwipeEventArgs args)
    {
        if (args.SwipeDirection == SwipeEventArgs.SwipeDirections.LEFT)
        {

        }
        else if (args.SwipeDirection == SwipeEventArgs.SwipeDirections.RIGHT)
        {

        }
    }
}