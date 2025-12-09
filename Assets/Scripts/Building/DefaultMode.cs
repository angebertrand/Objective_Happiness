using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMode : MonoBehaviour
{
    // Initialize variables
    public Camera myCamera;
    
    
    public GameObject building;
    
    private CharacterScript character;

    
    private GameObject school;

    


    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray myRay = myCamera.ScreenPointToRay(mousePosition);
        bool weHitSomething = Physics.Raycast(myRay, out RaycastHit hit);

        

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
