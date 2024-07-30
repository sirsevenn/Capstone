using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    public void ToggleMusic()
    {
        MusicManager musicManager = MusicManager.Instance;
        float volume = musicManager.GetVolume();
        switch (volume)
        {
            case 0f:
                musicManager.SetVolume(0.5f);
                break;
            case 0.5f:
                musicManager.SetVolume(1f);
                break;
            case 1f:
                musicManager.SetVolume(0f);
                break;
            default:
                break;
        }

        SoundEffectManager.Instance.PlayClick();
    }

    public void ToggleSFX()
    {
        SoundEffectManager sfxManager = SoundEffectManager.Instance;
        float volume = sfxManager.GetVolume();
        switch (volume)
        {
            case 0f:
                sfxManager.SetVolume(0.5f);
                break;
            case 0.5f:
                sfxManager.SetVolume(1f);
                break;
            case 1f:
                sfxManager.SetVolume(0f);
                break;
            default:
                break;
        }

        sfxManager.PlayClick();
    }
}
