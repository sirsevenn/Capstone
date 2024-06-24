using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;


    private void Awake()
    {
        healthBar.direction = Slider.Direction.LeftToRight;
    }

    public void UpdateHP(float hpInPercent)
    {
        if (hpInPercent < 0 || hpInPercent > 1) return;

        healthBar.value = hpInPercent;
    }
}
