using DG.Tweening;
using TMPro;
using UnityEngine;

public class WorldSpaceDamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private Vector3 defaultLocalPos;

    [Space(10)]
    [SerializeField] private float travelDistance;
    [SerializeField] private float travelDuration;


    private void Start()
    {
        ResetDamageNumber();
    }

    public void OnTakeDamage(int damage)
    {
        damageText.enabled = true;
        damageText.text = damage.ToString();

        canvasTransform.DOLocalMoveY(defaultLocalPos.y + travelDistance, travelDuration).SetEase(Ease.Linear).onComplete += ResetDamageNumber;
    }

    private void ResetDamageNumber()
    {
        damageText.enabled = false;
        canvasTransform.localPosition = defaultLocalPos;
    }
}
