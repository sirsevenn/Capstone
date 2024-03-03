using UnityEngine;
using UnityEngine.UI;

public class PopUpToggle : MonoBehaviour {
	
	[SerializeField] GameObject TargetPopUp;

	public void TogglePopUp(){
		Debug.Log(this.name + " was clicked");
		if(TargetPopUp){
			TargetPopUp.SetActive(!TargetPopUp.activeSelf);
		}
	}

}