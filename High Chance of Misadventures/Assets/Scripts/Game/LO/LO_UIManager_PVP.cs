using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class LO_UIManager_PVP : UIManager
{
    #region singleton
    public static LO_UIManager_PVP Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;

    }
    #endregion

    [Header("Round Display")]
    [SerializeField] private GameObject roundDisplay;
    [SerializeField] private TMP_Text currentRoomText;
    [SerializeField] private TMP_Text maxRoomText;

    [Header("SideUI")]
    [SerializeField] private SideBarManager sideBar;

    [Header("UI GameObjects")]
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject enemyUI;
    [SerializeField] private GameObject playerHPUI;
    [SerializeField] private GameObject endScreen;

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

    [Header("End Screen Dialogue")]
    public List<string> endScreenDialogue = new List<string>();
    [SerializeField] private TMP_Text endScreenText;

    [Header("UI Raycast")]
    [SerializeField] protected GraphicRaycaster m_Raycaster;
    [SerializeField] protected PointerEventData m_PointerEventData;
    [SerializeField] protected EventSystem m_EventSystem;
    [SerializeField] protected SpinnerPiece pieceDisplay;
    [SerializeField] protected SpinnerPiece swapPiece;
    [SerializeField] protected SpinnerPiece inventoryPiece;

    [Header("LockMode")]
    [SerializeField] private Image lockIcon;
    public bool lockMode = false;
    [SerializeField] private Color lockedColor;
    [SerializeField] private Color defaultColor;

    [Header("Touch Controls")]

    [SerializeField] protected Vector2 touchStartPos;
    [SerializeField] protected Vector2 touchEndPos;
    [Space(10)]
    [SerializeField] protected float touchTime = 0;
    [SerializeField] protected float minHoldDuration = 0.3f;

    [SerializeField] protected bool isDragging = false;
    [SerializeField] protected bool startedTouch = false;
    [SerializeField] protected bool triggerHold = false;
    [SerializeField] protected bool swapped = false;

    public void ToggleLockMode()
    {
        //Add Visuals
        if (lockMode)
        {
            lockMode = false;
            lockIcon.color = defaultColor;
        }
        else if (!lockMode)
        {
            if (inventoryPiece != null)
            {
                inventoryPiece.DeselectPiece();
                inventoryPiece = null;
            }
            lockMode = true;
            lockIcon.color = lockedColor;
        }
    }

    public void DisplayRoundCount(int currentRoom, int maxRoom)
    {
        int room = currentRoom + 1;
        currentRoomText.text = room.ToString();
        maxRoomText.text = maxRoom.ToString();
        roundDisplay.SetActive(true);

        StartCoroutine(GameUtilities.DelayFunction(CloseDisplayRoundCount, 3));
    }

    private void CloseDisplayRoundCount()
    {
        roundDisplay.SetActive(false);
    }

    protected virtual void Tap(Vector2 position)
    {
        //Debug.Log("Tap!");
        SpinnerPiece spinnerPiece = RaycastUI();

        if (spinnerPiece == null)
        {
            return;
        }

        //Lock Pieces
        if (lockMode)
        {
            if (spinnerPiece.isActive && spinnerPiece.CompareTag("InventoryPiece"))
            {
                spinnerPiece.ToggleLock();
            }
        }
        //Select Pieces
        else if (!lockMode)
        {
            if (spinnerPiece.isActive && spinnerPiece.CompareTag("InventoryPiece") && !spinnerPiece.isLocked)
            {
                if (inventoryPiece != null)
                {
                    inventoryPiece.DeselectPiece();
                }

                inventoryPiece = spinnerPiece;
                inventoryPiece.SelectPiece();
            }
            else if (spinnerPiece.isActive && spinnerPiece.CompareTag("PlayerPiece"))
            {
                if (swapPiece != null)
                {
                    swapPiece.DeselectPiece();
                }

                swapPiece = spinnerPiece;
                swapPiece.SelectPiece();
            }

            if (swapPiece != null && inventoryPiece != null)
            {
                switch (inventoryPiece.actionType)
                {
                    case ActionType.Fire:
                        InventoryManager.Instance.DeductItem(0);
                        break;
                    case ActionType.Earth:
                        InventoryManager.Instance.DeductItem(1);
                        break;
                    case ActionType.Water:
                        InventoryManager.Instance.DeductItem(2);
                        break;
                }

                sideBar.UpdateInventory();

                swapPiece.actionType = inventoryPiece.actionType;
                swapPiece.icon.sprite = inventoryPiece.icon.sprite;
                swapPiece.pieceImage.color = inventoryPiece.pieceImage.color;
                swapPiece.DeselectPiece();

                inventoryPiece.DeactivatePiece();
                inventoryPiece = null;
                swapPiece = null;
                swapped = true;
            }
            
        }
       

    }

    protected virtual void Hold(Vector2 position)
    {
        //Debug.Log("Hold!");

        // Draggable Piece
        if (inventoryPiece != null)
        {
            inventoryPiece.DeselectPiece();
        }

        inventoryPiece = RaycastUI();
        if (inventoryPiece != null)
        {
            if (!inventoryPiece.isLocked && inventoryPiece.CompareTag("InventoryPiece"))
            {
                inventoryPiece.DeactivatePiece();
                pieceDisplay.ChangePiece(inventoryPiece.actionType, inventoryPiece.icon.sprite, inventoryPiece.pieceImage.color);
                pieceDisplay.transform.position = new Vector3(position.x, position.y, 0);
                pieceDisplay.gameObject.SetActive(true);
                isDragging = true;
            }
        }
    }

    protected void TrySwap()
    {
        //Debug.Log("Try Swap!");
        swapPiece = RaycastUI();
        if (swapPiece != null && inventoryPiece != null)
        {
            if (swapPiece.CompareTag("PlayerPiece"))
            {
                switch (inventoryPiece.actionType)
                {
                    case ActionType.Fire:
                        InventoryManager.Instance.DeductItem(0);
                        break;
                    case ActionType.Earth:
                        InventoryManager.Instance.DeductItem(1);
                        break;
                    case ActionType.Water:
                        InventoryManager.Instance.DeductItem(2);
                        break;
                }

                sideBar.UpdateInventory();

                swapPiece.actionType = inventoryPiece.actionType;
                swapPiece.icon.sprite = inventoryPiece.icon.sprite;
                swapPiece.pieceImage.color = inventoryPiece.pieceImage.color;
                swapPiece.DeselectPiece();

                inventoryPiece = null;
                swapPiece = null;

                swapped = true;
            }
            
        }
    }

    protected void Drag()
    {
        SpinnerPiece raycastPiece = RaycastUI();
        if (raycastPiece != null)
        {

            if (raycastPiece.CompareTag("PlayerPiece"))
            {
                if (swapPiece == null)
                {
                    swapPiece = raycastPiece;
                    swapPiece.SelectPiece();
                }
            }
           
        }
        else if (raycastPiece == null)
        {
            if (swapPiece != null)
            {
                swapPiece.DeselectPiece();
            }
            
            swapPiece = null;
        }
    }

    protected SpinnerPiece RaycastUI()
    {
        m_PointerEventData = new PointerEventData(m_EventSystem);

        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        m_Raycaster.Raycast(m_PointerEventData, results);
        if (results.Count > 0)
        {
            //Debug.Log("Hit " + results[0].gameObject.name);
            SpinnerPiece inventoryPiece = null;
            results[0].gameObject.TryGetComponent<SpinnerPiece>(out inventoryPiece);
            return inventoryPiece;
        }

        Debug.Log("Returned Null");
        return null;
    }

    protected void Update()
    {
        if (!LO_GameFlow_PVP.Instance.combatManager.readyCombat)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            startedTouch = true;
            //inventoryPiece = null;
            //swapPiece = null;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (touchTime < minHoldDuration)
            {
                Tap(Input.mousePosition);
            }

            if (triggerHold && inventoryPiece != null)
            {
                //Try Swap Piece
                TrySwap();
            }

            if (!swapped)
            {
                if (inventoryPiece != null)
                {
                    inventoryPiece.ActivatePiece();
                }

            }

            pieceDisplay.gameObject.SetActive(false);

            touchTime = 0;
            startedTouch = false;
            swapped = false;
            triggerHold = false;
            isDragging = false;
            //inventoryPiece = null;
            //swapPiece = null;
            
        }

        if (startedTouch)
        {
            touchTime += Time.deltaTime;

            if (touchTime > minHoldDuration && !triggerHold)
            {
                Hold(Input.mousePosition);
                triggerHold = true;
            }

        }

        if (isDragging)
        {
            Drag();
            pieceDisplay.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        }
    }

    public void ActivatePlayerUI()
    {
        playerUI.SetActive(true);
        playerUI.transform.DOMoveY(playerUIEndTransform.position.y, tweenDuration, true).SetEase(fastOutBounce);

    }

    public void EndGame(bool victory)
    {
        playerUI.SetActive(false);
        enemyUI.SetActive(false);
        playerHPUI.SetActive(false);

        endScreen.SetActive(true);

        if (victory)
        {
            endScreenText.text = endScreenDialogue[0];
        }
        else
        {
            endScreenText.text = endScreenDialogue[1];
        }
       
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
        ResetSelectedPieces();

        for (int i = 0; i < inventoryPieces.Count; i++)
        {
            inventoryPieces[i].DeactivatePiece();
        }
        int tempRed = 0;
        int tempGreen = 0;
        int tempBlue = 0;

        for (int i = 0; i < inventoryPieces.Count; i++)
        {
            if (!inventoryPieces[i].isLocked)
            {
                bool valid = false;
                bool checkedRed = false;
                bool checkedBlue = false;
                bool checkedGreen = false;
                int index = -1;
                while (!valid && (!checkedRed || !checkedGreen || !checkedBlue))
                {
                    int randomAction = Random.Range(0, 3);
                    switch (randomAction)
                    {
                        case 0:
                            if (tempRed + 1 <= InventoryManager.Instance.RedPieces) { 
                                tempRed++; 
                                valid = true;
                                index = randomAction;
                            }
                            else
                            {
                                checkedRed = true;
                            }
                            break;
                        case 1:
                            if (tempGreen + 1 <= InventoryManager.Instance.GreenPieces) 
                            { 
                                tempGreen++; 
                                valid = true;
                                index = randomAction;
                            }
                            else
                            {
                                checkedGreen = true;
                            }

                            break;
                        case 2:
                            if (tempBlue + 1 <= InventoryManager.Instance.BluePieces) 
                            { 
                                tempBlue++; 
                                valid = true;
                                index = randomAction;
                            }
                            else
                            {
                                checkedBlue = true;
                            }
                            break;
                    }
                }
               
                //Check if we have enough piece from inven to add


                switch (index)
                {
                    case 0:
                        inventoryPieces[i].ActivatePiece();
                        inventoryPieces[i].actionType = ActionType.Fire;
                        inventoryPieces[i].icon.sprite = fireIcon;
                        inventoryPieces[i].pieceImage.color = heavyColor;
                        break;
                    case 1:
                        inventoryPieces[i].ActivatePiece();
                        inventoryPieces[i].actionType = ActionType.Earth;
                        inventoryPieces[i].icon.sprite = earthIcon;
                        inventoryPieces[i].pieceImage.color = lightColor;
                        break;
                    case 2:
                        inventoryPieces[i].ActivatePiece();
                        inventoryPieces[i].actionType = ActionType.Water;
                        inventoryPieces[i].icon.sprite = waterIcon;
                        inventoryPieces[i].pieceImage.color = parryColor;
                        break;
                }
            }
            else if (inventoryPieces[i].isLocked)
            {
                inventoryPieces[i].ActivatePiece();
            }
            
        }
        
    }

    public void ResetSelectedPieces()
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
