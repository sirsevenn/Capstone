using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HO_UIManager : UIManager
{
    public static HO_UIManager Instance { get; private set; }

#region
    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;

    }
#endregion

    [Header("Probability Texts")]
    [SerializeField] private TMP_Text heavyProbabilityText;
    [SerializeField] private TMP_Text lightProbabilityText;
    [SerializeField] private TMP_Text parryProbabilityText;

    public void SetProbabiltyBoard(int heavy, int light, int parry)
    {
        heavyProbabilityText.text = heavy.ToString() + "%";
        lightProbabilityText.text = light.ToString() + "%";
        parryProbabilityText.text = parry.ToString() + "%";
    }

    public void ResetProbabilityBoard()
    {
        heavyProbabilityText.text = "---" + "%";
        lightProbabilityText.text = "---" + "%";
        parryProbabilityText.text = "---" + "%";
    }

}
