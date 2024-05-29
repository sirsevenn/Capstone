using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "ScriptableObjects/Potion")]
public class PotionSO : CraftableSO
{
    [Space(10)] [Header("Potion Properties")]
    [SerializeField] private PotionType potionType;
    [TextArea(4, 10)]
    [SerializeField] private string potionDescription;


    public PotionType PotionType
    {  
        get { return potionType; } 
        private set {  potionType = value; } 
    }

    public string PotionDescription
    { 
        get { return potionDescription; } 
        private set { potionDescription = value; }
    }

    public override string GetCraftableName()
    {
        return potionType.ToString().Replace('_', ' ');
    }
}
