using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


    [Header("Action Icons")]
    [SerializeField] protected Sprite heavyIcon;
    [SerializeField] protected Sprite lightIcon;
    [SerializeField] protected Sprite parryIcon;

    [Header("Color Hex Codes")]
    [SerializeField] protected Color heavyColor;
    [SerializeField] protected Color lightColor;
    [SerializeField] protected Color parryColor;


    public Sprite GetHeavyIcon()
    {
        return heavyIcon;
    }

    public Sprite GetLightIcon()
    {
        return lightIcon;
    }

    public Sprite GetParryIcon()
    {
        return parryIcon;
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
