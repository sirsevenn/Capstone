using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "ScriptableObjects/HO/Spell")]
public class ElementalSpellSO : ConsumableSO
{
    [Space(10)] [Header("Elemental Spell Properties")]
    [SerializeField] private EElementalAttackType elementalType;
    [SerializeField] private GameObject projectilePrefab;


    public EElementalAttackType ElementalType
    {
        get { return elementalType; }
        private set { elementalType = value; }
    }

    public GameObject ProjectilePrefab
    {
        get { return projectilePrefab; }
        private set { projectilePrefab = value; }
    }
}