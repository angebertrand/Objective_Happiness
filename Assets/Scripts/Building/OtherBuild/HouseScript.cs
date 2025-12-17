using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour
{

    public CharacterScript currentCharacter;
    public bool isOccupied;
    GameManagerScript gameManagerScript;
    public Building buildingScript;
    public GameObject textZZZ;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        gameManagerScript = FindAnyObjectByType<GameManagerScript>();
        buildingScript = this.gameObject.GetComponent<Building>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerScript.day && currentCharacter != null)
        {
            Vector3 newPos = currentCharacter.transform.position + currentCharacter.transform.right * 3f;
            currentCharacter.agent.Warp(newPos);
            currentCharacter.agent.isStopped = false;
            currentCharacter.agent.ResetPath();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameManagerScript.day)
        {
            if (other.CompareTag("Character") || other.CompareTag("Mason"))
            {
                CharacterScript cs = other.GetComponent<CharacterScript>();

                if (cs == currentCharacter)
                {
                    textZZZ.SetActive(true);
                    isOccupied = true;
                    currentCharacter.isSleeping = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Character") || other.CompareTag("Mason"))
        {
            if (currentCharacter != null && other.gameObject == currentCharacter.gameObject)
            {
                isOccupied = false;
                currentCharacter.isSleeping = false;
                buildingScript.isUsed = false;
                textZZZ.SetActive(false);
                currentCharacter = null;
            }
            
        }
        
    }

}
