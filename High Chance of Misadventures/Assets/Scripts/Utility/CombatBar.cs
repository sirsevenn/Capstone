using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CombatBar : MonoBehaviour
{
    [SerializeField] private Slider combatSlider;

    public void ChangeValue(float value)
    {
        DOVirtual.Float(combatSlider.value, combatSlider.value + value, 1, IncrementValue);   
    }

    private void IncrementValue(float value)
    {
        combatSlider.value = value;
    }

}
