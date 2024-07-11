using System;
using UnityEngine;

[Serializable]
public class GestureInputProperties
{
    public Touch finger;
    public Vector2 startPoint;
    public Vector2 endPoint;
    public float gestureTime;

    public GestureInputProperties(Touch newFinger, Vector2 newStartPoint, Vector2 newEndPoint, float newGestureTime)
    {
        finger = newFinger;
        startPoint = newStartPoint;
        endPoint = newEndPoint;
        gestureTime = newGestureTime;
    }
}