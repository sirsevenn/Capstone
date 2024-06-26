using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private ParticleSystem fireVFX;
    [SerializeField] private ParticleSystem waterVFX;
    [SerializeField] private ParticleSystem earthVFX;

    public Animator GetAnimator()
    {
        return animator;
    }

    public void PlayAnimation(ActionType action)
    {
        fireVFX.Clear();
        fireVFX.Stop();
        waterVFX.Clear();
        waterVFX.Stop();
        earthVFX.Clear();
        earthVFX.Stop();

        switch (action)
        {
            case ActionType.Fire:
                fireVFX.Play();
                break;
            case ActionType.Water:
                waterVFX.Play();
                break;
            case ActionType.Earth:
                earthVFX.Play();
                break;
        }

        animator.SetTrigger("Attack");
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
        fireVFX.Stop();
        waterVFX.Stop();
        earthVFX.Stop();
    }
}
