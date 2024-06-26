using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonControl : MonoBehaviour
{

    enum FireState
    {
        Off,
        Low,
        Medium,
        High
    }

    [SerializeField] private FireState currentFireState;
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.name == "High")
                {
                    Debug.Log("Hit Button");
                    GameObject button = hit.collider.gameObject;

                    Vector3 newRotation = transform.rotation.eulerAngles;
                    newRotation.z -= 45;
                    button.transform.DORotate(newRotation, 0.5f);

                    switch (currentFireState)
                    {
                        case FireState.Off:
                            currentFireState = FireState.Low;
                            break;
                        case FireState.Low:
                            currentFireState = FireState.Medium;
                            break;
                        case FireState.Medium:
                            currentFireState = FireState.High;
                            break;
                        case FireState.High:
                            currentFireState = FireState.Off;
                            break;
                        default:
                            break;
                    }
                }
            }

        }
    }
}
