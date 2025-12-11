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
        animator = GetComponent<Animator>(); 
        buildingManager = FindObjectOfType<BuildingManager>();
        currentJob = "Farmer";
        cameraMain = Camera.main;
        Register();

    }

    // Update is called once per frame
    void Update()
    {


    }
}
