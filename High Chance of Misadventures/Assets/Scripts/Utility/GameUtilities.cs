using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtilities : MonoBehaviour
{
    public delegate void DelayDelegate();

    public static IEnumerator DelayFunction(DelayDelegate function, float delay)
    {
        yield return new WaitForSeconds(delay);
        function();
    }

    public static IEnumerator WaitForPlayerInput(DelayDelegate function)
    {
        bool inputReceived = false;
        while (!inputReceived)
        {
            if (Input.GetMouseButtonDown(0))
            {
                inputReceived = true;
            }
            yield return null;
        }

        function();
    }

}
