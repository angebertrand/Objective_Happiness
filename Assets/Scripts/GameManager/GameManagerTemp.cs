using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerTemp : MonoBehaviour
{
    // Buildings :
    public List<GameObject> houses = new List<GameObject>();
    public List<GameObject> farms = new List<GameObject>();
    public List<GameObject> libraries = new List<GameObject>();
    public List<GameObject> museums = new List<GameObject>();
    public List<GameObject> stones = new List<GameObject>();
    public List<GameObject> bushes = new List<GameObject>();
    public List<GameObject> forests = new List<GameObject>();




    public int nPopulation = 0;

    public int nHouses = 0;
    public int nFarms = 0;
    public int nLibraries = 0;
    public int nMuseums = 0;
    public int nStones = 0;
    public int nBushes = 0;
    public int nForests = 0;

    public int nWood = 0;
    public int nFood = 0;
    public int nStone = 0;
    public float Joy = 0;
    
    public float timeOfDay;
    public bool day;

    public List<CharacterScript> characters = new List<CharacterScript>();


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(dayCoroutine());
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

    private void StartDayCicle()
    {
        StartCoroutine(dayCoroutine());
    }

    IEnumerator dayCoroutine()
    {
        GameObject sun = GameObject.FindGameObjectWithTag("Sun");
        day = true;
        sun.GetComponent<Light>().color = new Color(0.9849057f, 0.8108497f, 0.3400711f);
        Debug.Log("Il fait jour.");
        yield return new WaitForSeconds(10f);
        day = false;
        sun.GetComponent<Light>().color = new Color(0.1071307f, 0.09558551f, 0.6754716f);
        Debug.Log("Il fait nuit.");
        yield return new WaitForSeconds(5f);

        StartCoroutine(dayCoroutine());
    }
}
