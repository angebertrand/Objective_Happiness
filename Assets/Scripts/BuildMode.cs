using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : MonoBehaviour
{
    public Camera myCamera;
    public Material BuildableMaterial;
    public Material notBuildableMaterial;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray myRay = myCamera.ScreenPointToRay(mousePosition);

        RaycastHit hit;

        bool weHitSomething = Physics.Raycast(myRay, out hit);

        if (weHitSomething && hit.transform.tag == "Tile")
        {
            if (hit.transform.GetComponent<TileSettings>().isBuildableOn)
            {
                hit.transform.GetComponentInChildren<MeshRenderer>().material = BuildableMaterial; 
            }
        }
        else
        {

        }
    }
}
