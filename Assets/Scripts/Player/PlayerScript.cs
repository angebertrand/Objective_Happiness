using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool isBuilding = false;
    public GameObject buildInterface;
    public DefaultMode defaultMode;
    public GameManagerScript gameManagerScript;
    private WarningMessagesScript warningMessages;

    
    // Start is called before the first frame update
    void Start()
    {
        warningMessages = GameObject.Find("WarningMessages").GetComponent<WarningMessagesScript>();
    }

    private void Awake()
    {
        defaultMode = GetComponentInChildren<DefaultMode>();
        gameManagerScript = FindAnyObjectByType<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding && !GetComponentInChildren<BuildMode>().enabled)
        {
            buildInterface.SetActive(true);
        }
    }
    public void GetInputs()
    {
        if (!isBuilding)
        {
            if (GetAvailableMason() != null)
            {
                isBuilding = true;
                defaultMode.enabled = false;
            }
            else
            {
                StartCoroutine(warningMessages.warningCoroutine(0));
            }
        }
        else
        {
            isBuilding = false;
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
