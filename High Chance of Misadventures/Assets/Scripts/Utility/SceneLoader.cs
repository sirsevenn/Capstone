using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public static List<string> sceneNames = new List<string>();

    static SceneLoader()
    {
        sceneNames.Add("Area_1_Level_1");
        sceneNames.Add("StoreScene");
    }

    public static void ChangeScene(int index)
    {
        SceneManager.LoadScene(sceneNames[index]);
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
