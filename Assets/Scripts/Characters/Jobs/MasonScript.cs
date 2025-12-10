using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MasonScript : CharacterScript
{

    public bool isBuildingSomething = false;
    
    private bool wanderingDestinationSet = false;

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
        // Si le Mason est en train de travailler (building), ne wandere pas
        if (isBuildingSomething)
            return; // on ne touche pas à MoveTo

        if (isWandering && !isLearning && !isWorking)
        {
            if (!IsWalking() && !wanderingDestinationSet)
            {
                Vector3 rand = RandomNavmeshLocation(30f);
                MoveTo(rand);
                wanderingDestinationSet = true;
            }
        }

        if (wanderingDestinationSet && !IsWalking())
        {
            wanderingDestinationSet = false;
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
