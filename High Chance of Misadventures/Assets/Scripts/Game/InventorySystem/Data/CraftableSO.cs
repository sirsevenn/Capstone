using System.Collections.Generic;
using UnityEngine;

public abstract class CraftableSO : ScriptableObject
{
    [Header("Craftable Properties")]
    [SerializeField] private Sprite craftableIcon;
    [SerializeField] private List<CraftingRecipe> recipesList;
    [SerializeField] private EMixingType mixingType;


    public Sprite CraftableIcon
    {
        get { return craftableIcon; }
        private set { craftableIcon = value; }
    }

    public List<CraftingRecipe> RecipesList
    {
        get { return recipesList; }
        private set { }
    }

    public EMixingType MixingType
    {
        get { return mixingType; }
        private set { }
    }

    public abstract string GetCraftableName();
}
