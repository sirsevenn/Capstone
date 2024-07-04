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
    public UIHover hover;

    [SerializeField] private bool firstSelected = false;

    private void Start()
    {
        hp.InitializaHealth(data.maxHealth);
    }

    public void SelectEnemy()
    {
        highlight.SetActive(true);
        if (!firstSelected)
        {

            hover.StopHover();
            firstSelected = true;
        }
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
        transform.DOMoveY(-3, 5);
    }

    public virtual EnemyData GetEnemyData()
    {
        return data;
    }

    public void ResetEnemy()
    {
        transform.position = new Vector3(0, -10, 0);
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);

        hover.ResetHover();
        firstSelected = false;
    }

}
