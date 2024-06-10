using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CraftingRecipe 
{
    [SerializeField] private List<CraftingMaterial> ingredientsList;


    public List<CraftingMaterial> IngredientsList
    { 
        get { return ingredientsList; } 
        private set { ingredientsList = value; }
    }

    //public bool CheckIfRecipeContainsIngredient(CraftingMaterialType material)
    //{
    //    return recipe.Exists(x => x.MaterialData.MaterialType == material);
    //}
}