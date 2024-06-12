using System;
using UnityEngine;

[Serializable]
public class HO_CharacterStat
{
    [Header("HP Stat")]
    [SerializeField] private int baseHP;
    [SerializeField] private int changeInHP;
    [SerializeField] private int currentHP;

    [Space(10)] [Header("ATK Stat")]
    [SerializeField] private int baseATK;
    [SerializeField] private int changeInATK;
    [SerializeField] private int totalATK;

    [Space(10)] [Header("DEF Stat")]
    [SerializeField] private int baseDEF;
    [SerializeField] private int changeInDEF;
    [SerializeField] private int totalDEF;
    [SerializeField] private int defReduc;


    public void InitializeCharacterStat()
    {
        ResetHP();
        totalATK = baseATK + changeInATK;
        totalDEF = baseDEF + changeInDEF;
    }

    #region HP
    public int GetMaxHP()
    {
        return baseHP + changeInHP;
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }

    public float GetCurrentHPInPercent()
    {
        return (float)currentHP / (float)GetMaxHP();
    }

    public void ResetHP()
    {
        currentHP = GetMaxHP();
    }

    public void SetBaseHP(int hp)
    {
        baseHP = hp;
    }

    public void ModifyHP(int increase)
    {
        changeInHP += increase;
        float percent = GetCurrentHPInPercent();
        currentHP = Mathf.RoundToInt(GetMaxHP() * percent);
    }

    public void Heal(int heal)
    {
        if (heal <= 0) return;

        currentHP += heal;
        currentHP = (currentHP > GetMaxHP()) ? GetMaxHP() : currentHP; 
        
    }

    public void TakeDamage(int damage, bool isElemental)
    {
        if (damage <= 0) return;

        float DEF = (totalDEF == 0) ? 1 : totalDEF;
        int reducedDMG = Mathf.RoundToInt(defReduc / DEF * damage);

        currentHP -= isElemental ? damage : reducedDMG;
        currentHP = (currentHP <= 0) ? 0 : currentHP;
    }
    #endregion


    #region ATK
    public int GetBaseATK()
    {
        return baseATK;
    }

    public int GetTotalATK()
    {
        return totalATK;
    }

    public void SetBaseATK(int atk)
    {
        baseATK = atk;
    }

    public void ModifyATK(int increase)
    {
        changeInATK += increase;
        totalATK = baseATK + changeInATK;
    }

    public void ResetATKToBase()
    {
        changeInATK = 0;
        totalATK = baseATK;
    }
    #endregion


    #region DEF
    public int GetBaseDEF()
    {
        return baseDEF;
    }

    public int GetTotalDEF()
    {
        return totalDEF;
    }

    public void SetBaseDEF(int def)
    {
        baseDEF = def;
    }

    public void ModifyDEF(int increase)
    {
        changeInDEF += increase;
        totalDEF = baseDEF + changeInDEF;
    }

    public void SetDEFReduc(int reduc)
    {
        if (reduc < 0) return;

        defReduc = reduc;
    }

    public void ResetDEFToBase()
    {
        changeInDEF = 0;
        totalDEF = baseDEF;
    }
    #endregion
}
