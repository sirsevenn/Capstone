using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    private int HP;
    private int maxHP;
    [SerializeField] private float hpLerpDuration = 1.0f;

    [Header("Components")]
    [SerializeField] private Slider hpBar;
    [SerializeField] private TMP_Text hpValue;

    public void InitializaHealth(int maxHealth)
    {
        HP = maxHealth;
        maxHP = maxHealth;

        hpBar.maxValue = maxHP;
        hpBar.value = HP;

        UpdateHealthText();
    }

    public void SetHealth(int hp)
    {
        HP = Mathf.Min(hp, maxHP);

        UpdateHealthText();
    }

    public void LerpHp(float value)
    {
        hpBar.value = value;
    }

    public void ApplyDamage(int value)
    {
        float newHp = Mathf.Max(0, HP - value);
        float oldHp = HP;
        DOVirtual.Float(oldHp, newHp, hpLerpDuration, LerpHp);
        HP = (int)newHp;

        if (hpValue == null)
        {
            return;
        }

        UpdateHealthText();
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
        OpenHpBar();
        hpBar.maxValue = maxHP;
        HP = maxHP;
        hpBar.value = HP;
    }

    private void UpdateHealthText()
    {
        if (hpValue == null)
        {
            return;
        }

        hpValue.text = HP.ToString() + "/" + maxHP.ToString();
    }
}
