using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Env Music")]
    // Add your music clips here
    public AudioClip menuMusic;
    public AudioClip forestMusic;
    public AudioClip caveMusic;
    public AudioClip castleMusic;

    [Header("Game Music")]
    public AudioClip gameWinMusic;
    public AudioClip gameLoseMusic;

    private AudioSource audioSource;
    public float fadeDuration = 1.0f; // Duration for fading effect

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 0:
                PlayMusic(menuMusic);
                break;
            case 1:
                
                break;
            case 2:
                StartCoroutine(FadeMusic(forestMusic));
                break;
            case 3:
                StartCoroutine(FadeMusic(forestMusic));
                break;
            case 4:
                StartCoroutine(FadeMusic(caveMusic));
                break;
            case 5:
                StartCoroutine(FadeMusic(caveMusic));
                break;
            case 6:
                StartCoroutine(FadeMusic(castleMusic));
                break;
            case 7:
                StartCoroutine(FadeMusic(castleMusic));
                break;
            default:
                PlayMusic(null); // No music for undefined scenes
                break;
        }
    }
    
    public IEnumerator FadeMusic(AudioClip newClip)
    {
        if (audioSource.clip == newClip)
        {
            yield break; // If the same clip is already playing, do nothing
        }

        if (audioSource.isPlaying)
        {
            // Fade out the current music
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = 1 - (t / fadeDuration);
                yield return null;
            }
            audioSource.Stop();
            audioSource.volume = 0;
        }

        audioSource.clip = newClip;
        if (newClip != null)
        {
            audioSource.Play();
            // Fade in the new music
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = t / fadeDuration;
                yield return null;
            }
            audioSource.volume = 1;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip)
        {
            return; // If the same clip is already playing, do nothing
        }

        audioSource.Stop();
        audioSource.clip = clip;
        if (clip != null)
        {
            audioSource.Play();
        }
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