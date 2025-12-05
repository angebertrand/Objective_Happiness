using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WandererScript : CharacterScript
{
    // Start is called before the first frame update
    void Start()
    {
        

    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentJob = "Wander";
        Register();
        //NextBuilding = GameObject.FindGameObjectWithTag("School");
        //GoToBuilding(NextBuilding);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
