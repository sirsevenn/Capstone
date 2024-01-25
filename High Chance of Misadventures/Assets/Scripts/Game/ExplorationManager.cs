using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ExplorationManager : MonoBehaviour
{

    [Header("Explore Prompts")]
    [SerializeField] private TMP_Text RoomNameText;
    [SerializeField] private Button NextRoomButton;
    [SerializeField] private Button EndButton;
    [SerializeField] private int MaxRooms;
    private int ExploredCount = 0;

    [Header("Player UI buttons")]

    [SerializeField] private Button[] CombatButtons; 

    // [Header("Room Text Box")]

    // [SerializeField] private TMP_Text CombatRoomCountText;
    // [SerializeField] private TMP_Text EventRoomCountText;
    // [SerializeField] private TMP_Text PeddlerRoomCountText;
    // [SerializeField] private TMP_Text RestRoomCountText;
    [SerializeField] private TMP_Text TotalRoomCountText;

    private void Start() {
        CombatManager.Instance.StartCombatEvent.AddListener(PauseExploration);
        CombatManager.Instance.EndCombatEvent.AddListener(StartExploration);
        
    }

    private void StartExploration(){
        Debug.Log("Starting Exploration");
        NextRoomButton.gameObject.SetActive(true);
        ToggleCombatButtons(false);
        
    }
    private void PauseExploration(){
        Debug.Log("Exploration Halted!");
        NextRoomButton.gameObject.SetActive(false);
        ToggleCombatButtons(true);
    }

    void ToggleCombatButtons(bool toggle){
        foreach(Button x in CombatButtons){
            x.interactable = toggle;
        }
    }

    public void NextRoom(){

        RoomType next = (RoomType)Random.Range(0, System.Enum.GetValues(typeof(RoomType)).Length);
        
        // TODO Wipe screen as transition

        Debug.Log("Heading to next room");

        RoomDataHolder rd = RoomDataHolder.Instance;

        switch(next){
            case RoomType.Combat:
                // TODO Start same combat
                RoomNameText.SetText("Combat");
                // CombatRoomCountText.SetText(rd.GetRoomCount(next).ToString());
                break;
            case RoomType.Event:
                RoomNameText.SetText("Event");
                // EventRoomCountText.SetText(rd.GetRoomCount(next).ToString());
                break;
            case RoomType.Peddler:
                RoomNameText.SetText("Peddler");
                // PeddlerRoomCountText.SetText(rd.GetRoomCount(next).ToString());
                break;
            case RoomType.Rest:
                RoomNameText.SetText("Rest");
                // RestRoomCountText.SetText(rd.GetRoomCount(next).ToString());
                break;
        }

        ExploredCount++;
        if(ExploredCount >= MaxRooms) EndExplore();
        // rd.UpdateRoomData(next);
        // TotalRoomCountText.SetText(rd.GetTotalRooms().ToString());
    }

    private void EndExplore()
    {
        ToggleCombatButtons(false);
        NextRoomButton.gameObject.SetActive(false);
        EndButton.transform.parent.gameObject.SetActive(true);
        EndButton.gameObject.SetActive(true);
    }

    public void ReturnToGuild(){
        SceneManager.LoadScene("ProtoGuild");
    }

    public void UpdateTotal(){
        TotalRoomCountText.SetText(RoomDataHolder.Instance.GetTotalRooms().ToString());
        Debug.Log("Total Rooms got updated!");
    }

}
