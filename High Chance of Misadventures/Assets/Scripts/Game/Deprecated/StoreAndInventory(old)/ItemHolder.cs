using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private TMP_Text counter;

    public void InitializeItemHolder(int count)
    {
        counter.text = count.ToString();
    }
}
