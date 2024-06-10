//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class CraftingSystemTemp : MonoBehaviour
//{
//    [Header("Craftables Properties")]
//    [SerializeField] private List<CraftableSO> craftablesList;

//    [Space(10)] [Header("Crafting Properties")]
//    [SerializeField] private bool isInSelectionMode;
//    [SerializeField] private int selectedSlotIndex;
//    [SerializeField] private CraftingMaterialSO[] selectedMaterialsList;

//    [Space(20)]
//    [SerializeField] private bool isCrafting;
//    //[SerializeField] private int numDisplayedDiceRolls;
//    [SerializeField] private List<CraftableSO> possibleCraftablesList;
//    [SerializeField] private CraftableSO possibleCraftableOutput;

//    [Space(20)]
//    [SerializeField] private GameObject screenPanel;
//    [SerializeField] private CauldronMixingHandler mixingScript;
//    [SerializeField] private bool isMaterialCraftingSuccessful;

//    [Space(10)] [Header("Other References")]
//    [SerializeField] private CraftingSystemUI UI;
//    [SerializeField] private List<DiceHolder> diceHoldersList;


//    #region Singleton
//    public static CraftingSystemTemp Instance;

//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(this.gameObject);
//        }
//        else
//        {
//            Instance = this;
//        }
//    }

//    private void OnDestroy()
//    {
//        if (Instance != null && Instance == this)
//        {
//            Destroy(this.gameObject);
//            mixingScript.OnFinishedMixingEvent -= OnFinishedMixingCauldron;
//        }
//    }
//    #endregion

//    private void Start()
//    {
//        isInSelectionMode = false;
//        selectedSlotIndex = -1;

//        isCrafting = false;
//        isMaterialCraftingSuccessful = false;
//        //numDisplayedDiceRolls = -1;

//        selectedMaterialsList = new CraftingMaterialSO[] { 
//            null,
//            null,
//            null
//        };
//        possibleCraftablesList.Clear();
//        possibleCraftableOutput = null;

//        mixingScript.OnFinishedMixingEvent += OnFinishedMixingCauldron;
//        mixingScript.enabled = false;
//        screenPanel.SetActive(false);
//    }

//    #region UI Event Methods
//    public void OnClickCraftingSlot(int index)
//    {
//        if (isCrafting) return;

//        // If nothing is currently selected
//        if (!isInSelectionMode)
//        {
//            UI.SetHighlightOnCraftingSlot(index, true);
//            isInSelectionMode = true;
//            selectedSlotIndex = index;
//        }
//        // If wanted to change the selected slot
//        else if (isInSelectionMode && selectedSlotIndex != index)
//        {
//            UI.SetHighlightOnCraftingSlot(index, true);
//            UI.SetHighlightOnCraftingSlot(selectedSlotIndex, false);
//            selectedSlotIndex = index;
//        }
//        // If wanted to remove the material from the selected slot
//        else if (isInSelectionMode && selectedSlotIndex == index)
//        {
//            selectedMaterialsList[index] = null;
//            UI.SetValuesOnSelectedSlot(null, index);

//            UI.SetHighlightOnCraftingSlot(index, false);
//            isInSelectionMode = false;
//            selectedSlotIndex = -1;

//            CheckForPossibleCraftables(true);
//        }
//    }

//    public void OnClickMaterial(CraftingMaterialSO material)
//    {
//        if (!isInSelectionMode || isCrafting) return;

//        // Check if there is at least one of the specified material in the inventory
//        if (!InventorySystem.Instance.HasMaterial(material.MaterialType)) return;

//        // Check the number of selected materials if it exceeds the amount in the inventory
//        CraftingMaterial materialToAdd = InventorySystem.Instance.GetCraftingMaterial(material.MaterialType);

//        int selectedAmount = 1;
//        foreach (var selectedMaterial in selectedMaterialsList)
//        {
//            if (selectedMaterial == null) continue;

//            if (selectedMaterial.MaterialType == materialToAdd.MaterialData.MaterialType) 
//            {
//                selectedAmount++;
//            }
//        }

