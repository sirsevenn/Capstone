using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine.Events;

public class JournalIncrement : MonoBehaviour
{

    [SerializeField] private ExplorationManager em;
    public RoomType type;
    // public ActionType enemyActType;
    [Header("Essentials")]
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private Button addButton;
    [SerializeField] private Button minusButton;


    private void Awake() {
        // data = CombatManager.Instance.GetCurrentEnemy().GetComponent<EnemyData>();
        // int temp = JournalLog.Instance.GetData(data, enemyActType);
        // num = (temp > 0) ? temp : 0;

        if(!em){
            em = GameObject.FindObjectOfType<ExplorationManager>();
        }
    }

    public void OnAdd(int inc = 1){
        UpdateNumText(inc);
    }
    public void OnSubtract(int inc = 1){
        UpdateNumText(-inc);
    }

    public void UpdateNumText(int num){

        // ? Just in case of more than 1k
        string mod = "";
        string text;

        // JournalLog.Instance.UpdateEnemyData(data, this.enemyActType, this.num);
        RoomDataHolder rd = RoomDataHolder.Instance;
        rd.UpdateRoomData(this.type, num);

        int total = rd.GetRoomCount(this.type);

        if(total > 1000){
            total /= 1000;
            mod = "k";
            text = total.ToString() + mod;
        }
        else text = total.ToString();
        numberText.SetText(text);

        em?.UpdateTotal();
    }
}

// ? Commented out lines were originally intended for Enemy data
// ? Remove when unneeded or reuse wherever or as basis