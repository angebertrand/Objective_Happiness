using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MasonScript : CharacterScript
{

    public bool isBuildingSomething = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        buildingManager = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<BuildingManager>();
        currentJob = "Mason";
        cameraMain = Camera.main;
        Register();

    }

    // Update is called once per frame
    void Update()
    {
        if (isWandering && !isLearning && !isWorking && !isBuildingSomething)
        {

            if (!IsWalking())
            {
                Wandering();
            }

        }

    }

    public void StartBuilding(GameObject Building)
    {
        isBuildingSomething = true;
        JobBuilding = Building;
        isWorking = true;
        isWandering = false;
        agent.SetDestination(Building.transform.position);
    }
}
