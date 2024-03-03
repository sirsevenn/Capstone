using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
	
	public void LoadScene(string scene){
			SceneManager.LoadScene(scene);
	}
	public void LoadScene(int scene){
			SceneManager.LoadScene(scene);
	}

}