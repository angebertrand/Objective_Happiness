using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : MonoBehaviour
{
    //Initialise variables
    public Camera myCamera;
    public Material buildableMaterial;
    public Material notBuildableMaterial;
    public GameObject building;
    public List<Material> lastHexaMaterials;
    private Material lastHexaMaterial;
    public List<GameObject> lastHexas;
    private GameObject lastHexa;
    public bool bigBuilding;
    private int buildableCount;

    void Start()
    {
        bigBuilding = building.GetComponent<BuildingParameter>().bigBuilding;
    }
    void OnEnable()
    {
        bigBuilding = building.GetComponent<BuildingParameter>().bigBuilding;
    }

    // Update is called once per frame
    void Update()
    {
        //Initialise a raycast that detect what the cursor's pointing at
        Vector3 mousePosition = Input.mousePosition;
        Ray myRay = myCamera.ScreenPointToRay(mousePosition);
        bool weHitSomething = Physics.Raycast(myRay, out RaycastHit hit);

        //Check if the pointed object is a tile
        if (weHitSomething && hit.transform.CompareTag("Tile"))
        {
            //Check if the pointed tile is the same as in previous frame
            if (lastHexas.Count > 0 && lastHexas[0] != hit.transform.gameObject)
            {
                /*Identify the tiles your pointing*/
                //Get their materials back to the tiles

                foreach (GameObject hexa in lastHexas)
                {
                    hexa.transform.GetComponentInChildren<MeshRenderer>().material = lastHexaMaterials[lastHexas.IndexOf(hexa)];
                }
                //Clear the Lists
                lastHexas.Clear();
                lastHexaMaterials.Clear();
                //Add the new pointed tile in Lists
                lastHexas.Add(hit.transform.gameObject);
                lastHexaMaterials.Add(lastHexas[0].GetComponentInChildren<MeshRenderer>().material);
                Debug.Log(lastHexaMaterials[0]);
                //If we are building a big building add sister tiles too
                if (bigBuilding)
                {
                    foreach (GameObject sisterTile in lastHexas[0].GetComponent<TileSettings>().sisterTiles)
                    {
                        lastHexas.Add(sisterTile);
                        lastHexaMaterials.Add(sisterTile.GetComponentInChildren<MeshRenderer>().material);
                    }
                }
                //Color the tile in the corresponding color for feedback
                //isBuildableOn = true
                if (hit.transform.GetComponent<TileSettings>().isBuildableOn)
                {
                    lastHexas[0].GetComponentInChildren<MeshRenderer>().material = buildableMaterial;
                }
                //isBuildableOn = false
                else
                {
                    lastHexas[0].GetComponentInChildren<MeshRenderer>().material = notBuildableMaterial;
                }
                //If bigBuilding is true we do this on every sisterTile too
                if (bigBuilding)
                {
                    foreach (GameObject sisterTile in lastHexas[0].GetComponent<TileSettings>().sisterTiles)
                    {
                        if (sisterTile.GetComponent<TileSettings>().isBuildableOn)
                        {
                            sisterTile.GetComponentInChildren<MeshRenderer>().material = buildableMaterial;
                        }
                        else
                        {
                            sisterTile.GetComponentInChildren<MeshRenderer>().material = notBuildableMaterial;
                        }
                    }
                }
            }
            else if (lastHexas.Count <= 0)
            {
                if (weHitSomething)
                {
                    lastHexas.Add(hit.transform.gameObject);
                }
            }
            //checking if every Tile(s) is/are buildable
            foreach (GameObject hexa in lastHexas)
            {
                if (hexa.GetComponent<TileSettings>().isBuildableOn) { buildableCount++; }
            }
            //If a tile is clicked and buildable
            if (Input.GetKeyDown(KeyCode.Mouse0) && buildableCount == lastHexas.Count)
            {
                //Generate building, change isBuildableOn of the targeted tile to false and exit building mode
                Instantiate(building, new Vector3(lastHexas[0].transform.position.x, lastHexas[0].transform.position.y, lastHexas[0].transform.position.z), lastHexas[0].transform.rotation);
                lastHexas[0].transform.GetComponent<TileSettings>().isBuildableOn = false;
                ExitBuildingMode();
            }
            //Exit Building Mode if B is pressed
            if (Input.GetKeyDown(KeyCode.B))
            {
                ExitBuildingMode();
            }
            buildableCount = 0;
        }
    }
    private void ExitBuildingMode()
    {
        lastHexas[0].transform.GetComponentInChildren<MeshRenderer>().material = lastHexaMaterial;
        GetComponentInParent<PlayerScript>().isBuilding = false;
        this.enabled = false;
    }
}