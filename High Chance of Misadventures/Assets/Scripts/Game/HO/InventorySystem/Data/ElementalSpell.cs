using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ElementalSpell : MonoBehaviour
{
    [SerializeField] private ParticleSystem orbParticle;
    [SerializeField] private ParticleSystem trailParticle;
    [SerializeField] private float delayBeforeDestroy;


    private void Awake()
    {
        orbParticle.Stop();
        trailParticle.Stop();
    }

    public IEnumerator FireProjectile(Vector3 endPos, float duration)
    {
        orbParticle.Play();
        trailParticle.Play();

        transform.DOMove(endPos, duration).SetEase(Ease.Linear);
        yield return new WaitForSeconds(duration);

        orbParticle.Stop();
        trailParticle.Stop();
        yield return new WaitForSeconds(delayBeforeDestroy);

        DestroyImmediate(gameObject);
    }
}