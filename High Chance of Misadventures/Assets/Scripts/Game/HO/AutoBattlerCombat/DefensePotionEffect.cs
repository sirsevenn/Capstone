using System.Collections.Generic;
using UnityEngine;

public class DefensePotionEffect : MonoBehaviour
{
    [SerializeField] private List<GameObject> shieldsList;
    [Tooltip("Angle per second")] 
    [SerializeField] private float rotationSpeed;


    private void Start()
    {
        foreach (var shield in shieldsList)
        {
            shield.SetActive(true);
        }
    }

    private void Update()
    {
        foreach (var shield in shieldsList)
        {
            shield.transform.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
        }
    }
}
