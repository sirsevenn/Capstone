using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIHover : MonoBehaviour
{
    private Tween hoverTween;

    // Start is called before the first frame update
    private void OnEnable()
    {
        StartHover();
    }

    public void StartHover()
    {
        float originalPos = transform.position.y;
        Sequence hoverSequence = DOTween.Sequence();

        // Move up
        hoverSequence.Append(transform.DOLocalMoveY(originalPos + 0.5f, 1).SetEase(Ease.OutSine));

        // Move down
        hoverSequence.Append(transform.DOLocalMoveY(originalPos, 1).SetEase(Ease.InSine));

        // Loop the sequence infinitely
        hoverSequence.SetLoops(-1, LoopType.Yoyo);

        hoverTween = hoverSequence;
    }

    public void StopHover()
    {
        if (hoverTween != null && hoverTween.IsActive())
        {
            hoverTween.Kill();
        }

        gameObject.SetActive(false);
    }

    public void ResetHover()
    {
        gameObject.SetActive(true);
    }



}