//        if (selectedAmount > materialToAdd.Amount) return;

//        // Replace current slot with the new material, then check for possible craftables from the combination
//        bool hasRemovedMaterial = selectedMaterialsList[selectedSlotIndex] != null;
//        selectedMaterialsList[selectedSlotIndex] = material;
//        CheckForPossibleCraftables(hasRemovedMaterial);

//        // Update UI for the selected slot along with its status
//        UI.SetValuesOnSelectedSlot(material.MaterialIcon, selectedSlotIndex);
//        UI.SetHighlightOnCraftingSlot(selectedSlotIndex, false);

//        // Update selection status
//        isInSelectionMode = false;
//        selectedSlotIndex = -1;
//    }

//    public void OnCraft()
//    {
//        if (possibleCraftableOutput == null) return;

//        // If craftable is an armor, see if it is already discovered
//        ArmorSO armorToCraft = null;

//        if (possibleCraftableOutput is ArmorSO)
//        {
//            armorToCraft = (ArmorSO)possibleCraftableOutput;
//        }
//        else if (possibleCraftableOutput is not PotionSO) return;
//        if (InventorySystem.Instance.IsCraftableDiscovered(armorToCraft)) return;


//        // Perform dice rolls and see if crafting is a success
//        isMaterialCraftingSuccessful = true;

//        for (int i = 0; i < selectedMaterialsList.Length; i++)
//        {
//            CraftingMaterialSO selectedMaterial = selectedMaterialsList[i];

//            if (selectedMaterial == null) continue;

//            if (selectedMaterial.DiceType == DiceType.Unknown)
//            {
//                Debug.Log("Dice for " + selectedMaterial.GetMaterialName() + " is not assigned properly!");
//                return;
//            }

//            // Determine outcome of the dice roll
//            string sidesStr = selectedMaterial.DiceType.ToString().Replace("D", "");
//            int sides = Int32.Parse(sidesStr);

//            int maxSuccessNumber = (int)Math.Ceiling(sides * selectedMaterial.SuccessRate);
//            int diceRollResult = UnityEngine.Random.Range(1, sides + 1);

//            isMaterialCraftingSuccessful = isMaterialCraftingSuccessful && (diceRollResult <= maxSuccessNumber);
//        }

//        // Prepare the mixing phase
//        mixingScript.SetMixingType(possibleCraftableOutput.MixingType);
//        mixingScript.enabled = true;
//        screenPanel.SetActive(true);
//        UI.SetIconOnOutputImage(null, false);
//    }

//    public void OnFinishedMixingCauldron(bool isMixingSuccessful)
//    {
//        Debug.Log((isMaterialCraftingSuccessful && isMixingSuccessful ? "Craft Successful" : "Craft Failed"));

//        // Recreate new selected material list WITH their total amount
//        List<CraftingMaterial> selectedMaterialsWithAmountList = new();
//        foreach (var selectedMaterial in selectedMaterialsList)
//        {
//            if (selectedMaterial == null) continue;

//            int index = selectedMaterialsWithAmountList.FindIndex(x => x.MaterialData.MaterialType == selectedMaterial.MaterialType);
//            if (index == -1)
//            {
//                selectedMaterialsWithAmountList.Add(new CraftingMaterial(selectedMaterial, 1));
//            }
//            else
//            {
//                selectedMaterialsWithAmountList[index].Amount++;
//            }
//        }

//        // Get the child class of the Craftable
//        ArmorSO armorToCraft = null;
//        PotionSO potionToCraft = null;

//        if (possibleCraftableOutput is ArmorSO)
//        {
//            armorToCraft = (ArmorSO)possibleCraftableOutput;
//        }
//        else if (possibleCraftableOutput is PotionSO)
//        {
//            potionToCraft = (PotionSO)possibleCraftableOutput;
//        }

//        // Reduce the materials from the inventory, and add the new craftable to the inventory
//        foreach (var material in selectedMaterialsWithAmountList)
//        {
//            InventorySystem.Instance.ReduceMaterials(material);
//        }

