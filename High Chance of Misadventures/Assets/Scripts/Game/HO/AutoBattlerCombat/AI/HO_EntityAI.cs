using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class HO_EntityAI : MonoBehaviour
{
    [SerializeField] protected HO_CharacterStat characterStats;

    [Space(10)] [Header("Attack Properties")]
    [SerializeField] protected int currentAttackDMG;
    [SerializeField] protected bool isAttackElemental;

    [Space(10)] [Header("Component References")]
    [SerializeField] protected Animator animator;


    protected virtual void Awake()
    {
        currentAttackDMG = 0;
        isAttackElemental = false;

        animator = GetComponent<Animator>();
    }

    public abstract void OnEntityTurn();

    public abstract void TriggerAttackAnimation();

    public abstract void TriggerHurtAnimation();

    public abstract void TriggerDeathAnimation();

    public int GetCurrentAttackDamage()
    {
        return currentAttackDMG;
    }

    public bool IsAttackElemental()
    {
        return isAttackElemental;
    }

    public virtual void EntityTakeDamage(int damage, bool isElemental)
    {
        characterStats.TakeDamage(damage, isElemental);
    }

    public bool IsEntityKilled()
    {
        return characterStats.GetCurrentHP() <= 0;
    }
}