using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


    [Header("Action Icons")]
    [SerializeField] protected Sprite fireIcon;
    [SerializeField] protected Sprite earthIcon;
    [SerializeField] protected Sprite waterIcon;

    [Header("Color Hex Codes")]
    [SerializeField] protected Color heavyColor;
    [SerializeField] protected Color lightColor;
    [SerializeField] protected Color parryColor;


    public Sprite GetHeavyIcon()
    {
        return fireIcon;
    }

    public Sprite GetLightIcon()
    {
        return earthIcon;
    }

    public Sprite GetParryIcon()
    {
        return waterIcon;
    }

    public Color GetHeavyColor()
    {
        return heavyColor;
    }

    public Color GetLightColor()
    {
        return lightColor;
    }

    public Color GetParryColor()
    {
        return parryColor;
    }
}
