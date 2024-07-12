using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [Header("Dragging Properties")]
    [SerializeField] private bool isDraggingMaterial;
    [SerializeField] private CraftingMaterialSO currentDraggedMaterial;

    [Space(10)] [Header("Crafting Properties")]
    [SerializeField] private int maxConsumablesToCraft;
    [SerializeField] private int maxNumberPerConsumable;
    [SerializeField] private bool isCauldronFiredUp;

    [Space(10)]
    [SerializeField] private CraftingMaterialSO[] droppedMaterials;
    [SerializeField] private int currentNumDroppedMaterials;

    [Space(20)]
    [SerializeField] private List<ConsumableSO> consumablesToCraftList;
    [SerializeField] private List<int> weightsOnEachConsumableList;
    [SerializeField] private float totalWeight;

    [Space(10)] [Header("Other References")]
    [SerializeField] private bool areInputsEnabled;
    [SerializeField] private CraftingSystemDisplay craftingDisplay;
    [SerializeField] private CraftingParticles particlesScript;
    [SerializeField] private PotionShelfRackManager shelfRackManager;
    [SerializeField] private Collider leverCollider;
    [SerializeField] private CraftingExitHandler exitHandler;
    [SerializeField] private HO_TutorialHandler tutorialHandler;


    #region Singleton
    public static CraftingSystem Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance != null && Instance == this)
        {
            Destroy(this.gameObject);
            GestureManager.Instance.OnSwipeEvent -= OnSwipe;
        }
    }
    #endregion


    private void Start()
    {
        isDraggingMaterial = false;
        currentDraggedMaterial = null;

        maxConsumablesToCraft = (maxConsumablesToCraft == 0) ? 15 : maxConsumablesToCraft;
        maxNumberPerConsumable = (maxNumberPerConsumable == 0) ? 10 : maxNumberPerConsumable;

        droppedMaterials = new CraftingMaterialSO[]
        {
            null, null, null
        };
        currentNumDroppedMaterials = 0;

        weightsOnEachConsumableList = new();
        ResetCraftingTrackers();

        if (!HO_GameManager.Instance.HasFinishedTutorial())
        {
            InitiateTutorial();
        }
        else
        {
            areInputsEnabled = true;
        }

        GestureManager.Instance.OnSwipeEvent += OnSwipe;
    }


    #region Touch Input Event Methods
    private void OnSwipe(object send, SwipeEventArgs args)
    {
        if (!areInputsEnabled) return;

        if (args.SwipeDirection != SwipeEventArgs.SwipeDirections.LEFT && args.SwipeDirection != SwipeEventArgs.SwipeDirections.RIGHT) return;

        Ray ray = Camera.main.ScreenPointToRay(args.SwipePos);
        RaycastHit hit;

        if (leverCollider.Raycast(ray, out hit, 100f))
        {
            // Check for valid movement
            if (args.SwipeDirection == SwipeEventArgs.SwipeDirections.LEFT)
            {
                areInputsEnabled = false;
                craftingDisplay.OnEnableInputs(false);
                shelfRackManager.MoveShelfRacks(true);
            }
            else if (args.SwipeDirection == SwipeEventArgs.SwipeDirections.RIGHT && shelfRackManager.CanMoveShelfRacksToTheRight())
            {
                areInputsEnabled = false;
                craftingDisplay.OnEnableInputs(false);
                shelfRackManager.MoveShelfRacks(false);
            }

            // Update some UI based on the movement
            if (!shelfRackManager.IsMiddleShelfRackEmmpty())
            {
                ResetCraftingTrackers();
                particlesScript.ResetAllParticles();

                List<CraftingMaterialSO> usedMaterials = shelfRackManager.GetUsedMaterialsFromMiddleShelfRack();
                UpdateUsedMaterials(usedMaterials);
                craftingDisplay.DisplayUsedMaterials(usedMaterials);
            }
            else if (!isCauldronFiredUp)
            {
                ResetCraftingTrackers();
                craftingDisplay.ResetCraftingUI();
            }
        }
    }

    public void EnableInputs()
    {
        areInputsEnabled = true;
        craftingDisplay.OnEnableInputs(true);
    }

    public void OnBeginDragMaterial(CraftingMaterialSO draggedMaterial)
    {
        if (!areInputsEnabled) return;

        if (!shelfRackManager.IsMiddleShelfRackEmmpty()) return; 
        
        if (isDraggingMaterial || currentDraggedMaterial != null) return;

        int index = Array.IndexOf(droppedMaterials, draggedMaterial);
        if (index != -1) return;

        isDraggingMaterial = true;
        currentDraggedMaterial = draggedMaterial;
        craftingDisplay.OnBeginDragMaterial(draggedMaterial.ItemIcon);
    }

    public void OnDragMaterial(CraftingMaterialSO draggedMaterial, Vector2 materialPos)
    {
        if (!areInputsEnabled || 
            !shelfRackManager.IsMiddleShelfRackEmmpty() || 
            !isDraggingMaterial || currentDraggedMaterial != draggedMaterial) return;

        int index = craftingDisplay.GetSlotIndexFromHoveredMaterial(materialPos);
        bool isHighlighted = (index != -1) && (droppedMaterials[index] == null);

        craftingDisplay.OnDragMaterial(materialPos, index, isHighlighted);
    }

    public void OnEndDragMaterial(CraftingMaterialSO draggedMaterial, Vector2 materialPos)
    {
        if (!areInputsEnabled || 
            !shelfRackManager.IsMiddleShelfRackEmmpty() || 
            !isDraggingMaterial || currentDraggedMaterial != draggedMaterial) return;

        // Determine if it is on top of an open slot
        int slotIndex = craftingDisplay.GetSlotIndexFromHoveredMaterial(materialPos);
        bool isValidDropMaterial = slotIndex != -1 && droppedMaterials[slotIndex] == null;

        if (isValidDropMaterial)
        {
            droppedMaterials[slotIndex] = draggedMaterial;
            currentNumDroppedMaterials++;

            // Enable certain particles on the cauldron
            particlesScript.PlayMaterialParticle(slotIndex, draggedMaterial.ParticleMaterialColor);
            if (!isCauldronFiredUp)
            {
                particlesScript.PlayFireParticle();
                isCauldronFiredUp = true;
            }

            // Keep track of the consumables that the dropped material can craft
            foreach (var currentConsumableWeight in draggedMaterial.ConsumableWeightsList)
            {
                int consumableIndex = consumablesToCraftList.FindIndex(x => x.ConsumableType == currentConsumableWeight.ConsumableToCraft.ConsumableType);
                int effectValue = Consumable.ConvertEffectIntoValue(currentConsumableWeight.CraftingEffect);

                if (consumableIndex != -1)
                {
                    int consumableWeight = weightsOnEachConsumableList[consumableIndex] + effectValue;
                    if (consumableWeight < 0)
                    {
                        effectValue += -consumableWeight;
                    }

                    weightsOnEachConsumableList[consumableIndex] += effectValue;
                    totalWeight += effectValue;
                }
            }
        }

        // Cleanup
        craftingDisplay.OnEndDragMaterial(isValidDropMaterial);
        currentDraggedMaterial = null;
        isDraggingMaterial = false;
    }
    #endregion


    #region Crafting Methods
    public void OnCraft()
    {
        if (!areInputsEnabled || 
            !isCauldronFiredUp || 
            !shelfRackManager.IsMiddleShelfRackEmmpty() ||
            currentNumDroppedMaterials < 3) return;

        // Reset inventory and disable inputs
        InventorySystem.Instance.ResetConsumables();
        areInputsEnabled = false;
        craftingDisplay.OnEnableInputs(false);
        craftingDisplay.EnableBlockSelectionPanel();

        // Determine the amount of all potions, then add the to middle shelf rack
        List<int> amountForEachConsumableList = CalculateAmountOfConsumablesToCraft();
        Dictionary<EConsumableType, int> newPotionDisplay = new();

        for (int i = 0; i < consumablesToCraftList.Count; i++)
        {
            newPotionDisplay.Add(consumablesToCraftList[i].ConsumableType, amountForEachConsumableList[i]);
        }

        shelfRackManager.UpdateUsedMaterialsForMiddleShelfRack(new List<CraftingMaterialSO>(droppedMaterials));
        shelfRackManager.FillUpMiddleShelfRack(newPotionDisplay);

        // Reset particles
        particlesScript.ResetAllParticles();
        isCauldronFiredUp = false;

        // Reset some trackers
        currentNumDroppedMaterials = 0;
    }

    private List<int> CalculateAmountOfConsumablesToCraft()
    {
        List<int> returnList = new();
        int totalAmount = 0; 
        int indexWithHighestAmount = -1;
        int indexWithLowestAmount = -1;
        float percentOfOneAmount = 1f / (float)maxConsumablesToCraft;

        for (int i = 0; i < weightsOnEachConsumableList.Count; i++)
        {
            // Determine amount of the same item to craft based on its weight percentage
            float percentage = (float)weightsOnEachConsumableList[i] / totalWeight;
            int amount = Mathf.RoundToInt(percentage / percentOfOneAmount);

            if (amount > maxNumberPerConsumable)
            {
                amount -= amount - maxNumberPerConsumable; // remove excess
            }

            totalAmount += amount;
            returnList.Add(amount);

            if (indexWithHighestAmount == -1 || amount > returnList[indexWithHighestAmount])
            {
                indexWithHighestAmount = i;
            }

            if (amount != 0 && (indexWithLowestAmount == -1 || amount < returnList[indexWithLowestAmount]))
            {
                indexWithLowestAmount = i;
            }
        }

        if (totalAmount > maxConsumablesToCraft)
        {
            int diff = totalAmount - maxConsumablesToCraft;
            //returnList[indexWithHighestAmount] -= diff;
            returnList[indexWithHighestAmount]--;

            //Debug.Log("too much to craft " + diff);
        }
        else if (totalAmount < maxConsumablesToCraft)
        {
            int diff = totalAmount - maxConsumablesToCraft;
            //returnList[indexWithLowestAmount] -= diff;
            returnList[indexWithLowestAmount]++;

            //Debug.Log("too less to craft " + diff);
        }

        return returnList;
    }

    private void ResetCraftingTrackers()
    {
        for (int i = 0; i < droppedMaterials.Length; i++)
        {
            droppedMaterials[i] = null;
        }

        weightsOnEachConsumableList.Clear();
        for (int i = 0; i < consumablesToCraftList.Count; i++)
        {
            weightsOnEachConsumableList.Add(50);
        }

        totalWeight = 250f;
        isCauldronFiredUp = false;
        currentNumDroppedMaterials = 0;
    }

    private void UpdateUsedMaterials(List<CraftingMaterialSO> usedConsumables)
    {
        for(int i = 0; i < usedConsumables.Count; i++)
        {
            droppedMaterials[i] = usedConsumables[i];
        }
    }
    #endregion

    public void InitiateTutorial()
    {
        areInputsEnabled = false;
        craftingDisplay.OnEnableInputs(false);
        tutorialHandler.InitiateTutorial();
    }

    public void TransitionToBattleScene()
    {
        if (!areInputsEnabled) return;

        // Add potions to inventory
        Dictionary<EConsumableType, int> numPotions = shelfRackManager.GetNumPotionsFromMiddleShelfRack();

        for (int i = 0; i < consumablesToCraftList.Count; i++)
        {
            ConsumableSO consumable = consumablesToCraftList[i];

            for (int j = 0; j < numPotions[consumable.ConsumableType]; j++)
            {
                uint id = (uint)InventorySystem.Instance.GetNextConsumableID();
                Consumable newConsumable = new Consumable(id, consumable);
                InventorySystem.Instance.AddConsumable(newConsumable);
            }
        }

        craftingDisplay.DisableUI();
        exitHandler.OnBeginExitFromCraftingScene();
    }
}
