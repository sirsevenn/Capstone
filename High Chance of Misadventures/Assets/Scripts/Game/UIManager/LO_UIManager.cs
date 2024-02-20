using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LO_UIManager : UIManager
{
    #region singleton
    public static LO_UIManager Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;

    }
    #endregion

    [Header("UI GameObjects")]
    [SerializeField] private GameObject playerUI;

    [Header("TransformLocations")]
    [SerializeField] private Transform playerUIStartTransform;
    [SerializeField] private Transform playerUIEndTransform;

    [Header("Settings")]
    [SerializeField] private float tweenDuration;

    [Header("Animation Curves")]
    [SerializeField] private AnimationCurve fastOutBounce;

    [Header("InventoryPieces")]
    public List<SpinnerPiece> inventoryPieces = new List<SpinnerPiece>();

    [Header("Spinner Piece Selection Settings")]
    public SpinnerPiece selectedPieceFromInventory;
    public SpinnerPiece selectedPieceFromSpinner;

    public void ActivatePlayerUI()
    {
        playerUI.SetActive(true);
        playerUI.transform.DOMoveY(playerUIEndTransform.position.y, tweenDuration, true).SetEase(fastOutBounce);

    }

    public void SelectInventoryPiece(SpinnerPiece piece)
    {
        if (selectedPieceFromInventory)
        {
            selectedPieceFromInventory.DeselectPiece();
            selectedPieceFromInventory = null;
        }

        selectedPieceFromInventory = piece;

        if (selectedPieceFromInventory.isActive)
        {
            selectedPieceFromInventory.SelectPiece();
        }
        else
        {
            selectedPieceFromInventory = null;
        }
       
    }

    public void SelectSpinnerPiece(SpinnerPiece piece)
    {
        if (selectedPieceFromSpinner)
        {
            selectedPieceFromSpinner.DeselectPiece();
            selectedPieceFromSpinner = null;
        }

        selectedPieceFromSpinner = piece;

        if (selectedPieceFromSpinner.isActive)
        {
            selectedPieceFromSpinner.SelectPiece();
        }
        else
        {
            selectedPieceFromSpinner = null;
        }
    }

    public void ChangeSlice()
    {
        if (selectedPieceFromInventory != null && selectedPieceFromSpinner != null)
        {
            ActionType newType = selectedPieceFromInventory.actionType;
            Sprite newIcon = selectedPieceFromInventory.icon.sprite;
            Color newColor = selectedPieceFromInventory.pieceImage.color;

            selectedPieceFromSpinner.ChangePiece(newType, newIcon, newColor);

            selectedPieceFromInventory.DeactivatePiece();

            selectedPieceFromInventory.DeselectPiece();
            selectedPieceFromSpinner.DeselectPiece();

            selectedPieceFromSpinner = null;
            selectedPieceFromInventory = null;
        }

    }

    public void GenerateInventoryPieces()
    {
        ResetInventory();

        int nPieces = Random.Range(1, 4);

        for (int i = 0; i < inventoryPieces.Count; i++)
        {
            inventoryPieces[i].gameObject.SetActive(false);
            inventoryPieces[i].ResetPiece();
        }

        for (int i = 0; i < nPieces; i++)
        {
            int randomAction = Random.Range(0, 3);
            switch (randomAction)
            {
                case 0:
                    inventoryPieces[i].gameObject.SetActive(true);
                    inventoryPieces[i].actionType = ActionType.Heavy;
                    inventoryPieces[i].icon.sprite = heavyIcon;
                    inventoryPieces[i].pieceImage.color = heavyColor;
                    break;
                case 1:
                    inventoryPieces[i].gameObject.SetActive(true);
                    inventoryPieces[i].actionType = ActionType.Light;
                    inventoryPieces[i].icon.sprite = lightIcon;
                    inventoryPieces[i].pieceImage.color = lightColor;
                    break;
                case 2:
                    inventoryPieces[i].gameObject.SetActive(true);
                    inventoryPieces[i].actionType = ActionType.Parry;
                    inventoryPieces[i].icon.sprite = parryIcon;
                    inventoryPieces[i].pieceImage.color = parryColor;
                    break;
            }
        }
        
    }

    public void ResetInventory()
    {
        if (selectedPieceFromInventory)
        {
            selectedPieceFromInventory.DeselectPiece();
        }

        if (selectedPieceFromSpinner)
        {
            selectedPieceFromSpinner.DeselectPiece();
        }
        selectedPieceFromInventory = null;
        selectedPieceFromSpinner = null;
    }

}
