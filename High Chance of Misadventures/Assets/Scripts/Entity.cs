using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    enum EntityType
    {
        Character,
        Enemy
    }

    //Stats
    public string characterName;

    //Combat Stats
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int health;

    //Getter
    public int GetAttack()
    {
        return attack;
    }

    public int GetDefense()
    {
        return defense;
    }

    public int GetHealth()
    {
        return health;
    }

    public int ApplyDamage(int damage)
    {
        return Mathf.Max(0, health - damage);
    }

}
