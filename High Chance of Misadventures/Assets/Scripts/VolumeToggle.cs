using UnityEngine;
using UnityEngine.UI;
public class VolumeToggle : MonoBehaviour {
	
	[SerializeField] SettingsHandler.AudioType type;

	[SerializeField] Toggle toggle;

	private void Start() {

		if(!toggle){
			toggle = gameObject.GetComponent<Toggle>();
		}

		switch(type){
			case SettingsHandler.AudioType.music:
				toggle.isOn = SettingsHandler.Instance.playMusic;
				break;
			case SettingsHandler.AudioType.sfx:
				toggle.isOn = SettingsHandler.Instance.playSFX;
				break;
		}
	}

	public void UpdateAudioToggle(){
		switch(type){
			case SettingsHandler.AudioType.music:
				SettingsHandler.Instance.playMusic = toggle.isOn;
				Debug.Log("Music toggled: " + toggle.isOn);
				break;
			case SettingsHandler.AudioType.sfx:
				SettingsHandler.Instance.playSFX = toggle.isOn;
				Debug.Log("SFX toggled: " + toggle.isOn);
				break;
		}
	}	

}