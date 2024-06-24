//using System;
//using System.Collections.Generic;
//using UnityEngine;

//[Serializable]
//public class Potion : Consumable
//{
//    [Space(10)]
//    [SerializeField] private PotionSO potionData;


//    public Potion(uint id, int finalValue, PotionSO data) : base(id)
//    {
//        this.potionData = data;
//        this.finalValue = Mathf.Clamp(finalValue, data.GetEffectValueFromModifier(ECraftingEffect.Bad_Effect), data.GetEffectValueFromModifier(ECraftingEffect.Strong_Effect));
//        this.appliedModifierType = data.GetModifierFromEffectValue(this.finalValue);
//    }

//    public PotionSO PotionData
//    {
//        get { return potionData; }
//        private set { }
//    }
//}