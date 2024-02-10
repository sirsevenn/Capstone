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
            case ActionType.Heavy:
                animator.SetTrigger("Heavy");
                break;
            case ActionType.Parry:
                animator.SetTrigger("Parry");
                break;
            case ActionType.Light:
                animator.SetTrigger("Light");
                break;
        }
    }

    public void ToggleMove()
    {
        animator.SetBool("Move", !animator.GetBool("Move"));
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Death");
    }

    public void ResetAnimation()
    {
        animator.Rebind();
        animator.Update(0f);
    }
}
