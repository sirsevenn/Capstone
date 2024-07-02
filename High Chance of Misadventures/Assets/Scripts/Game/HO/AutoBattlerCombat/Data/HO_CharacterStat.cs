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

    [Space(10)] [Header("Other Properties")]
    public EElementalAttackType WeakToElement;
    public EElementalAttackType ResistantToElement;


    public void CopyStats(HO_CharacterStat newStats)
    {
        baseHP = newStats.baseHP;
        changeInHP = newStats.changeInHP;
        currentHP = newStats.currentHP;

        baseATK = newStats.baseATK;
        changeInATK = newStats.changeInATK;
        totalATK = newStats.totalATK;

        baseDEF = newStats.baseDEF;
        changeInDEF = newStats.changeInDEF;
        totalDEF = newStats.totalDEF;

        WeakToElement = newStats.WeakToElement;
        ResistantToElement = newStats.ResistantToElement;
    }

    public void InitializeCharacterStats()
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

    public int TakeDamage(int damage, EElementalAttackType attackElementalType)
    {
        if (damage <= 0) return - 1;

        int modifiedDMG = 0;

        if (attackElementalType == WeakToElement)
        {
            modifiedDMG = Mathf.FloorToInt(damage * 1.5f);
            //Debug.Log("weak");
        }
        else if (attackElementalType == ResistantToElement)
        {
            modifiedDMG = Mathf.FloorToInt(damage * 0.5f);
            //Debug.Log("resistant");
        }
        else if (attackElementalType != EElementalAttackType.Unknown)
        {
            modifiedDMG = damage;
            //Debug.Log("just element");
        }
        else
        {
            modifiedDMG = (damage - totalDEF) <= 0 ? 0 : damage - totalDEF;
            //Debug.Log("normal ");
        }


        //Debug.Log("received " + modifiedDMG + "DMG   " + (attackElementalType == EElementalAttackType.Unknown ? "normal" : attackElementalType.ToString()));

        currentHP -= modifiedDMG;
        currentHP = (currentHP <= 0) ? 0 : currentHP;

        return modifiedDMG;
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

    public void ResetDEFToBase()
    {
        changeInDEF = 0;
        totalDEF = baseDEF;
    }
    #endregion
}
