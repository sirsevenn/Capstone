using DG.Tweening;
using TMPro;
using UnityEngine;

public class WorldSpaceHealthNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private Vector3 defaultLocalPos;

    [Space(10)]
    [SerializeField] private float travelDistance;
    [SerializeField] private float travelDuration;


    private void Start()
    {
        ResetCanvas();
    }

    public void OnChangeHP(int changeInHP)
    {
        // cleanup just in case
        canvasTransform.DOKill();
        ResetCanvas();

        // update values
        numberText.enabled = true;
        numberText.text = (changeInHP > 0) ? "+" + changeInHP.ToString() : changeInHP.ToString();
        numberText.color = (changeInHP > 0) ? Color.green : Color.red;

        // animate
        canvasTransform.DOLocalMoveY(defaultLocalPos.y + travelDistance, travelDuration).SetEase(Ease.Linear).onComplete += ResetCanvas;
    }

    private void ResetCanvas()
    {
        numberText.enabled = false;
        numberText.text = "";
        numberText.color = Color.white;
        canvasTransform.localPosition = defaultLocalPos;
    }
}
