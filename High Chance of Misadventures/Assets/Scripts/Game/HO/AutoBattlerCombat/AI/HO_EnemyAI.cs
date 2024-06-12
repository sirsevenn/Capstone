using UnityEngine;

[RequireComponent(typeof(WorldSpaceHealthBar))]
public class HO_EnemyAI : HO_EntityAI
{
    [SerializeField] private WorldSpaceHealthBar healthBar;

    [Header("Enemy Properties")] 
    [SerializeField] private HO_EnemyDataSO enemyData;


    protected override void Awake()
    {
        base.Awake();
        healthBar = GetComponent<WorldSpaceHealthBar>();
    }

    public void SetEnemyData(HO_EnemyDataSO data)
    {
        enemyData = data;

        characterStats = new();
        characterStats.SetBaseHP(enemyData.EnemyHP);
        characterStats.SetBaseATK(enemyData.EnemyATK);
        characterStats.SetBaseDEF(enemyData.EnemyDEF);
        characterStats.SetDEFReduc(enemyData.EnemyDEFReduc);
        characterStats.InitializeCharacterStat();
    }

    public override void OnEntityTurn()
    {
        currentAttackDMG = characterStats.GetTotalATK();
        isAttackElemental = false;
    }

    public override void TriggerAttackAnimation()
    {
        animator.SetTrigger(enemyData.AttackAnimTrigger);
    }

    public override void TriggerHurtAnimation()
    {
        
    }

    public override void TriggerDeathAnimation()
    {
        animator.SetTrigger(enemyData.DeathAnimTrigger);
    }

    public  override void EntityTakeDamage(int damage, bool isElemental)
    {
        base.EntityTakeDamage(damage, isElemental);  
        healthBar.UpdateHP(characterStats.GetCurrentHPInPercent());
    }
}