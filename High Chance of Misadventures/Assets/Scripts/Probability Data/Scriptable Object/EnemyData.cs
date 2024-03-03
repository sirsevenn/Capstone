using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy")]
    public EnemyType type;

    [Header("Settings")]
    public int maxHealth;
    public int attackDamage;

    [Header("Probabilitites")]
    public int heavyAttackProbability;
    public int lightAttackProbability;
    public int parryAttackProbability;
}
