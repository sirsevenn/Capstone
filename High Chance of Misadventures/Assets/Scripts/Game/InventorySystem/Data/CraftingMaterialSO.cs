using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Material", menuName = "ScriptableObjects/CraftingMaterial")]
public class CraftingMaterialSO : ScriptableObject
{
    [Header("Main Properties")]
    [SerializeField] private CraftingMaterialType materialType;
    [TextArea(4, 10)]
    [SerializeField] private string materialDescription;
    [SerializeField] private Sprite materialIcon;

    [Space(10)] [Header("Crafting Properties")]
    [SerializeField] private DiceType diceType;
    [Tooltip("Success Rate in percent")] 
    [SerializeField] private float successRate;


    public CraftingMaterialType MaterialType
    {
        get { return materialType; }
        private set { materialType = value; }
    }

    public string MaterialDescription
    { 
        get { return materialDescription; } 
        private set { materialDescription = value; }
    }

    public Sprite MaterialIcon
    {
        get { return materialIcon; }
        private set { materialIcon = value; }
    }

    public DiceType DiceType
    {
        get { return diceType; }
        private set { diceType = value; }
    }

    public float SuccessRate
    {
        get { return successRate / 100f; }
        private set { successRate = value; }
    }

    public string GetMaterialName()
    {
        return materialType.ToString().Replace('_', ' ');
    }
}