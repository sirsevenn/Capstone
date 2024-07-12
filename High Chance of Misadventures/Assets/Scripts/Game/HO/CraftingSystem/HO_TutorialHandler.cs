using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HO_TutorialHandler : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private List<GameObject> infoPanelsList;
    [SerializeField] private float infoDelay;
    private WaitForSeconds infoDelayInSeconds;


    private void Awake()
    {
        tutorialPanel.SetActive(false);

        foreach (var infoPanel in  infoPanelsList)
        {
            infoPanel.SetActive(false);
        }

        infoDelay = (infoDelay == 0) ? 8f : infoDelay;
        infoDelayInSeconds = new WaitForSeconds(infoDelay);
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

            yield return infoDelayInSeconds;
        }

        infoPanelsList.Last().SetActive(false);
        tutorialPanel.SetActive(false);
        HO_GameManager.Instance.OnFinishedTutorial();
        CraftingSystem.Instance.EnableInputs();
    }
}
