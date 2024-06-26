using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice_SO", menuName = "ScriptableObjects/Deprecated/Dice")]
public class DiceSO : ScriptableObject
{
    [SerializeField] private DiceType diceType;

    [Space(10)]
    [Tooltip("Each index with addition of 1 corresponds to the resulting number/dice roll of each vector")]
    [SerializeField] private List<Vector3> diceNumbersToVectorsList = new();

    [Space(10)]
    [Tooltip("Dice rolls should be prerecorded along with its resulting number/dice roll at the end of the animation")]
    [SerializeField] private List<PrerecordedRoll> prerecordedRollsList = new();

    [Space(10)]
    [SerializeField] private string animationTriggerPrefix;
    [SerializeField] private string idleTrigger;


    public DiceType DiceType
    {
        get { return diceType; }
        private set { }
    }

    public uint Sides 
    {
        get { return UInt32.Parse(diceType.ToString().Replace("D", "")); } 
        private set { } 
    }

    public List<Vector3> DiceNumbersToVectorsList 
    { 
        get { return diceNumbersToVectorsList; }
        private set { } 
    }

    public List<PrerecordedRoll> PrerecordedRollsList 
    {
        get { return prerecordedRollsList; }
        private set { } 
    }

    public string AnimationTriggerPrefix
    { 
        get { return animationTriggerPrefix; } 
        private set { } 
    }

    public string IdleTrigger
    {
        get { return idleTrigger; }
        private set { }
    }
}
