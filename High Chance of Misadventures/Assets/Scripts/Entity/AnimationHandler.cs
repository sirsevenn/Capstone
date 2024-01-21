using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public void PlayAnimation(ActionType action)
    {
        switch (action)
        {
            case ActionType.Attack:
                animator.SetTrigger("Attack");
                break;
            case ActionType.Defend:
                animator.SetTrigger("Defend");
                break;
            case ActionType.Skill:
                animator.SetTrigger("Skill");
                break;
        }
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Death");
    }
}
