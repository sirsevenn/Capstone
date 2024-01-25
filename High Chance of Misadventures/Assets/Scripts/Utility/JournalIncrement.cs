using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor.Rendering;
public class JournalIncrement : MonoBehaviour
{

    [SerializeField] private GameObject Enemy;


    public ActionType enemyActType;

    [SerializeField] private TMP_Text numberText;
    [SerializeField] private Button addButton;
    [SerializeField] private Button minusButton;

    [SerializeField] private int num;

    EnemyData data;

    private void Awake() {
        // data = CombatManager.Instance.GetCurrentEnemy().GetComponent<EnemyData>();
        // int temp = JournalLog.Instance.GetData(data, enemyActType);
        // num = (temp > 0) ? temp : 0;
    }

    public void OnAdd(int inc){
        if(inc > 1) num += inc;
        else num++;
        UpdateNumText(num);
    }
    public void OnSubtract(int inc){
        if(inc > 1) num -= inc;
        else num--;
        UpdateNumText(num);
    }

    public void UpdateNumText(int num){
        string mod = "";
        string text;

        // JournalLog.Instance.UpdateEnemyData(data, this.enemyActType, this.num);

        if(num > 1000){
            num /= 1000;
            mod = "k";
            text = num.ToString() + mod;
        }
        else text = num.ToString();
        numberText.SetText(text);


    }
}
