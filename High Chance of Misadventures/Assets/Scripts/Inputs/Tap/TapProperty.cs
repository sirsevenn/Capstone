using UnityEngine;

[System.Serializable]
public class TapProperty
{
    [Tooltip("Maximum allowable time until it's not a tap anymore")]
    public float tapTime = 0.7f;

    [Tooltip("Maximum allowable distance until it's not a tap anymore")]
    public float tapMaxDistance = 0.1f;
}