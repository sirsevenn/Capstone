using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    public void UseObject(Vector3 pos)
    {
        transform.position = pos;

        //tentative
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

   public void ResetPoolableObject()
    {
        Enemy enemy = GetComponent<Enemy>();
        if (enemy)
        {
            enemy.ResetEnemy();
        }

        AnimationHandler animHandler = GetComponent<AnimationHandler>();
        if (animHandler)
        {
            animHandler.ResetAnimation();
        }

        Health health = GetComponent<Health>();
        if (health)
        {
            //Debug.Log("Reset HP");
            health.ResetHp();
        }

        Collider col = GetComponent<Collider>();
        if (col)
        {
            col.enabled = true;
        }

    }

}
