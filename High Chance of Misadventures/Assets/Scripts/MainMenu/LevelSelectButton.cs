using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectButton : MonoBehaviour
{
    public Button button;
    public GameObject lockedImage;
    public GameObject unlockedText;

    public void LockLevel()
    {
        button.enabled = false;
        lockedImage.SetActive(true);
        unlockedText.SetActive(false);
    }

    public void UnlockLevel()
    {
        button.enabled = true;
        lockedImage.SetActive(false);
        unlockedText.SetActive(true);
    }
}
