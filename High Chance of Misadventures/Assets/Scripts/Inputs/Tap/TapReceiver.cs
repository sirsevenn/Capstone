using UnityEngine;

public class TapReceiver : MonoBehaviour
{
    //private PlayerScript playerScript;

    void Start()
    {
        GestureManager.Instance.OnTapEvent += onTap;

        //playerScript = GameManager.Instance.GetPlayerScript();
    }

    void OnDisable()
    {
        GestureManager.Instance.OnTapEvent -= onTap;
    }

    private void onTap(object send, TapEventArgs args)
    {

    }
}