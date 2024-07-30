using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    // Singleton instance
    public static SoundEffectManager Instance { get; private set; }

    // AudioSource to play the sound effects
    private AudioSource audioSource;

    [Header("Generic Sound Effects")]
    [SerializeField] private AudioClip clickSoundEffect;

    private void Awake()
    {
        // Check if there's already an instance of this class
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy this instance if it's a duplicate
            return;
        }

        // Assign the instance and make it persist
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize the audio source
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Method to play a sound effect
    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("No audio clip provided to play.");
            return;
        }

        audioSource.PlayOneShot(clip);
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSoundEffect);
    }

    public float GetVolume()
    {
        return audioSource.volume;
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }
}