using System;

[Serializable]
public class Potion
{
    //public PotionType PotionType;
    public PotionSO PotionData;
    public uint Amount;


    public Potion(PotionSO data, uint amount)
    {
        this.PotionData = data;
        this.Amount = amount;
    }
}