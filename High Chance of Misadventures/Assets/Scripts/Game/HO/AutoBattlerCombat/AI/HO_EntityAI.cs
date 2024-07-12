using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class HO_EntityAI : MonoBehaviour
{
    [SerializeField] protected HO_CharacterStat characterStats;

    [Space(10)] [Header("Attack Properties")]
    [SerializeField] protected int currentAttackDMG;
    [SerializeField] protected EElementalAttackType attackElementalType;

    [Space(10)] [Header("Audio Properties")]
    [SerializeField] protected List<AudioClip> attackSoundsList;

    [Space(10)] [Header("Component References")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected WorldSpaceHealthBar healthBar;
    [SerializeField] protected WorldSpaceHealthNumber healthNumber;


    protected virtual void Awake()
    {
        currentAttackDMG = 0;
        attackElementalType = EElementalAttackType.Unknown;

        animator = GetComponent<Animator>();
        healthBar = GetComponent<WorldSpaceHealthBar>();
        healthNumber = GetComponent<WorldSpaceHealthNumber>();
    }

    public abstract void OnEntityTurn(HO_ElementalEffects elementalEffects);

    public abstract void TriggerAttackAnimation(Vector3 opponentPos, float meleeDistanceOffset, float animDuration);

    public abstract void TriggerEndAttackAnimation(Vector3 originalPos, float animDuration);

    public abstract void TriggerHurtAnimation();

    public abstract void TriggerDeathAnimation();

    public HO_ElementalEffects ElementalEffects => characterStats.ElementalEffects;

    public void CopyStats(HO_CharacterStat newStat)
    {
        characterStats = new();
        characterStats.CopyStats(newStat);
        healthBar.UpdateHP(characterStats.GetCurrentHPInPercent());
    }

    public int GetCurrentAttackDamage()
    {
        return currentAttackDMG;
    }

    public EElementalAttackType GetAttackElementalType()
    {
        return attackElementalType;
    }

    public virtual void EntityTakeDamage(int damage, EElementalAttackType attackElementalType = EElementalAttackType.Unknown, bool hasArmorPierce = false)
    {
        int dmg = characterStats.TakeDamage(damage, attackElementalType, hasArmorPierce);
        healthBar.UpdateHP(characterStats.GetCurrentHPInPercent());
        healthNumber.OnChangeHP(-dmg);
    }

    public bool IsEntityKilled()
    {
        return characterStats.GetCurrentHP() <= 0;
    }
}