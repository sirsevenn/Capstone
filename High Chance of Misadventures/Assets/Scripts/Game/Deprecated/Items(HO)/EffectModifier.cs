using System;

[Serializable]
public class EffectModifier
{
    public ECraftingEffect ModifierType;
    public int EffectValue;

    
    public EffectModifier(ECraftingEffect type, int value)
    {
        this.ModifierType = type;
        this.EffectValue = value;
    }
}