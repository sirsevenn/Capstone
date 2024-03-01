using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int HP;
    [SerializeField] private int maxHP;
    [SerializeField] private float hpLerpDuration = 1.0f;

    [Header("Components")]
    [SerializeField] private Slider hpBar;

    private void Start()
    {
        hpBar.maxValue = maxHP;
        hpBar.value = HP;
    }

    public void SetHealth(int hp)
    {
        HP = Mathf.Min(hp, maxHP);
    }

    public void LerpHp(int value)
    {
        hpBar.value = value;
    }

    public void ApplyDamage(int value)
    {
        int newHp = Mathf.Max(0, HP - value);
        int oldHp = HP;
        DOVirtual.Int(oldHp, newHp, hpLerpDuration, LerpHp);
        HP = newHp;
    }

    public int GetHP()
    {
        return HP;
    }

    public void OpenHpBar()
    {
        hpBar.gameObject.SetActive(true);
    }

    public void CloseHpBar()
    {
        hpBar.gameObject.SetActive(false);
    }

    public void ResetHp()
    {
        hpBar.maxValue = maxHP;
        hpBar.value = HP;
        OpenHpBar();
    }
}
