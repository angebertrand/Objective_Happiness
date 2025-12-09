using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode_temp : MonoBehaviour
{
    //Initialise variables
    public Camera myCamera;
    public Material buildableMaterial;
    public Material notBuildableMaterial;
    public GameObject building;
    private List<Material> lastHexaMaterials;
    private Material lastHexaMaterial;
    public List<GameObject> lastHexas;
    private GameObject school;
    private CharacterScript character;
    private GameManagerScript gameManager;
    private bool bigBuilding;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManagerScript>();

        
    }
  

    // Update is called once per frame
    void Update()
    {
        //Initialise a raycast that detect what the cursor's pointing at
        Vector3 mousePosition = Input.mousePosition;
        Ray myRay = myCamera.ScreenPointToRay(mousePosition);
        bool weHitSomething = Physics.Raycast(myRay, out RaycastHit hit);

        //Check if the pointed object is a tile
        if (weHitSomething && hit.transform.CompareTag("School"))
        {

            if (school != hit.transform.gameObject)
            { 
                 school = hit.transform.gameObject;
                
            }
            
            //If a tile is clicked and buildable
            if (Input.GetKeyDown(KeyCode.Mouse0) && !(school.transform.GetComponent<SchoolScript>().isSomeoneLearning))
            {
                //Generate building, change isBuildableOn of the targeted tile to false and exit building mode
                if (gameManager.day)
                {
                    SchoolScript schoolScript = school.transform.GetComponent<SchoolScript>();
                    schoolScript.ShowSchoolCanva();
                }
                
                


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
}
