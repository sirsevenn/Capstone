using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    public GameObject probabilityBoard;
    public ProbabilityManager probabilityManager;
    

    // Start is called before the first frame update
    void Start()
    {
        probabilityManager = GetComponent<ProbabilityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
