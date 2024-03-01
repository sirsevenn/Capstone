using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Spinner : MonoBehaviour
{
    [Header("Spinner Pieces")]
    public SpinnerPiece[] spinnerPieces;

    //Other stuff
    private int[] pieceAngles = {0, 18, 54, 90, 126, 162, 198, 234, 270, 306, 342};

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spin(3);
        }
    }
    */

    public void Spin(float tweenDuration, bool isPlayer)
    {
        //OutCirc
        //OutElastic
        int randomAngle = Random.Range(1080, 1440);
        transform.DORotate(new Vector3(0, 0, randomAngle), tweenDuration, RotateMode.FastBeyond360).SetEase(Ease.OutCirc).OnComplete(() => OnSpinEnd(isPlayer));

    }

    public void OnSpinEnd(bool isPlayer)
    {

        //Calculate which piece spinner lands on
        float currentZRot = transform.rotation.eulerAngles.z;
        Debug.Log("Current Z Rot: " + currentZRot);

        //Turn into positive rotation
        while (currentZRot < 0)
        {
            currentZRot += 360;
        }

        int index = -1;
        //Check Piece
        if ((currentZRot >= pieceAngles[0] && currentZRot < pieceAngles[1]) || (currentZRot >= pieceAngles[10] && currentZRot < 360))
        {
            //spinner piece 0
            index = 0;
        }

        for (int i = 1; i < pieceAngles.Length - 1; i++)
        {
            if (currentZRot >= pieceAngles[i] && currentZRot < pieceAngles[i + 1])
            {
                index = i;
            }
        }

        LO_GameFlow_PVP.Instance.SetActions(spinnerPieces[index].actionType, isPlayer);

    }

    public void ChangeWheel(EnemyData probability)
    {
        int heavy = probability.heavyAttackProbability;
        int light = probability.lightAttackProbability;
        int parry = probability.parryAttackProbability;

        int total = heavy + light + parry;
        int counter = 0;

        for (int i = 0; i < total; i++)
        {
            if (counter == 0 && heavy > 0)
            {
                heavy--;
                spinnerPieces[i].ChangePiece(ActionType.Heavy, LO_UIManager_PVP.Instance.GetHeavyIcon(), LO_UIManager_PVP.Instance.GetHeavyColor());
       
            }
            else if (counter == 1 && light > 0)
            {
                light--;
                spinnerPieces[i].ChangePiece(ActionType.Light, LO_UIManager_PVP.Instance.GetLightIcon(), LO_UIManager_PVP.Instance.GetLightColor());
         
            }
            else if (counter == 2 && parry > 0)
            {
                parry--;
                spinnerPieces[i].ChangePiece(ActionType.Parry, LO_UIManager_PVP.Instance.GetParryIcon(), LO_UIManager_PVP.Instance.GetParryColor());

            }
            else
            {
                i--;
            }


            counter++;
            if (counter > 2)
            {
                counter = 0;
            }
        }

    }
}
