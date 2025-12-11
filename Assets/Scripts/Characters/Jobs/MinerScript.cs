using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinerScript : CharacterScript
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        buildingManager = FindObjectOfType<BuildingManager>();
        currentJob = "Miner";
        cameraMain = Camera.main;
        Register();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
