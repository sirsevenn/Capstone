using System.Collections.Generic;
using UnityEngine;

public class CraftingParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private List<ParticleSystem> materialParticlesList;

    [Space(10)]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip boilingSound;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = boilingSound;

        ResetAllParticles();
    }

    public void ResetAllParticles()
    {
        fireParticles.Stop();

        foreach (var particle in materialParticlesList)
        {
            particle.Stop();
        }

        audioSource.Stop();
    }

    public void PlayFireParticle()
    { 
        fireParticles.Play();
        audioSource.Play();
    }

    public void PlayMaterialParticle(int index, Color particleColor)
    {
        if (index < 0 || index >= materialParticlesList.Count) return;

        var mainModule = materialParticlesList[index].main;
        mainModule.startColor = particleColor;
        materialParticlesList[index].Play();
    }
}