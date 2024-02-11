using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalManager : MonoBehaviour
{
    [Header("Journal Pages")]
    [SerializeField] private GameObject backgroundPage;
    [SerializeField] private GameObject[] pages;

    [Header("Debug")]
    private int currentPage;

    public void OpenJournal()
    {
        backgroundPage.SetActive(true);
    }

    public void CloseJournal()
    {
        backgroundPage.SetActive(false);
    }

    public void ToggleJournal()
    {
        backgroundPage.SetActive(!backgroundPage.activeSelf);
    }

    public void ChangePage(int value)
    {
        CloseAllPages();
        pages[GetNextPage(value)].SetActive(true);
    }

    private void CloseAllPages()
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
    }

    private int GetNextPage(int value)
    {
        currentPage += value;

        if (currentPage < 0)
        {
            currentPage = pages.Length - 1;
        }
        else if (currentPage > pages.Length - 1)
        {
            currentPage = 0;
        }

        return currentPage;
        
    }

}
