using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : MonoBehaviour
{
    // Initialize variables
    public Camera myCamera;
    public Material buildableMaterial;
    public Material notBuildableMaterial;
    public GameObject building;
    private CharacterScript character;

    // Lists to track hovered hexagons and their original materials
    public List<Material> lastHexaMaterials = new List<Material>();
    public List<GameObject> lastHexas = new List<GameObject>();

    public bool bigBuilding; // 1 hexagon if false, multiple (e.g., 4) hexagons if true
    private int buildableCount;
    private GameObject school;

    // Define the required size for a big building (assuming 4 hexagons)
    private const int BIG_BUILDING_SIZE = 4;

    void Start()
    {
        // Ensure BuildingParameter is on the 'building' object
        if (building != null && building.GetComponent<BuildingParameter>() != null)
        {
            bigBuilding = building.GetComponent<BuildingParameter>().bigBuilding;
        }
    }

    void OnEnable()
    {
        // Reset building size on each activation
        if (building != null && building.GetComponent<BuildingParameter>() != null)
        {
            bigBuilding = building.GetComponent<BuildingParameter>().bigBuilding;
        }
        // Ensure lists are empty upon activation
        lastHexas.Clear();
        lastHexaMaterials.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray myRay = myCamera.ScreenPointToRay(mousePosition);
        bool weHitSomething = Physics.Raycast(myRay, out RaycastHit hit);

        // --- HOVER MANAGEMENT ON TILES (Hexagons) ---
        if (weHitSomething && hit.transform.CompareTag("Tile"))
        {
            buildableCount = 0;

            GameObject currentHitHexa = hit.transform.gameObject;
            bool newHexaHovered = lastHexas.Count == 0 || lastHexas[0] != currentHitHexa;

            if (newHexaHovered)
            {
                // 1. Restore materials of previous hexagons
                for (int i = 0; i < lastHexas.Count; i++)
                {
                    if (lastHexas[i] != null && i < lastHexaMaterials.Count)
                    {
                        lastHexas[i].transform.GetComponentInChildren<MeshRenderer>().material = lastHexaMaterials[i];
                    }
                }

                // 2. Populate the new lists
                lastHexas.Clear();
                lastHexaMaterials.Clear();

                // Add the main hexagon
                lastHexas.Add(currentHitHexa);
                lastHexaMaterials.Add(currentHitHexa.GetComponentInChildren<MeshRenderer>().material);

                // If it's a big building, add "sister tiles" too
                if (bigBuilding && currentHitHexa.GetComponent<TileSettings>() != null)
                {
                    foreach (GameObject sisterTile in currentHitHexa.GetComponent<TileSettings>().sisterTiles)
                    {
                        if (sisterTile != null)
                        {
                            lastHexas.Add(sisterTile);
                            lastHexaMaterials.Add(sisterTile.GetComponentInChildren<MeshRenderer>().material);
                        }
                    }
                }
            }
            else if (weHitSomething && hit.transform.CompareTag("School"))
            {

                if (school != hit.transform.gameObject)
                {
                    school = hit.transform.gameObject;

                }

                //If a tile is clicked and buildable
                if (Input.GetKeyDown(KeyCode.Mouse0) && !(school.transform.GetComponent<SchoolScript>().isSomeoneLearning))
                {
                    //Generate building, change isBuildableOn of the targeted tile to false and exit building mode
                    Debug.Log(school.transform.GetComponent<SchoolScript>().isSomeoneLearning);
                    SchoolScript schoolScript = school.transform.GetComponent<SchoolScript>();
                    schoolScript.ShowSchoolCanva();


                }

            }
            else if (weHitSomething && hit.transform.CompareTag("Character"))
            {

                if (character != hit.transform.gameObject)
                {
                    character = hit.transform.gameObject.GetComponent<CharacterScript>();

                }
                character.BeingHover();

            }
            else
            {
                if (character != null)
                {
                    character.StopBeingHover();
                }
            }

            // --- 3. Global buildability check AND SIZE CHECK ---

            // Count buildable tiles
            foreach (GameObject hexa in lastHexas)
            {
                if (hexa != null && hexa.GetComponent<TileSettings>() != null && hexa.GetComponent<TileSettings>().isBuildableOn)
                {
                    buildableCount++;
                }
            }

            // Check if the area is large enough for the building
            bool isSizeCorrect = true;
            if (bigBuilding)
            {
                // For a big building, we MUST have exactly BIG_BUILDING_SIZE (e.g., 4) hexagons in the list
                isSizeCorrect = (lastHexas.Count == BIG_BUILDING_SIZE);
            }
            // For a small building, we MUST have exactly 1 hexagon
            else
            {
                isSizeCorrect = (lastHexas.Count == 1);
            }

            // The area is fully buildable only if all tiles are free AND the size is correct
            bool isFullyBuildable = isSizeCorrect && (buildableCount == lastHexas.Count);
            Material feedbackMaterial = isFullyBuildable ? buildableMaterial : notBuildableMaterial;

            // 4. Apply the feedback material to all selected hexagons (every frame)
            foreach (GameObject hexa in lastHexas)
            {
                if (hexa != null)
                {
                    hexa.transform.GetComponentInChildren<MeshRenderer>().material = feedbackMaterial;
                }
            }

            // --- BUILDING LOGIC (Left Click) ---
            // Clicking is allowed only if isFullyBuildable is true
            if (Input.GetKeyDown(KeyCode.Mouse0) && isFullyBuildable)
            {
                // Instantiate the building at the position of the main hexagon
                Instantiate(building,
                    new Vector3(lastHexas[0].transform.position.x, lastHexas[0].transform.position.y, lastHexas[0].transform.position.z),
                    lastHexas[0].transform.rotation);

                // Disable buildability on ALL used hexagons
                foreach (GameObject builtHexa in lastHexas)
                {
                    if (builtHexa != null && builtHexa.GetComponent<TileSettings>() != null)
                    {
                        builtHexa.transform.GetComponent<TileSettings>().isBuildableOn = false;
                    }
                }

                ExitBuildingMode();
            }

            // --- EXIT BUILDING MODE (Key B) ---
            if (Input.GetKeyDown(KeyCode.B))
            {
                ExitBuildingMode();
            }
        }
        // --- HOVER MANAGEMENT OUTSIDE A TILE ---
        else
        {
            // --- SCHOOL INTERACTION MANAGEMENT ---
            if (weHitSomething && hit.transform.CompareTag("School"))
            {
                school = hit.transform.gameObject;

                if (Input.GetKeyDown(KeyCode.Mouse0) && school.transform.GetComponent<SchoolScript>() != null)
                {
                    SchoolScript schoolScript = school.transform.GetComponent<SchoolScript>();
                    // Check the learning condition before displaying the Canvas
                    if (!schoolScript.isSomeoneLearning)
                    {
                        schoolScript.ShowSchoolCanva();
                    }
                }
            }
        }
    }

    private void ExitBuildingMode()
    {
        // Restore original materials of ALL hexagons
        for (int i = 0; i < lastHexas.Count; i++)
        {
            // Safety check for null reference
            if (lastHexas[i] != null && i < lastHexaMaterials.Count)
            {
                MeshRenderer renderer = lastHexas[i].transform.GetComponentInChildren<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = lastHexaMaterials[i];
                }
            }
        }

        // Clear the lists
        lastHexas.Clear();
        lastHexaMaterials.Clear();

        // Disable building mode
        GetComponentInParent<PlayerScript>().isBuilding = false;
        this.enabled = false;
    }
}