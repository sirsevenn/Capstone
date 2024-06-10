using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceHolder : MonoBehaviour
{
    [SerializeField] private List<Dice> diceList;
    [SerializeField] private Dice activeDice;
    [SerializeField] private int recentDiceRollResult;
    [SerializeField] private bool isFinishedRolling;


    private void Start()
    {
        ResetDiceProperties();
    }

    public void ResetDiceProperties()
    {
        activeDice = null;
        recentDiceRollResult = -1;
        isFinishedRolling = false;
    }

    public void PickDice(DiceType type)
    {
        foreach (Dice dice in diceList)
        {
            if (dice.GetDiceType() == type)
            {
                activeDice.RemoveAllListenersInEvent();

                activeDice = dice;
                dice.gameObject.SetActive(true);
            }
            else
            {
                dice.gameObject.SetActive(false);
            }
        }

        if (activeDice == null) 
        {
            Debug.Log("No specified die was found: " + type.ToString() + 
                "\nFrom the holder: " + gameObject.name);
        }
    }

    public void SubscribeToFinishedDiceRollEvent(Action methodThatWillSubscribe)
    {
        activeDice.OnFinishedDiceRollAnimationEvent += methodThatWillSubscribe;
        activeDice.OnFinishedDiceRollAnimationEvent += (() => isFinishedRolling = true);
    }

    public int RollDice()
    {
        recentDiceRollResult = (activeDice == null) ? -1 : activeDice.PerformDiceRoll();
        return recentDiceRollResult;
    }

    public int GetRecentDiceRollResult()
    { 
        return recentDiceRollResult;
    }

    public bool IsFinishedRolling()
    {
        return isFinishedRolling;
    }
}
