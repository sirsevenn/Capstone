using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HO_LevelPanel : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image levelIcon;
    [SerializeField] private TMP_Text levelName;


    public void InitiailizeLevelPanel(HO_LevelSO levelData, Action<int> onLevelClick)
    {
        levelIcon.sprite = levelData.LevelIcon;
        levelName.text = "Level " + levelData.LevelID.ToString() + "\n" + levelData.EnemyData.GetEnemyName();

        button.onClick.AddListener(() => {
            onLevelClick((int)levelData.LevelID);
        });
    }
}
