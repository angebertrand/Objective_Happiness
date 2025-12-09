using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{
    public string buildingType;
    public bool isUsed = false;
    public int tileType;            //
    public bool bigBuilding = false;
    public BuildingManager manager;
    GameManagerScript gm;

    private void Awake()
    {
        buildingType = tag;
        
        manager = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<BuildingManager>();

        manager.RegisterBuilding(this);

        gm = FindAnyObjectByType<GameManagerScript>();
        gm.RegisterStructure(this.gameObject);

  

    }


}
