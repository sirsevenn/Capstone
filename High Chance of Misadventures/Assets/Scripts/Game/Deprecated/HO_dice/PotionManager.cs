using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionManager : MonoBehaviour
{
    [SerializeField] GameObject dicePrefab;
    [SerializeField] GameObject cauldronPanel;
    [SerializeField] Sprite goblinSprite;
    [SerializeField] Sprite snakeSprite;
    [SerializeField] TMP_Text text;
    [SerializeField] Button button;
    [SerializeField] List<Dice> diceList;

    [SerializeField] List<DicePanel> dicePanelList = new();
    [SerializeField] List<int> resultsList = new();
    [SerializeField] int finishedRolls = 0;


    public void AddGoblin()
    {
        for (int i = 0; i < dicePanelList.Count; i++)
        {
            DicePanel panel = dicePanelList[i];
            if (panel.GetDiceType() == DiceTypeOld.Goblin)
            {
                dicePanelList.Remove(panel);
                DestroyImmediate(panel.gameObject, true);
            }
        }

        GameObject instance = GameObject.Instantiate(dicePrefab, cauldronPanel.transform);
        DicePanel script = instance.GetComponent<DicePanel>();
        script.InitializeDice(goblinSprite, this, DiceTypeOld.Goblin);
        dicePanelList.Insert(0, script);
        text.text = "";
        button.interactable = true;
    }

    public void AddSnake()
    {
        if (dicePanelList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < dicePanelList.Count; i++)
        {
            DicePanel panel = dicePanelList[i];
            if (panel.GetDiceType() == DiceTypeOld.Snake)
            {
                dicePanelList.Remove(panel);
                DestroyImmediate(panel.gameObject, true);
            }
        }

        GameObject instance = GameObject.Instantiate(dicePrefab, cauldronPanel.transform);
        DicePanel script = instance.GetComponent<DicePanel>();
        script.InitializeDice(snakeSprite, this, DiceTypeOld.Snake);
        dicePanelList.Insert(1, script);
        text.text = "";
    }

    public void DeleteDicePanel(DiceTypeOld t)
    {
        if (t == DiceTypeOld.Snake)
        {
            int index = dicePanelList.FindIndex(p => p.GetDiceType() == t);
            DestroyImmediate(dicePanelList[index].gameObject, true);
            dicePanelList.RemoveAt(index);
        }
        else if (t == DiceTypeOld.Goblin)
        {
            foreach(DicePanel p in dicePanelList)
            {
                DestroyImmediate(p.gameObject, true);
            }
            dicePanelList.Clear();
            button.interactable = false;
        }

        text.text = "";
    }



    public void StartRollDice()
    {
        for(int i = 0; i < dicePanelList.Count; i++)
        {
            resultsList.Add(diceList[i].PerformDiceRoll());
        }
        button.interactable = false;
    }

    public void OnFinishedRoll(Dice d)
    {
        int index = diceList.IndexOf(d);
        dicePanelList[index].SetText(resultsList[index].ToString());

        finishedRolls++;
        if (finishedRolls >= dicePanelList.Count)
        {
            if (resultsList[0] == 1 || resultsList[0] == 2 ||
                (resultsList[0] == 3 && resultsList[1] >= 4))
            {
                text.text = "success";
            }
            else if (resultsList[0] >= 4)
            {
                text.text = "fail";
            }

            resultsList.Clear();
            finishedRolls = 0;
        }
    }
}
