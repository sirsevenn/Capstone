using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProbability", menuName = "ScriptableObjects/EnemyProbability")]
public class EnemyProbability : ScriptableObject
{
        public EnemyType type;
        public float heavyAttackProbability;
        public float lightAttackProbability;
        public float parryAttackProbability;
}
