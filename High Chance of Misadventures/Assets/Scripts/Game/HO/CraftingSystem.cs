using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [Header("Craftables Properties")]
    [SerializeField] private List<CraftableSO> craftablesList;

    [Space(10)] [Header("Crafting Properties")]
    [SerializeField] private bool isInSelectionMode;
    [SerializeField] private int selectedSlotIndex;
    [SerializeField] private CraftingMaterialSO[] selectedMaterialsList;

    [SerializeField] private List<CraftableSO> possibleCraftablesList;
    [SerializeField] private CraftableSO possibleCraftableOutput;

    [Space(10)] [Header("Other References")]
    [SerializeField] private CraftingSystemUI UI;


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
        isInSelectionMode = false;
        selectedSlotIndex = -1;

        selectedMaterialsList = new CraftingMaterialSO[] { 
            null,
            null,
            null
        };
        possibleCraftablesList.Clear();
        possibleCraftableOutput = null;
    }

    #region UI Event Methods
    public void OnClickCraftingSlot(int index)
    {
        // If nothing is currently selected
        if (!isInSelectionMode)
        {
            UI.SetHighlightOnCraftingSlot(index, true);
            isInSelectionMode = true;
            selectedSlotIndex = index;
        }
        // If wanted to change the selected slot
        else if (isInSelectionMode && selectedSlotIndex != index)
        {
            UI.SetHighlightOnCraftingSlot(index, true);
            UI.SetHighlightOnCraftingSlot(selectedSlotIndex, false);
            selectedSlotIndex = index;
        }
        // If wanted to remove the material from the selected slot
        else if (isInSelectionMode && selectedSlotIndex == index)
        {
            selectedMaterialsList[index] = null;
            UI.SetIconOnSelectedSlot(null, index);

            UI.SetHighlightOnCraftingSlot(index, false);
            isInSelectionMode = false;
            selectedSlotIndex = -1;

            CheckForPossibleCraftables(false);
        }
    }

    public void OnClickMaterial(CraftingMaterialSO material)
    {
        if (!isInSelectionMode) return;

        // Check if there is at least one of the specified material in the inventory
        if (!InventorySystem.Instance.HasMaterial(material.MaterialType)) return;

        // Check the number of selected materials if it exceeds the amount in the inventory
        CraftingMaterial materialToAdd = InventorySystem.Instance.GetCraftingMaterial(material.MaterialType);

        int selectedAmount = 1;
        foreach (var selectedMaterial in selectedMaterialsList)
        {
            if (selectedMaterial == null) continue;

            if (selectedMaterial.MaterialType == materialToAdd.MaterialData.MaterialType) 
            {
                selectedAmount++;
            }
        }

        if (selectedAmount > materialToAdd.Amount) return;

        // Replace current slot with the new material, then check for possible craftables from the combination
        selectedMaterialsList[selectedSlotIndex] = material;
        CheckForPossibleCraftables(true);

        // Update UI for the selected slot along with its status
        UI.SetIconOnSelectedSlot(material.MaterialIcon, selectedSlotIndex);
        UI.SetHighlightOnCraftingSlot(selectedSlotIndex, false);

        // Update selection status
        isInSelectionMode = false;
        selectedSlotIndex = -1;
    }

    public void OnCraft()
    {
        if (possibleCraftableOutput == null) return;

        // Get the child class of the Craftable; if it is an armor, see if it is already discovered
        ArmorSO armorToCraft = null;
        PotionSO potionToCraft = null;

        if (possibleCraftableOutput is ArmorSO)
        {
            armorToCraft = (ArmorSO)possibleCraftableOutput;
        }
        else if (possibleCraftableOutput is PotionSO)
        {
            potionToCraft = (PotionSO)possibleCraftableOutput;
        }
        else return;

        if (InventorySystem.Instance.IsCraftableDiscovered(armorToCraft)) return;

        // Perform dice rolls and see if crafting is a success
        // Also recreate new selected material list WITH their total amount
        bool isCraftingSuccessful = true;
        List<CraftingMaterial> selectedMaterialsWithAmountList = new();

        foreach (CraftingMaterialSO selectedMaterial in selectedMaterialsList)
        {
            if (selectedMaterial == null) continue;

            if (selectedMaterial.DiceType == DiceType.Unknown)
            {
                Debug.Log("Dice for " + selectedMaterial.GetMaterialName() + " is not assigned properly!");
                return;
            }

            // Determine outcome of the dice roll
            string sidesStr = selectedMaterial.DiceType.ToString().Replace("D", "");
            int sides = Int32.Parse(sidesStr);

            int maxSuccessNumber = (int)Math.Ceiling(sides * selectedMaterial.SuccessRate);
            int randNum = UnityEngine.Random.Range(1, sides + 1);

            isCraftingSuccessful = isCraftingSuccessful && (randNum <= maxSuccessNumber);

            // Update material list with the correct amount of the same material
            int index = selectedMaterialsWithAmountList.FindIndex(x => x.MaterialData.MaterialType == selectedMaterial.MaterialType);
            if (index == -1)
            {
                selectedMaterialsWithAmountList.Add(new CraftingMaterial(selectedMaterial, 1));
            }
            else
            {
                selectedMaterialsWithAmountList[index].Amount++;
            }
        }

        // Reduce the materials from the inventory, and add the new craftable to the inventory
        foreach (var material in selectedMaterialsWithAmountList)
        {
            InventorySystem.Instance.ReduceMaterials(material);
        }

        if (isCraftingSuccessful && armorToCraft != null)
        {
            InventorySystem.Instance.UpgradeArmorPiece(armorToCraft);
        }
        else if (isCraftingSuccessful && potionToCraft != null)
        {
            Potion newPotion = new Potion(potionToCraft, 1);
            InventorySystem.Instance.AddPotions(newPotion);
            InventorySystem.Instance.AddPotionToCatalogue(newPotion.PotionData.PotionType);
        }

        // Reset selection slots and trackers for possible craftables 
        for (int i = 0; i < selectedMaterialsList.Length; i++)
        {
            selectedMaterialsList[i] = null;
        }

        possibleCraftablesList.Clear();
        possibleCraftableOutput = null;

        // Reset UI
        UI.ResetIconsOnSelectedSlots();
        UI.SetIconOnOutputImage(null, false);

        foreach (var material in selectedMaterialsWithAmountList)
        {
            CraftingMaterial materialInInventory = InventorySystem.Instance.GetCraftingMaterial(material.MaterialData.MaterialType);
            if (materialInInventory == null)
            {
                materialInInventory = new CraftingMaterial(material.MaterialData, 0);
            }
            UI.UpdateMaterialPanel(materialInInventory);
        }

        if (isCraftingSuccessful && armorToCraft != null) 
        {
            UI.UpdateArmorPanel(armorToCraft);
        }
        else if (isCraftingSuccessful && potionToCraft != null) 
        {
            Potion newPotion = InventorySystem.Instance.GetPotion(potionToCraft.PotionType);
            UI.UpdatePotionPanel(newPotion);
        }

        Debug.Log((isCraftingSuccessful ? "Craft Successful" : "Craft Failed"));
    }
    #endregion

    #region Possible Craftables Methods
    private void CheckForPossibleCraftables(bool hasAddedNewMaterial)
    {
        // Recreate new selected material list WITH their total amount
        List<CraftingMaterial> selectedMaterialsWithAmountList = new();
        foreach (var selectedMaterial in selectedMaterialsList)
        {
            if (selectedMaterial == null) continue;

            int index = selectedMaterialsWithAmountList.FindIndex(x => x.MaterialData.MaterialType == selectedMaterial.MaterialType);
            if (index == -1)
            {
                selectedMaterialsWithAmountList.Add(new CraftingMaterial(selectedMaterial, 1));
            }
            else
            {
                selectedMaterialsWithAmountList[index].Amount++;
            }
        }

        // Reset output
        possibleCraftableOutput = null;

        // If 2nd or 3rd material has been added
        if (hasAddedNewMaterial && possibleCraftablesList.Count > 0)
        {
            List<CraftableSO> newList = new List<CraftableSO>(possibleCraftablesList);
            UpdatePossibleCraftablesList(newList, selectedMaterialsWithAmountList, false);
        }
        // Else if 1st material has been added or a material has been removed
        else
        {
            possibleCraftablesList.Clear();
            UpdatePossibleCraftablesList(craftablesList, selectedMaterialsWithAmountList, true);
        }

        // Update output UI if player has already discovered it
        if (InventorySystem.Instance.IsCraftableDiscovered(possibleCraftableOutput))
        {
            UI.SetIconOnOutputImage(possibleCraftableOutput.CraftableIcon, true);
        }
        else
        {
            UI.SetIconOnOutputImage(null, (possibleCraftableOutput != null));
        }
    }

    private void UpdatePossibleCraftablesList(List<CraftableSO> craftablesListToSearch, List<CraftingMaterial> materialsList, bool resetList)
    {
        foreach (var craftable in craftablesListToSearch)
        {
            bool isCraftable = false;

            foreach (var recipe in craftable.RecipesList)
            {
                bool hasFoundMatchOnRecipe = true;
                int numMaterialToIngredientMatch = 0;

                foreach (var selectedMaterial in materialsList)
                {
                    bool hasFoundMatchOnIngredient = false;

                    foreach (var ingredient in recipe.IngredientsList)
                    {
                        if (ingredient.MaterialData.MaterialType == selectedMaterial.MaterialData.MaterialType && 
                            selectedMaterial.Amount <= ingredient.Amount)
                        {
                            hasFoundMatchOnIngredient = true;
                            numMaterialToIngredientMatch += (ingredient.Amount == selectedMaterial.Amount) ? 1 : 0;
                            break;
                        }
                    }

                    if (!hasFoundMatchOnIngredient)
                    {
                        hasFoundMatchOnRecipe = false;
                        break;
                    }
                }

                if (numMaterialToIngredientMatch == recipe.IngredientsList.Count && numMaterialToIngredientMatch == materialsList.Count)
                {
                    possibleCraftableOutput = craftable;
                }

                isCraftable = isCraftable || hasFoundMatchOnRecipe;
            }

            if (!isCraftable && !resetList)
            {
                possibleCraftablesList.Remove(craftable);
            }
            else if (isCraftable && resetList)
            {
                possibleCraftablesList.Add(craftable);
            }
        }
    }
    #endregion
}
