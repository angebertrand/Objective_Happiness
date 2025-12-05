using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public List<CharacterScript> characters = new List<CharacterScript>();
    public int nPopulation = 0;
    public int nWood = 0;
    public int nFood = 0;
    public int Joy = 0;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        nPopulation = characters.Count;
    }

    // Create chara
    public void RegisterCharacter(CharacterScript character)
    {
        // If the character already has a valid ID
        if (character.ID >= 0 && character.ID < characters.Count)
        {
            Debug.Log("Replacing existing character at ID " + character.ID + " with " + character.name);
            characters[character.ID] = character;
        }
        else
        {
            // Otherwise, add to the end of the list and assign a new ID
            character.ID = characters.Count;
            characters.Add(character);
            Debug.Log("Registered new character: " + character.name + " | ID: " + character.ID);
        }
    }

    public void UnregisterCharacter(CharacterScript character)
    {
        if (characters.Contains(character))
        {
            characters.Remove(character);
            Debug.Log("Character unregistered: " + character.name);
        }
    }
}
