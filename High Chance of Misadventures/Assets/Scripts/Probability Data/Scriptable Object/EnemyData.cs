using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
        public EnemyType type;
        public int heavyAttackProbability;
        public int lightAttackProbability;
        public int parryAttackProbability;
}