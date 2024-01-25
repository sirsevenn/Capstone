using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JournalLog : MonoBehaviour
{
    
    #region Singleton
    public static JournalLog Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;

    }
    #endregion

    [SerializeField] private List<EnemyData> EnemyDataList = new();

    public void UpdateEnemyData(EnemyData data, ActionType action, int amt){

        if(EnemyDataList.Contains(data)){
            EnemyData enemy = EnemyDataList[EnemyDataList.IndexOf(data)];
            enemy.UpdateData(action, amt);
        }
        else{
            data.UpdateData(action, amt);
            EnemyDataList.Add(data);
        }
    }

    public int GetData(EnemyData data ,ActionType action){
        if(EnemyDataList.Contains(data)){
            return EnemyDataList[EnemyDataList.IndexOf(data)].GetData(action);
        }

        Debug.Log("No data was recovered");
        return 0;
    }

}