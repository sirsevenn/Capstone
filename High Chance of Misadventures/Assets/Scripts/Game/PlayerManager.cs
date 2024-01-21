using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private ActionType playerAction;

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
}
