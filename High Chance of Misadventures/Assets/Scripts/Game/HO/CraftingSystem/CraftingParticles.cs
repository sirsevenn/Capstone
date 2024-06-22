using System.Collections.Generic;
using UnityEngine;

public class CraftingParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private List<ParticleSystem> materialParticlesList;


    private void Start()
    {
        ResetAllParticles();
    }

    public void ResetAllParticles()
    {
        fireParticles.Stop();

        foreach (var particle in materialParticlesList)
        {
            particle.Stop();
        }
    }

    public void PlayFireParticle()
    { 
        fireParticles.Play();
    }

    public void PlayMaterialParticle(int index, Color particleColor)
    {
        if (index < 0 || index >= materialParticlesList.Count) return;

        var mainModule = materialParticlesList[index].main;
        mainModule.startColor = particleColor;
        materialParticlesList[index].Play();
    }
}