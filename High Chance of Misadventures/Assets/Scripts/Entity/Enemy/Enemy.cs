using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyType enemyType;
    public EnemyData data;
    public GameObject highlight;
    public Health hp;
    public Collider col;

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
        hp.CloseHpBar();
        col.enabled = false;
        GetComponent<AnimationHandler>().PlayDeathAnimation();
    }

    public virtual EnemyData GetEnemyData()
    {
        return data;
    }

}
