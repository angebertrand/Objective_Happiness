using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour
{

    public CharacterScript currentCharacter;
    public bool isOccupied;
    GameManagerScript gameManagerScript;
    public Building buildingScript;


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
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameManagerScript.day)
        {
            if (other.CompareTag("Character"))
            {
                CharacterScript cs = other.GetComponent<CharacterScript>();

                if (cs == currentCharacter)
                {
                    isOccupied = true;
                    currentCharacter.isSleeping = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Character"))
        {
            isOccupied = false;
            currentCharacter.isSleeping = false;
            buildingScript.isUsed = false;
        }
        
    }

}
