using UnityEngine;

public class SpreadReceiver : MonoBehaviour
{
    private Transform playerTransform;
    //private PlayerScript playerScript;

    void Start()
    {
        //playerTransform = GameManager.Instance.GetPlayerTransform();
        //playerScript = GameManager.Instance.GetPlayerScript();

        GestureManager.Instance.OnSpreadEvent += OnSpread;
    }

    private void OnDisable()
    {
        GestureManager.Instance.OnSpreadEvent -= OnSpread;
    }

    private void OnSpread(object send, SpreadEventArgs args)
    {
        if (args.DistanceDelta > 0)
        {

        }
        else if (args.DistanceDelta < 0)
        {

        }
    }
}