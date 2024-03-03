using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AnimationHandler))]
[RequireComponent(typeof(Poolable))]
[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : Entity
{
    //Required Types
  

    public EnemyType enemyType;
    public EnemyData data;
    public GameObject highlight;
    public Health hp;
    public Collider col;

    private void Start()
    {
        hp.InitializaHealth(data.maxHealth);
    }

    public void SelectEnemy()
    {
        highlight.SetActive(true);
    }

    public void DeselectEnemy()
    {
        highlight.SetActive(false);
    }

    public void Die()
    {
        DeselectEnemy();
       
        col.enabled = false;

        AnimationHandler handler = GetComponent<AnimationHandler>();
        Animator animator = handler.GetAnimator();

        handler.PlayDeathAnimation();
        StartCoroutine(GameUtilities.DelayFunction(Decay, 2));
    }

    public void Decay()
    {
        hp.CloseHpBar();
        transform.DOMoveY(-1, 5);
    }

    public virtual EnemyData GetEnemyData()
    {
        return data;
    }

}
