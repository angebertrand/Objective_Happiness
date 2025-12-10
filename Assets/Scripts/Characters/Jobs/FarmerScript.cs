using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmerScript : CharacterScript
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        buildingManager = FindObjectOfType<BuildingManager>();
        currentJob = "Farmer";
        cameraMain = Camera.main;
        Register();

    }

    // Update is called once per frame
    void Update()
    {
        // Si le perso travaille ou apprend, ne pas wander
        if (isWorking || isLearning || isBuildingSomething)
            return;

        // Wandering
        if (isWandering)
        {
            // Si pas de target, on en choisit une
            if (!hasWanderTarget)
            {
                currentWanderTarget = RandomNavmeshLocation(wanderRadius);
                agent.SetDestination(currentWanderTarget);
                hasWanderTarget = true;
            }

            // Si arrivé à destination, reset pour en choisir une nouvelle
            if (Vector3.Distance(transform.position, currentWanderTarget) <= arriveThreshold)
            {
                hasWanderTarget = false;
            }
        }

    }
}
