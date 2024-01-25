using UnityEngine;

public class JournalManager : MonoBehaviour {
	
	[SerializeField] private GameObject JournalPanel;
	[SerializeField] private GameObject[] PanelList;

	int panelIndex = 0;

	void Start(){
		foreach(GameObject x in PanelList){
			x.SetActive(false);
		}
		PanelList[panelIndex].SetActive(true);
	}

	public void ToggleJournalPanel(){
		JournalPanel.SetActive(!JournalPanel.activeSelf);
	}

	public void SwitchPanel(int index){
		if(index >= 0 && index < PanelList.Length){
			foreach(GameObject x in PanelList){
				x.SetActive(false);
			}
			PanelList[index].SetActive(true);
		}
	}
	public void SwitchPanel(){
		{
			PanelList[panelIndex].SetActive(false);

			panelIndex++;
			// Debug.Log($"{panelIndex} / {PanelList.Length}");
			if(panelIndex > PanelList.Length - 1) panelIndex = 0;

			PanelList[panelIndex].SetActive(true);			
		}
	}
}