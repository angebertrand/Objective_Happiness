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
        animator = GetComponentInChildren<Animator>();
        buildingManager = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<BuildingManager>();
        currentJob = "Wander";
        isWandering = true;
        cameraMain = Camera.main;
        Register();
        StartWandering();
        //NextBuilding = GameObject.FindGameObjectWithTag("School");
        //GoToBuilding(NextBuilding);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
