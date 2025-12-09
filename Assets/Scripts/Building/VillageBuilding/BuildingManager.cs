using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    private Dictionary<string, List<Building>> buildings = new Dictionary<string, List<Building>>();

    public Dictionary<string, string> jobToBuilding = new Dictionary<string, string>()
    {
        { "Farmer", "Bush" },
        { "Miner", "Stone" },
        { "Woods", "Forest" },
    };

    private void Awake()
    {

    }



    public void RegisterBuilding(Building b)
    {
        if (!buildings.ContainsKey(b.buildingType))
            buildings.Add(b.buildingType, new List<Building>());

        buildings[b.buildingType].Add(b);

        
    }


    public Building GetBuildingForJob(string jobName)
    {
        if (!jobToBuilding.ContainsKey(jobName))
        {
            
            return null;
        }

        string buildingType = jobToBuilding[jobName];

        return GetFreeBuilding(buildingType);
    }
    public Building GetFreeBuilding(string type)
    {
        if (!buildings.ContainsKey(type)) return null;

        foreach (Building b in buildings[type])
        {
            if (!b.isUsed)
            {
                // Si c'est une maison, vérifier si elle est occupée
                if (type == "House")
                {
                    HouseScript hs = b.GetComponent<HouseScript>();
                    if (hs != null && hs.isOccupied)
                        continue;
                }

                b.isUsed = true;
                return b;
            }
        }
        return null;
    }

    public void ReleaseBuilding(Building b)
    {
        if (b != null)
            b.isUsed = false;
    }

    public bool HasBuildingType(string type)
    {
        return buildings.ContainsKey(type) && buildings[type].Count > 0;
    }
}

