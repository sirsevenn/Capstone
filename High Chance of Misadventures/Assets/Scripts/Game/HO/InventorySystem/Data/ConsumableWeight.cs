using System;
using UnityEngine;

[Serializable]
public class ConsumableWeight 
{
    [SerializeField] private ECraftingEffect craftingEffect;
    [SerializeField] private ConsumableSO consumableToCraft;


    public ECraftingEffect CraftingEffect 
    { 
        get { return craftingEffect; } 
        private set { craftingEffect = value; } 
    }

    public ConsumableSO ConsumableToCraft
    {
        get { return consumableToCraft; }
        private set { consumableToCraft = value; }
    }
}