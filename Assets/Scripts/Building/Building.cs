using UnityEngine;

public class Building : MonoBehaviour
{
    public string buildingType;
    public bool isUsed = false;
    public BuildingManager manager;

    private void Awake()
    {
        buildingType = tag;
        
        manager = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<BuildingManager>();

        manager.RegisterBuilding(this);

        
        
        



    }
}
