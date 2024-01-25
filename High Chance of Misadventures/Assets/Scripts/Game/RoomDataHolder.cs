using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RoomDataHolder : MonoBehaviour {
	
    #region singleton
    public static RoomDataHolder Instance { get; private set; }


    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
        DontDestroyOnLoad(this);

    }
    #endregion

	private Dictionary<RoomType, int> RoomCounts = new(){
		{RoomType.Combat, 0},
		{RoomType.Event, 0},
		{RoomType.Peddler, 0},
		{RoomType.Rest, 0}
	};


	public void UpdateRoomData(RoomType type, int amt = 1){
		RoomCounts[type] += amt;
		Debug.Log($" {type} now has {RoomCounts[type]} room counts!");
	}

	public int GetRoomCount(RoomType type){
		return RoomCounts[type];
	}

	public int GetTotalRooms(){
		return RoomCounts.Values.Sum();
	}
}
public enum RoomType {Combat, Event, Peddler, Rest}