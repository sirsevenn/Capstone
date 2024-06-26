using Cinemachine;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HO_UIManager : UIManager
{
    public static HO_UIManager Instance { get; private set; }

#region
    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;

    }
    #endregion

    [Header("Other Properties")]
    [SerializeField] private List<Button> attackButtonsList;

    [Space(10)]
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private GameObject enemyAttackPopUp;
    [SerializeField] private Image popUpIcon;
    [SerializeField] private Image popUpBG;


    public void Start()
    {
        enemyAttackPopUp.SetActive(false);

        SetActiveAttackButtons(false);
    }

    public void SetActiveAttackButtons(bool value)
    {
        foreach (var button in attackButtonsList)
        {
            button.interactable = value;

            // reset button state, it gets stuck as highlighted as some point
            button.enabled = false;
            button.enabled = true;
        }
    }

    public void ActivatePopUp(ActionType attackType, Vector3 pos)
    {
        Sprite icon = null;
        Color color = Color.white;

        switch (attackType)
        {
            case ActionType.Fire:
                icon = fireIcon;
                color = heavyColor;
                break;
            case ActionType.Earth:
                icon = earthIcon;
                color = lightColor;
                break;
            case ActionType.Water:
                icon = waterIcon;
                color = parryColor;
                break;
            default:
                return;
        }

        enemyAttackPopUp.SetActive(true);
        popUpIcon.sprite = icon;
        popUpBG.color = color;

        enemyAttackPopUp.transform.LookAt(mainCamera.transform);
        enemyAttackPopUp.transform.position = new Vector3(pos.x, pos.y + 0.5f, pos.z);
        float yDelta = enemyAttackPopUp.transform.position.y + 2;
        enemyAttackPopUp.transform.DOMoveY(yDelta, 1f, false).SetEase(Ease.Linear).OnComplete(OnCompletePopUp);
    }

    private void OnCompletePopUp()
    {
        enemyAttackPopUp.SetActive(false);
    }
}
