using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Guild : MonoBehaviour
{  
    [SerializeField] private TMP_Text[] countText;
    [SerializeField] private TMP_Text totalText;

    private void Start() {

        RoomDataHolder rd = RoomDataHolder.Instance;
        int total = rd.GetTotalRooms();

        for(int i = 0; i < countText.Length; i++){
            int num = rd.GetRoomCount((RoomType)i);

            // ? Apparently cast redundant but only works this way
            float percent = (float)num / (float)total * 100.0f;
            Debug.Log($"{percent} = {num} / {total} * 100");
            countText[i].SetText($"{num} - {percent:F0}%");
        }

        totalText.SetText(rd.GetTotalRooms().ToString());
    }
}
