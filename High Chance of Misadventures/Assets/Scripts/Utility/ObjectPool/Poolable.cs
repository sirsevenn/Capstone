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
        transform.position = new Vector3(0, -10, 0);
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);

        AnimationHandler animHandler = GetComponent<AnimationHandler>();
        if (animHandler)
        {
            animHandler.ResetAnimation();
        }

        Health health = GetComponent<Health>();
        if (health)
        {
            health.ResetHp();
        }
    }

}
