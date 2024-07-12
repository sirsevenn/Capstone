using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WorldSpaceHealthBar))]
public class HO_EnemyAI : HO_EntityAI
{
    [Header("Enemy Properties")] 
    [SerializeField] private HO_EnemyDataSO enemyData;


    public void SetEnemyData(HO_EnemyDataSO data)
    {
        enemyData = data;
        attackSoundsList = new();
        attackSoundsList.Add(enemyData.AttackSound);
    }

    public override void OnEntityTurn(HO_ElementalEffects elementalEffects)
    {
        currentAttackDMG = characterStats.GetTotalATK();
        attackElementalType = EElementalAttackType.Unknown;
    }

    public override void TriggerAttackAnimation(Vector3 opponentPos, float meleeDistanceOffset, float animDuration)
    {
        Vector3 offsetDir = transform.position - opponentPos;
        offsetDir.Normalize();

        animator.SetTrigger(enemyData.AttackAnimTrigger);
        animator.SetFloat("Speed", enemyData.AttackAnimDuration / animDuration);
        transform.DOJump(opponentPos + offsetDir * meleeDistanceOffset, 1, 1, animDuration).SetEase(Ease.Linear).OnComplete(() => {
            SoundEffectManager.Instance.PlaySoundEffect(attackSoundsList.First());
        });
    }

    public override void TriggerEndAttackAnimation(Vector3 originalPos, float animDuration)
    {
        transform.DOJump(originalPos, 1, 1, animDuration).SetEase(Ease.Linear);
    }

    public override void TriggerHurtAnimation()
    {
        
    }

    public override void TriggerDeathAnimation()
    {
        animator.SetTrigger(enemyData.DeathAnimTrigger);
    }
}