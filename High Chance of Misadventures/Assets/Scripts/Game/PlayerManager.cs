using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private ActionType playerAction;
    [SerializeField] private GameObject JournalUI;
    public void PlayerAction(int type)
    {
        switch (type)
        {
            case 0:
                playerAction = ActionType.Attack;
                break;
            case 1:
                playerAction = ActionType.Defend;
                break;
            case 2:
                playerAction = ActionType.Skill;
                break;
        }

        //Start Combat

        CombatManager.Instance.StartComabat(playerAction);
        playerAction = ActionType.None;
    }
    
    public void ToggleJournal(){
        JournalUI.SetActive(!JournalUI.activeSelf);
    }
}
