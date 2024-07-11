using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public Animator GetAnimator()
    {
        return animator;
    }

    public void PlayAnimation(ActionType action)
    {
        switch (action)
        {
            case ActionType.Heavy:
                animator.SetTrigger("Attack");
                break;
            case ActionType.Parry:
                animator.SetTrigger("Parry");
                break;
            case ActionType.Light:
                animator.SetTrigger("Light");
                break;
        }
    }

    public void PlayerMove()
    {
        animator.SetBool("Move", true);
    }

    public void PlayerStopMove()
    {
        animator.SetBool("Move", false);
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Death");
    }

    public void PlayVictoryAnimation()
    {
        animator.SetTrigger("Victory");
    }

    public void ResetAnimation()
    {
        animator.Rebind();
        animator.Update(0f);
    }
}
