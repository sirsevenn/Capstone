using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject tutorialCanvas;

    [Header("Dialogue")]
    public List<GameObject> tutorialPanels;

    private int counter = -1;

    private void Start()
    {
        StartCoroutine(GameUtilities.DelayFunction(() =>
        {
            tutorialCanvas.SetActive(true);
            NextPanel();
        }, 5));

    }

    private void NextPanel()
    {
        if (counter >= 0)
        {
            tutorialPanels[counter].SetActive(false);
        }
       
        counter++;

        if (counter < tutorialPanels.Count)
        {
            tutorialPanels[counter].SetActive(true);
            StartCoroutine(GameUtilities.WaitForPlayerInput(NextPanel));
        }
        else
        {
            tutorialCanvas.SetActive(false);
        }
       
    }

}
