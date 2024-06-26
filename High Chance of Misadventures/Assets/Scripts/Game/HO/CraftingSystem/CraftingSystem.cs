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
    [SerializeField] private CraftingMaterialSO[] droppedMaterials;

    [Space(20)]
    [SerializeField] private List<ConsumableSO> consumablesToCraftList;
    [SerializeField] private List<int> weightsOnEachConsumableList;
    [SerializeField] private float totalWeight;

    [Space(10)] [Header("Other References")]
    [SerializeField] private CraftingSystemDisplay craftingDisplay;
    [SerializeField] private CraftingParticles particlesScript;
    [SerializeField] private PotionShelfRack middleShelfRack;
    [SerializeField] private PotionShelfRack leftShelfRack;


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

        weightsOnEachConsumableList = new();
        ResetCraftingTrackers();
    }


    #region Touch Input Event Methods
    public void OnBeginDragMaterial(CraftingMaterialSO draggedMaterial)
    {
        if (isDraggingMaterial || currentDraggedMaterial != null) return;

        isDraggingMaterial = true;
        currentDraggedMaterial = draggedMaterial;
        craftingDisplay.OnBeginDragMaterial(draggedMaterial.ItemIcon);
    }

    public void OnDragMaterial(CraftingMaterialSO draggedMaterial, Vector2 materialPos)
    {
        if (!isDraggingMaterial || currentDraggedMaterial != draggedMaterial) return;

        int index = craftingDisplay.GetSlotIndexFromHoveredMaterial(materialPos);
        bool isHighlighted = (index != -1) && (droppedMaterials[index] == null);

        craftingDisplay.OnDragMaterial(materialPos, index, isHighlighted);
    }

    public void OnEndDragMaterial(CraftingMaterialSO draggedMaterial, Vector2 materialPos)
    {
        if (!isDraggingMaterial || currentDraggedMaterial != draggedMaterial) return;

        // Determine if it is on top of an open slot
        int slotIndex = craftingDisplay.GetSlotIndexFromHoveredMaterial(materialPos);
        bool isValidDropMaterial = slotIndex != -1 && droppedMaterials[slotIndex] == null;

        if (isValidDropMaterial)
        {
            droppedMaterials[slotIndex] = draggedMaterial;

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
        if (!isCauldronFiredUp) return;

        // Reset inventory 
        InventorySystem.Instance.ResetConsumables();


        // Determine the amount of all consumables, then craft them
        List<int> amountForEachConsumableList = CalculateAmountOfConsumablesToCraft();

        for (int i = 0; i < consumablesToCraftList.Count; i++)
        {
            ConsumableSO consumable = consumablesToCraftList[i];

            for (int j = 0; j < amountForEachConsumableList[i]; j++)
            {
                uint id = (uint)InventorySystem.Instance.GetNextConsumableID();
                Consumable newConsumable = new Consumable(id, consumable);
                InventorySystem.Instance.AddConsumable(newConsumable);
            }
        }


        // Update both shelf rack; left - previous set of potions, middle - newly crafted set of potions
        leftShelfRack.UpdateShelfRack(middleShelfRack.GetNumActivePotions());
        
        Dictionary<EConsumableType, int> newPotionDisplay = new();
        for (int i = 0; i < consumablesToCraftList.Count; i++)
        {
            newPotionDisplay.Add(consumablesToCraftList[i].ConsumableType, amountForEachConsumableList[i]);
        }
        middleShelfRack.UpdateShelfRack(newPotionDisplay);


        // Reset selection slots and other trackers for crafting
        for (int i = 0; i < droppedMaterials.Length; i++)
        {
            droppedMaterials[i] = null;
        }

        ResetCraftingTrackers();


        // Reset properties from other scripts
        craftingDisplay.ResetCraftingUI();
        particlesScript.ResetAllParticles();
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

            Debug.Log("too much to craft " + diff);
        }
        else if (totalAmount < maxConsumablesToCraft)
        {
            int diff = totalAmount - maxConsumablesToCraft;
            //returnList[indexWithLowestAmount] -= diff;
            returnList[indexWithLowestAmount]++;

            Debug.Log("too less to craft " + diff);
        }

        return returnList;
    }

    private void ResetCraftingTrackers()
    {
        weightsOnEachConsumableList.Clear();
        for (int i = 0; i < consumablesToCraftList.Count; i++)
        {
            weightsOnEachConsumableList.Add(50);
        }

        totalWeight = 250f;
        isCauldronFiredUp = false;
    }
    #endregion


    public void TransitionToBattleScene()
    {
        HO_GameManager.Instance.TransitionToBattleScene();
    }
}
