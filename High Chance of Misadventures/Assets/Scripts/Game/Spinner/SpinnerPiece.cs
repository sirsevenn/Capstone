using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinnerPiece : MonoBehaviour
{
    public ActionType actionType;
    public Image icon;
    public Image pieceImage;
    public GameObject highlight;

    public bool isActive = true;

    public void ChangePiece(ActionType newType, Sprite newIcon, Color newColor)
    {
        actionType = newType;
        icon.sprite = newIcon;
        pieceImage.color = newColor;
    }

    public void SelectPiece()
    {
        highlight.SetActive(true);
    }

    public void DeselectPiece()
    {
        highlight.SetActive(false);
    }

    public void DeactivatePiece()
    {
        //icon.gameObject.SetActive(false);
        //pieceImage.gameObject.SetActive(false);
        highlight.SetActive(false);
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0.4f);
        pieceImage.color = new Color(pieceImage.color.r, pieceImage.color.g, pieceImage.color.b, 0.4f);
        isActive = false;
    }

    public void ActivatePiece()
    {
        
        isActive = true;
        icon.gameObject.SetActive(true);
        pieceImage.gameObject.SetActive(true);
        highlight.SetActive(true);
        
    }

    public void ResetPiece()
    {
        highlight.SetActive(false);
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1.0f);
        pieceImage.color = new Color(pieceImage.color.r, pieceImage.color.g, pieceImage.color.b, 1.0f);
        isActive = true;
    }

}
