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
    private Material lastHexaMaterial;
    private GameObject lastHexa;

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

            if (lastHexa != hit.transform.gameObject)
            {
                //Identify the tile your pointing
                if (lastHexa != null)
                {
                    lastHexa.transform.GetComponentInChildren<MeshRenderer>().material = lastHexaMaterial;
                    lastHexa = hit.transform.gameObject;
                }
                else
                {
                    lastHexa = hit.transform.gameObject;
                }
                //Color the tile in the corresponding color for feedback
                //isBuildableOn = true
                if (hit.transform.GetComponent<TileSettings>().isBuildableOn)
                {
                    lastHexaMaterial = hit.transform.GetComponentInChildren<MeshRenderer>().material;
                    hit.transform.GetComponentInChildren<MeshRenderer>().material = buildableMaterial;
                }
                //isBuildableOn = false
                else
                {
                    lastHexaMaterial = hit.transform.GetComponentInChildren<MeshRenderer>().material;
                    hit.transform.GetComponentInChildren<MeshRenderer>().material = notBuildableMaterial;
                }
            }
            //If a tile is clicked and buildable
            if (Input.GetKeyDown(KeyCode.Mouse0) && lastHexa.transform.GetComponent<TileSettings>().isBuildableOn)
            {
                //Generate building, change isBuildableOn of the targeted tile to false and exit building mode
                Instantiate(building, new Vector3(lastHexa.transform.position.x, lastHexa.transform.position.y, lastHexa.transform.position.z), lastHexa.transform.rotation);
                lastHexa.transform.GetComponent<TileSettings>().isBuildableOn = false;
                ExitBuildingMode();
            }
            //Exit Building Mode if B is pressed
            if (Input.GetKeyDown(KeyCode.B))
            {
                ExitBuildingMode();
            }
        }
    }
    private void ExitBuildingMode()
    {
        lastHexa.transform.GetComponentInChildren<MeshRenderer>().material = lastHexaMaterial;
        GetComponentInParent<PlayerScript>().isBuilding = false;
        this.enabled = false;
    }
}