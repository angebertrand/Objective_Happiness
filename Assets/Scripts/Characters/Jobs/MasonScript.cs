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
        animator = GetComponent<Animator>();
        buildingManager = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<BuildingManager>();
        currentJob = "Mason";
        cameraMain = Camera.main;
        Register();

    }

    

    void Update()
    {

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
