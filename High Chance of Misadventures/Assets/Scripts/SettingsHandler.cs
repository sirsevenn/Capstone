
using UnityEngine;

public class SettingsHandler : MonoBehaviour
{

    #region singleton
    public static SettingsHandler Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this);

    }
    #endregion

	public enum AudioType{ none, music, sfx};

    public float musicVol = 1f;
    public float sfxVol = 1f;

    private float newMusicVol = 0.0f;
    private float newSFXVol = 0.0f;

    public bool playMusic = true;
    public bool playSFX = true;

    bool musicVolChanged = false;
    bool sfxVolChanged = false;

    public void UpdateMusicVol(float vol){
        newMusicVol = vol;
        Debug.Log($"Music Volume updated to {vol}");
    }
    public void UpdateSFXVol(float vol){
        newSFXVol = vol;
        Debug.Log($"SFX Volume updated to {vol}");
    }

    public void ApplyChanges(){
        musicVol = newMusicVol;
        sfxVol = newSFXVol;
    }

    public void DiscardChanges(){
        RestoreChangedFlag();
    }

    public void RestoreChangedFlag(){
        musicVolChanged = false;
        sfxVolChanged = false;
    }


}
