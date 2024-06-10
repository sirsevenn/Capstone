using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [Header("Crafting Properties")]
    [SerializeField] private bool isDraggingMaterial;
    [SerializeField] private CraftingMaterialSO currentDraggedMaterial;

    [Space(20)]
    [SerializeField] private CraftingMaterialSO selectedBaseMaterial;
    [SerializeField] private CraftingMaterialSO[] selectedSupplementaryMaterials;

    [Space(10)]
    [SerializeField] private CraftableSO itemToCraft;
    [SerializeField] private float currentSuccessRate;
    [SerializeField] private float currentEffectModifier;
    [SerializeField] private int currentNumMaterials;

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
        isDraggingMaterial = false;
        currentDraggedMaterial = null;

        selectedBaseMaterial = null;
        selectedSupplementaryMaterials = new CraftingMaterialSO[] { 
            null,
            null,
            null
        };

        itemToCraft = null;
        currentSuccessRate = 0f;
        currentEffectModifier = 0f;
        currentNumMaterials = 0;
    }

    #region UI Event Methods
    public void OnBeginDragMaterial(CraftingMaterialSO draggedMaterial)
    {
        if (isDraggingMaterial || currentDraggedMaterial != null) return;

        isDraggingMaterial = true;
        currentDraggedMaterial = draggedMaterial;
        UI.OnBeginDragMaterial(draggedMaterial.ItemIcon);
    }

    public void OnDragMaterial(CraftingMaterialSO draggedMaterial, Vector2 materialPos)
    {
        if (!isDraggingMaterial || currentDraggedMaterial != draggedMaterial) return;

        UI.OnDragMaterial(materialPos);
    }

    public void OnEndDragMaterial(CraftingMaterialSO draggedMaterial, Vector2 materialPos)
    {
        if (!isDraggingMaterial || currentDraggedMaterial != draggedMaterial) return;

        // Check if there are enough materials to add
        int count = (selectedBaseMaterial != null && selectedBaseMaterial.MaterialType == draggedMaterial.MaterialType) ? 1 : 0;
        foreach (var selectedMaterial in selectedSupplementaryMaterials)
        {
            if (selectedMaterial != null && selectedMaterial.MaterialType == draggedMaterial.MaterialType) count++;
        }

        if (count >= InventorySystem.Instance.GetMaterialAmount(draggedMaterial.MaterialType)) return;


        // Determine on which slots was the material dropped onto
        int index = UI.IsDroppedMaterialOnBaseSlot(materialPos) ? 0 : UI.GetSupplementarySlotIndexFromHoveredMaterial(materialPos);

        if (index == 0 && selectedBaseMaterial == null)
        {
            selectedBaseMaterial = draggedMaterial;
            itemToCraft = draggedMaterial.ItemToCraft;

            currentSuccessRate += draggedMaterial.SuccessRate;
            currentNumMaterials++;

            // Update output UI if player has already discovered it
            Sprite icon = InventorySystem.Instance.IsCraftableDiscovered(itemToCraft) ? itemToCraft.ItemIcon : null;
            UI.SetIconOnOutputImage(icon);
            UI.SetSuccessRateBar(currentSuccessRate);
            UI.SetCraftingEffectBar(EEffectModifier.Unknown);
        }
        else if (selectedBaseMaterial != null && index != -1 && selectedSupplementaryMaterials[index - 1] == null)
        {
            selectedSupplementaryMaterials[index - 1] = draggedMaterial;

            currentSuccessRate *= currentNumMaterials;
            currentSuccessRate += draggedMaterial.SuccessRate;
            currentEffectModifier *= (currentNumMaterials - 1);
            currentEffectModifier += (int)draggedMaterial.CraftingEffect;

            currentNumMaterials++;
            currentSuccessRate /= currentNumMaterials;
            currentEffectModifier /= (currentNumMaterials - 1);

            UI.SetSuccessRateBar(currentSuccessRate);
            UI.SetCraftingEffectBar((EEffectModifier)Mathf.RoundToInt(currentEffectModifier));
        }
        else
        {
            index = -1;
        }

        UI.OnEndDragMaterial(index);
        currentDraggedMaterial = null;
        isDraggingMaterial = false;
    }

    public void OnCraft()
    {
        if (itemToCraft == null) return;

        // If craftable is an armor, see if it is already discovered
        if (itemToCraft is ArmorSO && 
            InventorySystem.Instance.IsCraftableDiscovered(itemToCraft)) return;

        // Check if crafting is successful
        float randNum = Random.Range(0f, 1f);
        bool isCraftingSuccessful = randNum < currentSuccessRate;
        int modifier = Mathf.RoundToInt(currentEffectModifier);

        // Craft item if it is successful
        if (isCraftingSuccessful && itemToCraft is ArmorSO)
        {
            CraftArmor((ArmorSO)itemToCraft);
        }
        else if (isCraftingSuccessful && itemToCraft is PotionSO)
        {
            CraftPotion((PotionSO)itemToCraft, (EEffectModifier)modifier);
        }
        else if (isCraftingSuccessful && itemToCraft is ScrollSpellSO)
        {
            CraftScroll((ScrollSpellSO)itemToCraft, (EEffectModifier)modifier);
        }

        // Recreate new selected material list WITH their total amount
        List<CraftingMaterial> selectedMaterialsWithAmountList = new() {
            new CraftingMaterial(selectedBaseMaterial, 1)
        };

        foreach (var selectedMaterial in selectedSupplementaryMaterials)
        {
            if (selectedMaterial == null) continue;

            // Add material to the amount list
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

        // Reduce the materials from the inventory
        foreach (var material in selectedMaterialsWithAmountList)
        {
            InventorySystem.Instance.ReduceMaterials(material);
        }

        // Reset selection slots and other trakers for crafting
        selectedBaseMaterial = null;
        for (int i = 0; i < selectedSupplementaryMaterials.Length; i++)
        {
            selectedSupplementaryMaterials[i] = null;
        }
        
        itemToCraft = null;
        currentSuccessRate = 0f;
        currentEffectModifier = 0f;
        currentNumMaterials = 0;

        // Reset UI
        UI.ResetCraftingUI();

        foreach (var material in selectedMaterialsWithAmountList)
        {
            CraftingMaterial materialInInventory = InventorySystem.Instance.GetCraftingMaterial(material.MaterialData.MaterialType);
            if (materialInInventory == null)
            {
                materialInInventory = new CraftingMaterial(material.MaterialData, 0);
            }
            UI.UpdateMaterialPanel(materialInInventory);
        }
    }

    private void CraftArmor(ArmorSO armorToCraft)
    {
        InventorySystem.Instance.UpgradeArmorPiece(armorToCraft);
        UI.UpdateArmorPanel(armorToCraft);
    }

    private void CraftPotion(PotionSO potionToCraft, EEffectModifier effectModifier)
    {
        uint id = (uint)InventorySystem.Instance.GetNextPotionID();
        Potion newPotion = new Potion(id, potionToCraft, effectModifier);
        InventorySystem.Instance.AddPotion(newPotion);
        InventorySystem.Instance.AddPotionToCatalogue(newPotion.PotionData.GetItemName());
        UI.AddNewPotionPanel(newPotion);
    }

    private void CraftScroll(ScrollSpellSO scrollToCraft, EEffectModifier effectModifier)
    {
        uint id = (uint)InventorySystem.Instance.GetNextScrollID();
        ScrollSpell newScroll = new ScrollSpell(id, scrollToCraft, effectModifier);
        InventorySystem.Instance.AddScroll(newScroll);
        InventorySystem.Instance.AddScrollToCatalogue(newScroll.ScrollData.GetItemName());
        UI.AddNewScrollPanel(newScroll);
    }
    #endregion
}
