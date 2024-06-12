using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;


    public void UpdateHP(float hpInPercent)
    {
        if (hpInPercent < 0 || hpInPercent > 1) return;

        healthBar.value = hpInPercent;
    }
}
