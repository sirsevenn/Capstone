using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    //public GameObject probabilityBoard;
    //public ProbabilityManager probabilityManager;
    //public EnemyType enemyType;
    public EnemyProbability enemyProbability;
    public GameObject highlight;

    public void SelectEnemy()
    {
        highlight.SetActive(true);
    }

    public void DeselectEnemy()
    {
        highlight.SetActive(false);
    }

}
