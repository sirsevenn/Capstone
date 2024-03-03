using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour {

	[SerializeField] Slider volumeSlider;
	[SerializeField] SettingsHandler.AudioType type;

	private void Awake() {

		if(!volumeSlider){
			volumeSlider = gameObject.GetComponent<Slider>();
		}

		switch(type){
			case SettingsHandler.AudioType.music:
				volumeSlider.value = SettingsHandler.Instance.musicVol;
				break;
			case SettingsHandler.AudioType.sfx:
				volumeSlider.value = SettingsHandler.Instance.sfxVol;
				break;
		}
	}

	public void UpdateVolume(){

		Debug.Log("Updating volume");

		switch(type){
			case SettingsHandler.AudioType.music:
				SettingsHandler.Instance.UpdateMusicVol(volumeSlider.value);
				break;
			case SettingsHandler.AudioType.sfx:
				SettingsHandler.Instance.UpdateSFXVol(volumeSlider.value);
				break;
			default:
				Destroy(this);
				break;
		}
	}

	public void ToggleSliderStatus(){
		switch(type){
			case SettingsHandler.AudioType.music:
				volumeSlider.interactable = SettingsHandler.Instance.playSFX;
				break;
			case SettingsHandler.AudioType.sfx:
				volumeSlider.interactable = SettingsHandler.Instance.playSFX;
				break;
		}
	}

	public void DiscardChanges(){
		SettingsHandler.Instance.DiscardChanges();
		Debug.Log(this.name + " discarded their changes");
		UpdateVolume();
	}
}
