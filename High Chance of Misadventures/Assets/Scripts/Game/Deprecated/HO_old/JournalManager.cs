using System;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq;

public class JournalManager : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private GameObject sideBar;
    [SerializeField] private GameObject sideArrow;
    [SerializeField] private bool isOpen = false;

    [Space(10)]
    [SerializeField] private Transform closePos;
    [SerializeField] private Transform openPos;
    [SerializeField] private float travelDuration = 1;

    [Header("Journal Properties")]
    [SerializeField] private int currentPageNum = 0;
    [SerializeField] private EnemyType currentEnemyType;
    [SerializeField] private EnemyType prevEnemyType;
    [SerializeField] private int maxPages;
    private Dictionary<JournalDataType, int> recordedTallies = new();
    private Dictionary<EnemyType, int> totalTallies = new();

    [Header("Journal UI")]
    [SerializeField] private TMP_Text enemyTypeText;

    [Space(5)]
    [SerializeField] private TMP_Text heavyTallyText;
    [SerializeField] private TMP_Text lightTallyText;
    [SerializeField] private TMP_Text parryTallyText;
    [SerializeField] private TMP_Text totalTallyText;

    [Space(5)]
    [SerializeField] private TMP_Text heavyProbText;
    [SerializeField] private TMP_Text lightProbText;
    [SerializeField] private TMP_Text parryProbText;
    [SerializeField] private TMP_Text totalProbText;


    [Header("Other References")]
    [SerializeField] private HO_CombatManager combatManager;


    private void Start()
    {
        foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
        {
            if (enemyType == EnemyType.None)
            {
                continue;
            }

            int total = 0;

            foreach (ActionType attackType in Enum.GetValues(typeof(ActionType)))
            {
                if (attackType == ActionType.None)
                {
                    continue;
                }

                string prefsKey = CreateJournalKey(enemyType, attackType);
                JournalDataType data;
                data.enemyType = enemyType;
                data.attackType = attackType;

                int tally = PlayerPrefs.GetInt(prefsKey, defaultValue: 0);
                recordedTallies[data] = tally;
                total += tally;
            }

            totalTallies[enemyType] = total;
        }

        currentEnemyType = EnemyType.None;
        prevEnemyType = EnemyType.None;
    }

    public void ToggleJournal()
    {
        if (!isOpen)
        {
            UpdateJournalPage();

            sideBar.transform.DOMoveX(openPos.position.x, travelDuration, true).SetEase(Ease.Linear);
            sideArrow.transform.localScale = new Vector3(-1, 1, 1);
            isOpen = !isOpen;
        }
        else if (isOpen)
        {
            sideBar.transform.DOMoveX(closePos.position.x, travelDuration, true).SetEase(Ease.Linear);
            sideArrow.transform.localScale = new Vector3(1, 1, 1);
            isOpen = !isOpen;
        }
    }

    public void ChangePage(int value)
    {
        currentPageNum += value;
        if (currentPageNum < 0)
        {
            currentPageNum = maxPages - 1;
        }
        else if (currentPageNum > maxPages - 1)
        {
            currentPageNum = 0;
        }

        if (combatManager.enemyList.Count != 0)
        {
            Enemy newlySelectedEnemy = combatManager.enemyList[currentPageNum];
            HO_GameFlow.Instance.OnReselectEnemy(newlySelectedEnemy); // updates page here
        }
    }

    public void IncreaseTally(int attackType)
    {
        if (currentEnemyType == EnemyType.None)
        {
            return;
        }

        JournalDataType data;
        data.enemyType = currentEnemyType;
        data.attackType = (ActionType)attackType;

        recordedTallies[data] += 1;
        totalTallies[currentEnemyType] += 1;
        UpdateJournalPage();
    }

    public void DecreaseTally(int attackType)
    {
        if (currentEnemyType == EnemyType.None)
        {
            return;
        }

        JournalDataType data;
        data.enemyType = currentEnemyType;
        data.attackType = (ActionType)attackType;

        int tally = recordedTallies[data];
        if (tally != 0)
        {
            tally -= 1;
            recordedTallies[data] = tally;
            totalTallies[currentEnemyType] -= 1;
        }
        UpdateJournalPage();
    }

    public void SaveJournalData()
    {
        foreach (var pair in recordedTallies)
        {
            JournalDataType data = pair.Key;
            int tally = pair.Value;
            string prefsKey = CreateJournalKey(data.enemyType, data.attackType);
            PlayerPrefs.SetInt(prefsKey, tally);
        }
    }

    private string CreateJournalKey(EnemyType enemyType, ActionType attackType)
    {
        string key = "JournalTally_" + enemyType.ToString() + "+";
        key += (attackType == ActionType.None) ? "Total" : attackType.ToString();
        return key;
    }

    public void UpdateJournalPage()
    {
        maxPages = combatManager.enemyList.Count;
        currentPageNum = HO_GameFlow.Instance.SelectedEnemyIndex;

        if (maxPages != 0)
        {
            Enemy selectedEnemy = combatManager.enemyList[currentPageNum]; 
            HO_GameFlow.Instance.OnReselectEnemy(selectedEnemy);

            prevEnemyType = currentEnemyType;
            currentEnemyType = selectedEnemy.data.type;
        }

        enemyTypeText.text = currentEnemyType.ToString();

        int tally = 0;
        decimal probability = 0M;
        int total = totalTallies[currentEnemyType];

        JournalDataType heavyData;
        heavyData.enemyType = currentEnemyType;
        heavyData.attackType = ActionType.Heavy;

        tally = recordedTallies[heavyData];
        heavyTallyText.text = tally.ToString();
        probability = (total == 0) ? 0M : ((decimal)tally / total * 100);
        probability = Math.Round(probability, 2);
        heavyProbText.text = probability.ToString() + "%";
        

        JournalDataType lightData;
        lightData.enemyType = currentEnemyType;
        lightData.attackType = ActionType.Light;

        tally = recordedTallies[lightData];
        lightTallyText.text = tally.ToString();
        probability = (total == 0) ? 0M : ((decimal)tally / total * 100);
        probability = Math.Round(probability, 2);
        lightProbText.text = probability.ToString() + "%"; 


        JournalDataType parryData;
        parryData.enemyType = currentEnemyType;
        parryData.attackType = ActionType.Parry;

        tally = recordedTallies[parryData];
        parryTallyText.text = tally.ToString();
        probability = (total == 0) ? 0M : ((decimal)tally / total * 100);
        probability = Math.Round(probability, 2);
        parryProbText.text = probability.ToString() + "%";


        totalTallyText.text = total.ToString();
        probability = (total == 0) ? 0M : 100M;
        totalProbText.text = probability.ToString() + "%";
    }


    //DEBUG
    public void ResetJournalKeys()
    {
        foreach (var key in recordedTallies.Keys.ToList())
        {
            string prefsKey = CreateJournalKey(key.enemyType, key.attackType);
            PlayerPrefs.DeleteKey(prefsKey);

            recordedTallies[key] = 0;
        }

        foreach (var key in totalTallies.Keys.ToList())
        {
            totalTallies[key] = 0;
        }

        UpdateJournalPage();
    }
}