using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Text Colors")]
    [SerializeField] private Color heavyColor;
    [SerializeField] private Color lightColor;
    [SerializeField] private Color parryColor;

    [Header("Result Texts")]
    [TextArea(3,10)]
    [SerializeField] private string[] heavyText;
    [TextArea(3, 10)]
    [SerializeField] private string[] lightText;
    [TextArea(3, 10)]
    [SerializeField] private string[] parryText;

    [Header("Components")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject playerButtons;
    public TMP_Text dialogue;

    //This function changes the dialogue text to the result of the combat
    public void ChangeDialogue(CombatResult result)
    {

        switch (result)
        {
            case CombatResult.None:
                Debug.Log("NO RESULT");
                break;
            case CombatResult.PlayerHeavyWin:
                dialogue.text = heavyText[0];
                break;
            case CombatResult.PlayerHeavyTie:
                dialogue.text = heavyText[1];
                break;
            case CombatResult.PlayerHeavyLose:
                dialogue.text = heavyText[2];
                break;
            case CombatResult.PlayerLightWin:
                dialogue.text = lightText[0];
                break;
            case CombatResult.PlayerLightTie:
                dialogue.text = lightText[1];
                break;
            case CombatResult.PlayerLightLose:
                dialogue.text = lightText[2];
                break;
            case CombatResult.PlayerParryWin:
                dialogue.text = parryText[0];
                break;
            case CombatResult.PlayerParryTie:
                dialogue.text = parryText[1];
                break;
            case CombatResult.PlayerParryLose:
                dialogue.text = parryText[2];
                break;

        }

        OpenDialogueBox();

    }

    public void OpenDialogueBox()
    {
        dialogueBox.SetActive(true);
        playerButtons.SetActive(false);
    }

    public void OpenPlayerButtons()
    {
        dialogueBox.SetActive(false);
        playerButtons.SetActive(true);
    }

}
