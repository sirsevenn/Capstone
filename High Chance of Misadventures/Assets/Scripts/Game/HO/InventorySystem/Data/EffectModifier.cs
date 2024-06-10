using System;

[Serializable]
public class EffectModifier
{
    public EEffectModifier ModifierType;
    public int EffectValue;

    
    public EffectModifier(EEffectModifier type, int value)
    {
        this.ModifierType = type;
        this.EffectValue = value;
    }
}