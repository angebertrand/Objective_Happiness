using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WoodsmanScript : CharacterScript
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentJob = "Woods";
        Register();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
