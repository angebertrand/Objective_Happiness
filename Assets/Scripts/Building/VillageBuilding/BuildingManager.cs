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


    public Building GetBuildingForJob(string jobName, CharacterScript requester)
    {
        if (!jobToBuilding.ContainsKey(jobName))
            return null;

        string buildingType = jobToBuilding[jobName];

        return GetFreeBuilding(buildingType, requester);
    }





    public Building GetFreeBuilding(string type, CharacterScript requester = null)
    {
        if (!buildings.ContainsKey(type))
            return null;

        Building closest = null;
        float closestDist = Mathf.Infinity;

        foreach (Building b in buildings[type])
        {
            if (b == null || b.isUsed)
                continue;

            // If a house -> check if occupied
            if (type == "House")
            {
                HouseScript hs = b.GetComponent<HouseScript>();
                if (hs != null && hs.isOccupied)
                    continue;
            }

           
            if (requester == null)
            {
                closest = b;
                break;
            }

            
            float dist = Vector3.Distance(requester.transform.position, b.transform.position);

            if (dist < closestDist)
            {
                closest = b;
                closestDist = dist;
            }
        }

        if (closest != null)
            closest.isUsed = true;

        return closest;
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

