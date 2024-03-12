using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class LO_CombatManager_PVP : BaseCombatManager
{

    [Header("Spinners")]
    [SerializeField] private float spinDuration;
    [SerializeField] private Spinner playerSpinner;
    [SerializeField] private Spinner enemySpinner;

    public void SpinWheels()
    {
        if (!readyCombat || currentEnemy == null)
        {
            return;
        }

        LO_UIManager_PVP.Instance.ResetSelectedPieces();
        readyCombat = false;
        playerSpinner.Spin(spinDuration * 0.5f, true);
        enemySpinner.Spin(spinDuration, false);
        StartCoroutine(GameUtilities.DelayFunction(StartCombat, spinDuration + 1.0f));
    }

    protected override void OnEnemyPreCombat()
    {
        base.OnEnemyPreCombat();
        LO_UIManager_PVP.Instance.GenerateInventoryPieces();
    }

    protected override void TriggerEndRoom()
    {
        base.TriggerEndRoom();
        LO_GameFlow_PVP.Instance.EndRoom();
    }



}
