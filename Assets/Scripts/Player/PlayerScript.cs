using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool isBuilding = false;
    public GameObject buildInterface;
    public DefaultMode defaultMode;
    public GameManagerScript gameManagerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        defaultMode = GetComponent<DefaultMode>();
        gameManagerScript = GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();

        if (isBuilding && !GetComponentInChildren<BuildMode>().enabled)
        {
            buildInterface.SetActive(true);
        }
    }
    private void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.B) && GetAvailableMason() != null)
        {
            
            isBuilding = true;
            defaultMode.enabled = false;
        }
    }

    private MasonScript GetAvailableMason()
    {
        foreach (CharacterScript c in gameManagerScript.characters)
        {
            MasonScript mason = c as MasonScript;

            if (mason != null)
            {
                if (!mason.isWorking && !mason.isLearning && mason.isJobless == false && mason.isBuildingSomething == false)
                {
                    return mason;
                }
            }
        }

        return null;
    }
}