//        if (isMaterialCraftingSuccessful && isMixingSuccessful && armorToCraft != null)
//        {
//            InventorySystem.Instance.UpgradeArmorPiece(armorToCraft);
//        }
//        else if (isMaterialCraftingSuccessful && isMixingSuccessful && potionToCraft != null)
//        {
//            Potion newPotion = new Potion(potionToCraft, 1);
//            InventorySystem.Instance.AddPotions(newPotion);
//            InventorySystem.Instance.AddPotionToCatalogue(newPotion.PotionData.PotionType);
//        }

//        // Reset selection slots and trackers for possible craftables 
//        for (int i = 0; i < selectedMaterialsList.Length; i++)
//        {
//            selectedMaterialsList[i] = null;
//        }

//        possibleCraftablesList.Clear();
//        possibleCraftableOutput = null;

//        // Reset UI
//        UI.ResetSelectedSlots();

//        foreach (var material in selectedMaterialsWithAmountList)
//        {
//            CraftingMaterial materialInInventory = InventorySystem.Instance.GetCraftingMaterial(material.MaterialData.MaterialType);
//            if (materialInInventory == null)
//            {
//                materialInInventory = new CraftingMaterial(material.MaterialData, 0);
//            }
//            UI.UpdateMaterialPanel(materialInInventory);
//        }

//        if (isMaterialCraftingSuccessful && isMixingSuccessful && armorToCraft != null)
//        {
//            UI.UpdateArmorPanel(armorToCraft);
//        }
//        else if (isMaterialCraftingSuccessful && isMixingSuccessful && potionToCraft != null)
//        {
//            Potion newPotion = InventorySystem.Instance.GetPotion(potionToCraft.PotionType);
//            UI.UpdatePotionPanel(newPotion);
//        }

//        // Disable mixing script and reenable UI interaction
//        mixingScript.enabled = false;
//        screenPanel.SetActive(false);

//        isMaterialCraftingSuccessful = false;
//    }

//    //public void OnCraft()
//    //{
//    //    if (possibleCraftableOutput == null) return;

//    //    // If craftable is an armor, see if it is already discovered
//    //    ArmorSO armorToCraft = null;
//    //    if (possibleCraftableOutput is ArmorSO)
//    //    {
//    //        armorToCraft = (ArmorSO)possibleCraftableOutput;
//    //    }
//    //    else if (possibleCraftableOutput is not PotionSO) return;
//    //    if (InventorySystem.Instance.IsCraftableDiscovered(armorToCraft)) return;

//    //    // Update crafting prooperties; Disable any other UI interactions
//    //    isCrafting = true;
//    //    isCraftSuccessful = false;
//    //    numDisplayedDiceRolls = 0;
//    //    UI.ResetIconsOnSelectedSlots();

//    //    // Perform dice rolls and see if crafting is a success
//    //    bool isCraftingSuccessful = true;

//    //    for (int i = 0; i < selectedMaterialsList.Length; i++)
//    //    {
//    //        CraftingMaterialSO selectedMaterial = selectedMaterialsList[i];

//    //        if (selectedMaterial == null) continue;

//    //        if (selectedMaterial.DiceType == DiceType.Unknown)
//    //        {
//    //            Debug.Log("Dice for " + selectedMaterial.GetMaterialName() + " is not assigned properly!");
//    //            return;
//    //        }

//    //        // Determine outcome of the dice roll
//    //        DiceHolder holder = diceHoldersList[i];
//    //        holder.PickDice(selectedMaterial.DiceType);
//    //        holder.SubscribeToFinishedDiceRollEvent(OnFinishedDiceRollEvent);

//    //        string sidesStr = selectedMaterial.DiceType.ToString().Replace("D", "");
//    //        int sides = Int32.Parse(sidesStr);

//    //        int maxSuccessNumber = (int)Math.Ceiling(sides * selectedMaterial.SuccessRate);
//    //        int diceRollResult = holder.RollDice();

//    //        isCraftingSuccessful = isCraftingSuccessful && (diceRollResult <= maxSuccessNumber);
//    //    }

//    //    Debug.Log((isCraftingSuccessful ? "Craft Successful" : "Craft Failed"));
//    //}
//    #endregion

