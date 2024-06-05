using UnityEngine;
using System;

public class DragEventArgs : EventArgs
{
    private Touch _dragFinger;
    private float _dragDuration;
    private GameObject _hitObject;


    public DragEventArgs(Touch dragFinger, float dragDuration, GameObject obj = null)
    {
        _dragFinger = dragFinger;
        _dragDuration = dragDuration;
        _hitObject = obj;
    }

    public Touch DragFinger
    {
        get
        {
            return _dragFinger;
        }
    }

    public float DragDuration
    {
        get
        {
            return _dragDuration;
        }
    }

    public GameObject HitObject
    {
        get
        {
            return _hitObject;
        }
    }
}
