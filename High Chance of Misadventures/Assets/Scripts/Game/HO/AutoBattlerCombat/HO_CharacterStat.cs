using System;
using UnityEngine;

[Serializable]
public class HO_CharacterStat
{
    [Header("HP Stat")]
    [SerializeField] private int baseHP;
    [SerializeField] private int increaseHP;
    [SerializeField] private int currentHP;

    [Space(10)] [Header("ATK Stat")]
    [SerializeField] private int baseATK;
    [SerializeField] private int increaseATK;
    [SerializeField] private int totalATK;

    [Space(10)] [Header("DEF Stat")]
    [SerializeField] private int baseDEF;
    [SerializeField] private int increaseDEF;
    [SerializeField] private int totalDEF;
    [SerializeField] private int defReduc;


    public void InitializeCharacterStat()
    {
        ResetHP();
        totalATK = baseATK + increaseATK;
        totalDEF = baseDEF + increaseDEF;
    }

    #region HP
    public int GetMaxHP()
    {
        return baseHP + increaseHP;
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

    public void SetIncreaseHP(int increase)
    {
        if (increase < 0) return;

        increaseHP = increase;
        float percent = GetCurrentHPInPercent();
        currentHP = Mathf.RoundToInt(GetMaxHP() * percent);
    }

    public void Heal(int heal)
    {
        if (heal < 0) return;

        currentHP += heal;
        currentHP = (currentHP > GetMaxHP()) ? GetMaxHP() : currentHP; 
        
    }

    public void TakeDamage(int damage, bool isElemental)
    {
        if (damage < 0) return;

        int reducedDMG = Mathf.RoundToInt(defReduc / totalDEF * damage);
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

    public void SetIncreaseATK(int increase)
    {
        if (increase < 0) return;

        increaseATK = increase;
        totalATK = baseATK + increaseATK;
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

    public void SetIncreaseDEF(int increase)
    {
        if (increase < 0) return; 

        increaseDEF = increase;
        totalDEF = baseDEF + increaseDEF;
    }

    public void SetDEFReduc(int reduc)
    {
        if (reduc < 0) return;

        defReduc = reduc;
    }
    #endregion
}