//    #region Dice Roll and Crafting Methods
//    //private void OnFinishedDiceRollEvent()
//    //{
//    //    for (int i = 0; i < diceHoldersList.Count; i++)
//    //    {
//    //        DiceHolder holder = diceHoldersList[i];

//    //        if (holder.IsFinishedRolling())
//    //        {
//    //            UI.DisplayDiceRollDisplay(i, holder.GetRecentDiceRollResult(), OnDisplayDiceRoll);
//    //            holder.ResetDiceProperties();
//    //            break;
//    //        }
//    //    }
//    //}

//    //private void OnDisplayDiceRoll()
//    //{
//    //    numDisplayedDiceRolls++;

//    //    int numMaterials = 0;
//    //    foreach (var selectedMaterial in selectedMaterialsList)
//    //    {
//    //        if (selectedMaterial != null) numMaterials++;
//    //    }

//    //    if (numDisplayedDiceRolls == numMaterials)
//    //    {
//    //        CraftItem();
//    //    }
//    //}

//    //private void CraftItem()
//    //{
//    //    // Recreate new selected material list WITH their total amount
//    //    List<CraftingMaterial> selectedMaterialsWithAmountList = new();
//    //    foreach (var selectedMaterial in selectedMaterialsList)
//    //    {
//    //        if (selectedMaterial == null) continue;

//    //        int index = selectedMaterialsWithAmountList.FindIndex(x => x.MaterialData.MaterialType == selectedMaterial.MaterialType);
//    //        if (index == -1)
//    //        {
//    //            selectedMaterialsWithAmountList.Add(new CraftingMaterial(selectedMaterial, 1));
//    //        }
//    //        else
//    //        {
//    //            selectedMaterialsWithAmountList[index].Amount++;
//    //        }
//    //    }

//    //    // Get the child class of the Craftable
//    //    ArmorSO armorToCraft = null;
//    //    PotionSO potionToCraft = null;

//    //    if (possibleCraftableOutput is ArmorSO)
//    //    {
//    //        armorToCraft = (ArmorSO)possibleCraftableOutput;
//    //    }
//    //    else if (possibleCraftableOutput is PotionSO)
//    //    {
//    //        potionToCraft = (PotionSO)possibleCraftableOutput;
//    //    }

//    //    // Reduce the materials from the inventory, and add the new craftable to the inventory
//    //    foreach (var material in selectedMaterialsWithAmountList)
//    //    {
//    //        InventorySystem.Instance.ReduceMaterials(material);
//    //    }

//    //    if (isCraftSuccessful && armorToCraft != null)
//    //    {
//    //        InventorySystem.Instance.UpgradeArmorPiece(armorToCraft);
//    //    }
//    //    else if (isCraftSuccessful && potionToCraft != null)
//    //    {
//    //        Potion newPotion = new Potion(potionToCraft, 1);
//    //        InventorySystem.Instance.AddPotions(newPotion);
//    //        InventorySystem.Instance.AddPotionToCatalogue(newPotion.PotionData.PotionType);
//    //    }

//    //    // Reset selection slots and trackers for possible craftables 
//    //    for (int i = 0; i < selectedMaterialsList.Length; i++)
//    //    {
//    //        selectedMaterialsList[i] = null;
//    //    }

//    //    possibleCraftablesList.Clear();
//    //    possibleCraftableOutput = null;

//    //    // Reset/Update UI
//    //    UI.ResetIconsOnSelectedSlots();
//    //    UI.SetIconOnOutputImage(null, false);
//    //    UI.ResetDiceRollDisplays();

//    //    foreach (var material in selectedMaterialsWithAmountList)
//    //    {
//    //        CraftingMaterial materialInInventory = InventorySystem.Instance.GetCraftingMaterial(material.MaterialData.MaterialType);
//    //        if (materialInInventory == null)
//    //        {
//    //            materialInInventory = new CraftingMaterial(material.MaterialData, 0);
//    //        }
//    //        UI.UpdateMaterialPanel(materialInInventory);
//    //    }

