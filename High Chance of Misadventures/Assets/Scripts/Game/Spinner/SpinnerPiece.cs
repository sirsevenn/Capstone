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
    public Image lockImage;
    public Image lockOverlay;

    public bool isActive = true;
    public bool isLocked = true;

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

    public void ToggleLock()
    {
        if (isLocked)
        {
            UnlockPiece();
        }
        else if (!isLocked)
        {
            LockPiece();
        }
    }

    private void LockPiece()
    {
        if (lockImage != null)
        {
            lockImage.gameObject.SetActive(true);
        }

        if (lockOverlay != null)
        {
            lockOverlay.gameObject.SetActive(true);
        }

        isLocked = true;
    }

    private void UnlockPiece()
    {
        if (lockImage != null)
        {
            lockImage.gameObject.SetActive(false);
        }

        if (lockOverlay != null)
        {
            lockOverlay.gameObject.SetActive(false);
        }

        isLocked = false;
    }

    public void DeactivatePiece()
    {
        //icon.gameObject.SetActive(false);
        //pieceImage.gameObject.SetActive(false);
        highlight.SetActive(false);
        icon.gameObject.SetActive(false);
        pieceImage.gameObject.SetActive(false);
        isActive = false;
    }

    public void ActivatePiece()
    {
        
        isActive = true;
        icon.gameObject.SetActive(true);
        pieceImage.gameObject.SetActive(true);
        UnlockPiece();


    }

}
