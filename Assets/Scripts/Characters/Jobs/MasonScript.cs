using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MasonScript : CharacterScript
{

    
    
    

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

    public void StartBuilding(GameObject Building)
    {
        if (Building == null) return;

        JobBuilding = Building;
        isBuildingSomething = true;
        isWorking = true;
        isWandering = false;

        // Déplacer vers le bâtiment
        MoveTo(Building);
    }
}