//    //    if (isCraftSuccessful && armorToCraft != null)
//    //    {
//    //        UI.UpdateArmorPanel(armorToCraft);
//    //    }
//    //    else if (isCraftSuccessful && potionToCraft != null)
//    //    {
//    //        Potion newPotion = InventorySystem.Instance.GetPotion(potionToCraft.PotionType);
//    //        UI.UpdatePotionPanel(newPotion);
//    //    }
//    //}
//    #endregion

//    #region Possible Craftables Methods
//    private void CheckForPossibleCraftables(bool hasRemovedMaterial)
//    {
//        // Recreate new selected material list WITH their total amount
//        // Also get their overall success rate
//        List<CraftingMaterial> selectedMaterialsWithAmountList = new();
//        float successRate = 1f;

//        foreach (var selectedMaterial in selectedMaterialsList)
//        {
//            if (selectedMaterial == null) continue;

//            int index = selectedMaterialsWithAmountList.FindIndex(x => x.MaterialData.MaterialType == selectedMaterial.MaterialType);
//            if (index == -1)
//            {
//                selectedMaterialsWithAmountList.Add(new CraftingMaterial(selectedMaterial, 1));
//            }
//            else
//            {
//                selectedMaterialsWithAmountList[index].Amount++;
//            }

//            successRate *= selectedMaterial.SuccessRate;
//        }

//        // Reset output
//        possibleCraftableOutput = null;

//        // If 2nd or 3rd material has been added
//        if (!hasRemovedMaterial && possibleCraftablesList.Count > 0)
//        {
//            List<CraftableSO> newList = new List<CraftableSO>(possibleCraftablesList);
//            UpdatePossibleCraftablesList(newList, selectedMaterialsWithAmountList, false);
//        }
//        // Else if 1st material has been added or a material has been removed
//        else
//        {
//            possibleCraftablesList.Clear();
//            UpdatePossibleCraftablesList(craftablesList, selectedMaterialsWithAmountList, true);
//        }

//        // Update output UI if player has already discovered it
//        if (InventorySystem.Instance.IsCraftableDiscovered(possibleCraftableOutput))
//        {
//            UI.SetIconOnOutputImage(possibleCraftableOutput.CraftableIcon, true, successRate);
//        }
//        else
//        {
//            UI.SetIconOnOutputImage(null, (possibleCraftableOutput != null), successRate);
//        }
//    }

//    private void UpdatePossibleCraftablesList(List<CraftableSO> craftablesListToSearch, List<CraftingMaterial> materialsList, bool resetList)
//    {
//        foreach (var craftable in craftablesListToSearch)
//        {
//            bool isCraftable = false;

//            foreach (var recipe in craftable.RecipesList)
//            {
//                bool hasFoundMatchOnRecipe = true;
//                int numMaterialToIngredientMatch = 0;

//                foreach (var selectedMaterial in materialsList)
//                {
//                    bool hasFoundMatchOnIngredient = false;

//                    foreach (var ingredient in recipe.IngredientsList)
//                    {
//                        if (ingredient.MaterialData.MaterialType == selectedMaterial.MaterialData.MaterialType && 
//                            selectedMaterial.Amount <= ingredient.Amount)
//                        {
//                            hasFoundMatchOnIngredient = true;
//                            numMaterialToIngredientMatch += (ingredient.Amount == selectedMaterial.Amount) ? 1 : 0;
//                            break;
//                        }
//                    }

//                    if (!hasFoundMatchOnIngredient)
//                    {
//                        hasFoundMatchOnRecipe = false;
//                        break;
//                    }
//                }

//                if (numMaterialToIngredientMatch == recipe.IngredientsList.Count && numMaterialToIngredientMatch == materialsList.Count)
//                {
//                    possibleCraftableOutput = craftable;
//                }

//                isCraftable = isCraftable || hasFoundMatchOnRecipe;
//            }

//            if (!isCraftable && !resetList)
//            {
//                possibleCraftablesList.Remove(craftable);
//            }
//            else if (isCraftable && resetList)
//            {
//                possibleCraftablesList.Add(craftable);
//            }
//        }
//    }
//    #endregion
//}
