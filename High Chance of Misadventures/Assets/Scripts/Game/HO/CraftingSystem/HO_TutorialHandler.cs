using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HO_TutorialHandler : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private List<GameObject> infoPanelsList;


    private void Awake()
    {
        tutorialPanel.SetActive(false);

        foreach (var infoPanel in  infoPanelsList)
        {
            infoPanel.SetActive(false);
        }
    }

    public void InitiateTutorial()
    {
        tutorialPanel.SetActive(true);

        StartCoroutine(InitiateTutorialCoroutine());
    }

    private IEnumerator InitiateTutorialCoroutine()
    {
        for (int i = 0; i < infoPanelsList.Count; i++)
        {
            infoPanelsList[i].SetActive(true);
            if (i > 0)
            {
                infoPanelsList[i - 1].SetActive(false);
            }

            yield return GameUtilities.WaitForPlayerInput();
        }

        tutorialPanel.SetActive(false);
        HO_GameManager.Instance.OnFinishedTutorial();
        CraftingSystem.Instance.EnableInputs();
    }
}
