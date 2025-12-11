using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : MonoBehaviour
{
    // Initialize variables
    private Camera myCamera;

    public Material buildableMaterial;
    public Material notBuildableMaterial;

    public GameObject building;
    private Building buildingScript;

    public GameObject construction;
    private GameManagerScript gameManager;
    private WarningMessagesScript warningMessages;
    private CharacterScript character;
    private GameObject PressBText;

    // Lists to track hovered hexagons and their original materials
    public List<Material> lastHexaMaterials = new();
    public List<GameObject> lastHexas = new();

    public bool bigBuilding; // 1 hexagon if false, multiple hexagons if true
    private int buildableCount;

    // Define the required size for a big building (assuming 4 hexagons)
    private const int BIG_BUILDING_SIZE = 4;
    private void Awake()
    {
        //Link the UI to the code
        PressBText = GameObject.Find("Press B to go back");
        PressBText.SetActive(false);
    }
    void Start()
    {
        //Link Camera, GameManager and WarningMessages to the code
        myCamera = Camera.main;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        warningMessages = GameObject.Find("WarningMessages").GetComponent<WarningMessagesScript>();
        // Ensure BuildingParameter is on the 'building' object
        if (building != null && buildingScript != null)
        {
            bigBuilding = buildingScript.bigBuilding;
        }
    }

    void OnEnable()
    {
        //Activate UI and set the new buildingscript
        PressBText.SetActive(true);
        buildingScript = building.GetComponent<Building>();

        // Reset building size on each activation
        if (building != null && buildingScript != null)
        {
            bigBuilding = buildingScript.bigBuilding;
        }

        // Ensure lists are empty upon activation
        lastHexas.Clear();
        lastHexaMaterials.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        //Send a ray
        Vector3 mousePosition = Input.mousePosition;
        Ray myRay = myCamera.ScreenPointToRay(mousePosition);
        bool weHitSomething = Physics.Raycast(myRay, out RaycastHit hit);

        // --- HOVER MANAGEMENT ON TILES (Hexagons) ---
        //if the ray hit a tile
        if (weHitSomething && hit.transform.CompareTag("Tile"))
        {
            //Reset the count of buildable tile for later
            buildableCount = 0;

            GameObject currentHitHexa = hit.transform.gameObject;
            bool newHexaHovered = lastHexas.Count == 0 || lastHexas[0] != currentHitHexa;

            if (newHexaHovered)
            {
                // Restore materials of previous hexagons
                for (int i = 0; i < lastHexas.Count; i++)
                {
                    if (lastHexas[i] != null && i < lastHexaMaterials.Count)
                    {
                        lastHexas[i].GetComponentInChildren<MeshRenderer>().material = lastHexaMaterials[i];
                    }
                }

                // Clear the lists
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

            // --- Global buildability check AND SIZE CHECK ---

            // Count buildable tiles
            foreach (GameObject hexa in lastHexas)
            {
                if (hexa != null && hexa.GetComponent<TileSettings>() != null && hexa.GetComponent<TileSettings>().isBuildableOn)
                {
                    buildableCount++;
                }
            }

            // Check if the area is large enough for the building
            bool isSizeCorrect;
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
                    hexa.GetComponentInChildren<MeshRenderer>().material = feedbackMaterial;
                }
            }

            // --- BUILDING LOGIC (Left Click) ---
            // Clicking is allowed only if isFullyBuildable is true & the player has the requiered ressources
            // Ressources are also removed from inventory in the EnoughRessources function
            if (Input.GetKeyDown(KeyCode.Mouse0) && isFullyBuildable && EnoughRessources(building, gameManager))
            {
                // Instantiate the building at the position of the main hexagon
                construction.GetComponent<ConstructionScript>().futureBuilding = building;
                Instantiate(construction,
                    lastHexas[0].transform.position + building.transform.position,
                    building.transform.rotation);

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
            else if (Input.GetKeyDown(KeyCode.Mouse0) && !isFullyBuildable)
            {
                StartCoroutine(warningMessages.warningCoroutine(2));
            }

            // --- EXIT BUILDING MODE (Key B) ---
            if (Input.GetKeyDown(KeyCode.B))
            {
                ExitBuildingMode();
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
    }

    // --- Functions ---

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
        GetComponentInParent<DefaultMode>().enabled = true;
        PressBText.SetActive(false);
        this.enabled = false;
    }
    // SelectBuilding
    public void SelectBuilding(GameObject Building)
    {
        building = Building;
    }
    public bool EnoughRessources(GameObject building, GameManagerScript gameManager)
    {
        bool enoughRessources;
        if (buildingScript.woodCost <= gameManager.nWood &&
            buildingScript.stoneCost <= gameManager.nStone &&
            buildingScript.foodCost <= gameManager.nFood)
        {
            enoughRessources = true;
            gameManager.nWood -= buildingScript.woodCost;
            gameManager.nStone -= buildingScript.stoneCost;
            gameManager.nFood -= buildingScript.foodCost;

        }
        else
        {
            enoughRessources = false;
            StartCoroutine(warningMessages.warningCoroutine(1));
        }
        return enoughRessources; 
    }
}