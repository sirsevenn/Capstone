using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProbability", menuName = "ScriptableObjects/EnemyProbability")]
public class EnemyProbability : ScriptableObject
{
        public EnemyType type;
        public int heavyAttackProbability;
        public int lightAttackProbability;
        public int parryAttackProbability;
}
