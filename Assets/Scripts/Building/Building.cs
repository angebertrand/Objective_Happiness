using UnityEngine;

public class Building : MonoBehaviour
{
    public string buildingType;
    public bool isUsed = false;
    public int tileType;            //0 = plain, 1 = Bush, 2 = Stone, 3 = Forest
    public bool bigBuilding = false;
    public BuildingManager manager;

    private void Awake()
    {
        buildingType = tag;
        manager = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<BuildingManager>();

        manager.RegisterBuilding(this);
    }
}
