using UnityEngine;

public abstract class CraftableSO : ItemSO
{
    [Space(10)] [Header("Craftable Properties")]
    [SerializeField] protected uint tierLevel;
    [SerializeField] protected int baseValue;


    public uint TierLevel
    {
        get { return tierLevel; }
        private set { }
    }

    public int BaseValue
    {
        get { return baseValue; }
        private set { }
    }
}