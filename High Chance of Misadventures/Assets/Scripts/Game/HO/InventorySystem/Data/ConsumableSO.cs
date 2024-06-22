using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "ScriptableObjects/HO/Consumable")]
public class ConsumableSO : ItemSO
{
    [Space(10)] [Header("Consumable Properties")]
    [SerializeField] private EConsumableType consumableType;

    [TextArea(4, 10)]
    [SerializeField] private string consumableDescription;

    [Space(10)] [Tooltip("can be healing value, defense value, or attack value")]
    [SerializeField] private int numberValue;


    public EConsumableType ConsumableType
    {
        get { return consumableType; }
        private set { consumableType = value; }
    }

    public string ConsumableDescription
    {
        get { return consumableDescription; }
        private set { consumableDescription = value; }
    }

    public int NumberValue
    {
        get { return numberValue; }
        private set { numberValue = value; }
    }

    public override string GetItemName()
    {
        return consumableType.ToString().Replace("_", " ");
    }
}