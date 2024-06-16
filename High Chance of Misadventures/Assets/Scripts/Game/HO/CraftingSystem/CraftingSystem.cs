using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CraftingSystem : MonoBehaviour
{
    [Header("Dragging Properties")]
    [SerializeField] private bool isDraggingMaterial;
    [SerializeField] private CraftingMaterialSO currentDraggedMaterial;

    [Space(10)] [Header("Crafting Properties")]
    [SerializeField] private CraftingMaterialSO selectedBaseMaterial;
    [SerializeField] private List<CraftingMaterialSO> selectedSupplementaryMaterials;

    [Space(20)]
    [SerializeField] private CraftableSO itemToCraft;
    [SerializeField] private uint currentFavorableOutcomes;
    [SerializeField] private float currentSuccessRate;
    [SerializeField] private int currentTotalEffectValue;

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
        selectedSupplementaryMaterials = new();
        selectedSupplementaryMaterials.Clear();

        itemToCraft = null;
        currentFavorableOutcomes = 0;
        currentSuccessRate = 0f;
        currentTotalEffectValue = 0;
    }

    #region Dragging Event Methods
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
        int count = 1; 
        count += (selectedBaseMaterial != null && selectedBaseMaterial.MaterialType == draggedMaterial.MaterialType) ? 1 : 0;

        foreach (var selectedMaterial in selectedSupplementaryMaterials)
        {
            if (selectedMaterial != null && selectedMaterial.MaterialType == draggedMaterial.MaterialType) count++;
        }

        bool hasEnoughMaterials = (count <= InventorySystem.Instance.GetMaterialAmount(draggedMaterial.MaterialType));

        // Determine if dropped material is on base or supplementary slots
        if (hasEnoughMaterials && UI.IsDroppedMaterialOnBaseSlot(materialPos) && selectedBaseMaterial == null)
        {
            selectedBaseMaterial = draggedMaterial;
            itemToCraft = draggedMaterial.ItemToCraft;

            // Update output UI if player has already discovered it
            Sprite icon = InventorySystem.Instance.IsCraftableDiscovered(itemToCraft) ? itemToCraft.ItemIcon : null;
            UI.SetIconOnOutputImage(icon);
            UI.SetSuccessRateBar(currentSuccessRate);
            UI.SetCraftingEffectBar(EEffectModifier.Unknown);
        }
        else if (hasEnoughMaterials && selectedBaseMaterial != null && UI.IsDroppedMaterialOnCauldron(materialPos))
        {
            // Add material to list
            selectedSupplementaryMaterials.Add(draggedMaterial);

            // Determine success rate and update UI
            currentFavorableOutcomes += draggedMaterial.SupplementaryAmount;
            currentSuccessRate = (float)currentFavorableOutcomes / (float)selectedBaseMaterial.BaseProbabilityValue;
            currentSuccessRate = (currentSuccessRate > 1) ? 1 : currentSuccessRate;
            UI.SetSuccessRateBar(currentSuccessRate);

            // Calculate total effect value and update UI
            if (itemToCraft is PotionSO)
            {
                PotionSO potionToCraft = (PotionSO)itemToCraft;
                currentTotalEffectValue += potionToCraft.GetEffectValueFromModifier(draggedMaterial.CraftingEffect);
                UI.SetCraftingEffectBar(potionToCraft.GetModifierFromEffectValue(currentTotalEffectValue));
            }
            else if (itemToCraft is ScrollSpellSO)
            {
                ScrollSpellSO scrollToCraft = (ScrollSpellSO)itemToCraft;
                currentTotalEffectValue += scrollToCraft.GetEffectValueFromModifier(draggedMaterial.CraftingEffect);
                UI.SetCraftingEffectBar(scrollToCraft.GetModifierFromEffectValue(currentTotalEffectValue));
            }
        }

        UI.OnEndDragMaterial();
        currentDraggedMaterial = null;
        isDraggingMaterial = false;
    }
    #endregion

    #region Crafting Methods
    public void OnCraft()
    {
        if (itemToCraft == null) return;

        // If craftable is an armor, see if it is already discovered
        //if (itemToCraft is ArmorSO && InventorySystem.Instance.IsCraftableDiscovered(itemToCraft)) return;

        // Check if crafting is successful 
        float randNum = Random.Range(0f, 1f);
        bool isCraftingSuccessful = randNum <= currentSuccessRate;

        // Craft item if it is successful
        //if (isCraftingSuccessful && itemToCraft is ArmorSO)
        //{
        //    CraftArmor((ArmorSO)itemToCraft);
        //}
        if (isCraftingSuccessful && itemToCraft is PotionSO)
        {
            CraftPotion((PotionSO)itemToCraft);
        }
        else if (isCraftingSuccessful && itemToCraft is ScrollSpellSO)
        {
            CraftScroll((ScrollSpellSO)itemToCraft);
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

        // Reset selection slots and other trackers for crafting
        selectedBaseMaterial = null;
        selectedSupplementaryMaterials.Clear();
        
        itemToCraft = null;
        currentFavorableOutcomes = 0;
        currentSuccessRate = 0f;
        currentTotalEffectValue = 0;

        // Reset UI
        UI.ResetCraftingUI();
    }

    //private void CraftArmor(ArmorSO armorToCraft)
    //{
    //    InventorySystem.Instance.UpgradeArmorPiece(armorToCraft);
    //}

    private void CraftPotion(PotionSO potionToCraft)
    {
        uint id = (uint)InventorySystem.Instance.GetNextPotionID();
        Potion newPotion = new Potion(id, currentTotalEffectValue, potionToCraft);
        InventorySystem.Instance.AddPotion(newPotion);
        InventorySystem.Instance.AddPotionToCatalogue(newPotion.PotionData.GetItemName());
    }

    private void CraftScroll(ScrollSpellSO scrollToCraft)
    {
        uint id = (uint)InventorySystem.Instance.GetNextScrollID();
        ScrollSpell newScroll = new ScrollSpell(id, currentTotalEffectValue, scrollToCraft);
        InventorySystem.Instance.AddScroll(newScroll);
        InventorySystem.Instance.AddScrollToCatalogue(newScroll.ScrollData.GetItemName());
    }
    #endregion

    public void TransitionToBattleScene()
    {
        SceneManager.LoadScene("AutoBattleScene");
    }
}
