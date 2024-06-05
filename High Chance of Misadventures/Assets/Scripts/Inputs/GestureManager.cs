using System;
using System.Collections.Generic;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
    #region Singleton
    public static GestureManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    #region Fields
    // events 
    public EventHandler<TapEventArgs> OnTapEvent;
    public EventHandler<SwipeEventArgs> OnSwipeEvent;
    public EventHandler<DragEventArgs> OnDragEvent;
    public EventHandler<SpreadEventArgs> OnSpreadEvent;

    // inputs
    public TapProperty _tapProperty;
    public SwipeProperty _swipeProperty;
    public SpreadProperty _spreadProperty;

    private List<GestureInputProperties> activeInputPropertiesList = new();
    #endregion

    // Unity Methods
    void Update()
    {
        if (Input.touchCount > 0)
        {
            DetectFingerInputs();
        }
    }


    // Methods for Checking Inputs
    private void DetectFingerInputs()
    {
        for (int i = activeInputPropertiesList.Count - 1; i > -1; i--)
        {
            if (activeInputPropertiesList[i].finger.phase == TouchPhase.Ended)
            {
                activeInputPropertiesList.RemoveAt(i);
            }
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            if (i + 1 > activeInputPropertiesList.Count)
            {
                GestureInputProperties newProperty = new(Input.GetTouch(i), new Vector2(-1f, -1f), new Vector2(-1f, -1f), 0f);
                activeInputPropertiesList.Add(newProperty);
            }
            else
            {
                GestureInputProperties property = activeInputPropertiesList[i];
                property.finger = Input.GetTouch(i); //always reset struct values, theyre immutable with no references
            }

            CheckSingleFingerGesture(activeInputPropertiesList[i].finger, activeInputPropertiesList[i]);
        }
    }

    private void CheckSingleFingerGesture(Touch finger, GestureInputProperties property)
    {
        if (finger.phase == TouchPhase.Began)
        {
            property.startPoint = finger.position;
            property.gestureTime = 0.0f;
        }
        else if (finger.phase == TouchPhase.Ended && property.startPoint.x != -1 && property.startPoint.y != -1)
        {
            property.endPoint = finger.position;

            if (property.gestureTime <= _tapProperty.tapTime &&
                Vector2.Distance(property.startPoint, property.endPoint) < (Screen.dpi * _tapProperty.tapMaxDistance))
            {
                FireTapEvent(property);
            }
            else if (property.gestureTime <= _swipeProperty.swipeTime &&
                Vector2.Distance(property.startPoint, property.endPoint) > (Screen.dpi * _swipeProperty.minSwipeDistance))
            {
                FireSwipeEvent(property);
                FireDragEvent(property);
            }
            else
            {
                FireDragEvent(property);
            }

            property.startPoint = new Vector2(-1, -1);
            property.endPoint = new Vector2(-1, -1);
        }
        else
        {
            property.gestureTime += Time.deltaTime;

            //if (finger.phase == TouchPhase.Moved)
            //{
                FireDragEvent(property);
            //}
        }
    }

    private Vector2 GetPreviousPoint(Touch finger)
    {
        return finger.position - finger.deltaPosition;
    }


    // Methods for Input Events
    private void FireTapEvent(GestureInputProperties tapProperties)
    {
        GameObject hitObj = null;
        TapEventArgs tapArgs = new TapEventArgs(tapProperties.endPoint, hitObj);
        if (OnTapEvent != null)
        {
            OnTapEvent(this, tapArgs);
        }
    }

    private void FireSwipeEvent(GestureInputProperties property)
    {
        // determine swipe direction
        SwipeEventArgs.SwipeDirections swipeDir = SwipeEventArgs.SwipeDirections.UNKNOWN;
        Vector2 dir = property.endPoint - property.startPoint;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0) swipeDir = SwipeEventArgs.SwipeDirections.RIGHT;
            else swipeDir = SwipeEventArgs.SwipeDirections.LEFT;
        }
        else
        {
            if (dir.y > 0) swipeDir = SwipeEventArgs.SwipeDirections.UP;
            else swipeDir = SwipeEventArgs.SwipeDirections.DOWN;
        }

        SwipeEventArgs swipeArgs = new SwipeEventArgs(property.startPoint, swipeDir, dir, null);
        if (OnSwipeEvent != null)
        {
            OnSwipeEvent(this, swipeArgs);
        }
    }

    private void FireDragEvent(GestureInputProperties property)
    {
        DragEventArgs args = new DragEventArgs(property.finger, property.gestureTime, null);

        if (OnDragEvent != null)
        {
            OnDragEvent(this, args);
        }
    }

    private void FireSpreadEvent(float spreadDistance)
    {
        //if (OnSpreadEvent != null)
        //{
        //    SpreadEventArgs spreadArgs = new SpreadEventArgs(swipeFinger, tapFinger, spreadDistance, null);
        //    OnSpreadEvent(this, spreadArgs);
        //}
    }
}