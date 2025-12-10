using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public List<CharacterScript> characters = new List<CharacterScript>();
    public GameObject spawnpoint;

    // Buildings :
    public List<GameObject> houses = new List<GameObject>();
    public List<GameObject> farms = new List<GameObject>();
    public List<GameObject> libraries = new List<GameObject>();
    public List<GameObject> museums = new List<GameObject>();
    public List<GameObject> stones = new List<GameObject>();
    public List<GameObject> bushes = new List<GameObject>();
    public List<GameObject> forests = new List<GameObject>();

    GameManagerUIScript gameManagerUIScript;

    public int nPopulation = 0;
    
    public int nHouses = 0;
    public int nFarms = 0;
    public int nLibraries = 0;
    public int nMuseums = 0;
    public int nStones = 0;
    public int nBushes = 0;
    public int nForests = 0;
    public int dayCount = 0;

    public float timeOfDay;
    public bool day;
    public bool canGoToNextDay = false;

    public AudioSource PausedSound;
    public AudioSource UnPausedSound;

    public int nWood = 0;
    public int nFood = 0;
    public int nStone = 0;

    public bool isPaused = false;
    public float pauseDuration = 0.6f;

    public float Joy = 0;

    private List<string> names = new List<string>()
    {
        "Zhyr'ka", "Tolmari", "Krexilon", "Veshaar", "Ollivrax",

        "Murok'Tai", "Khalyrex", "Syrvanna", "Tor'Vek", "Ylithra",

        "Zor'Kun", "Xelvaan", "Brakkon", "Kel'Vora", "Thryzann",

        "Ossa'Luun", "Vek'thar", "Marzellek", "Ishvori", "Urrak'Hol",

        "Vruzzik", "Kael'Ruun", "Lythyx", "Drox'Men", "Tzer'Lo",

        "Xyra'Ven", "Qorvannik", "Yss'Thra", "Brelkor", "Tarvax",

        "Xun'Kala", "Graathor", "Nir'Voth", "Voltharik", "Srak'Ni",

        "Jha'Rex", "Vor'Kessa", "Ulvath", "Praxion", "Kroth'Rai",

        "Nex'Zyr", "Zihrax", "Velxori", "Tarnukk", "Syllath",

        "Gryz'Nar", "Elthovex", "Kyr'Zaal", "Orvathos", "Mynthora"
    };


    // Start is called before the first frame update
    void Start()
    {
        gameManagerUIScript = FindAnyObjectByType<GameManagerUIScript>();
        StartCoroutine(CharacterListIntegrityCheck());
        StartCoroutine(dayCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        nPopulation = characters.Count;
        nHouses = houses.Count;
        nFarms = farms.Count;
        nLibraries = libraries.Count;
        nMuseums = museums.Count;
        nStones = stones.Count;
        nBushes = bushes.Count;
        nForests = forests.Count;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isPaused)
            {
                PausedSound.Play();
                StartCoroutine(SmoothPauseCoroutine());
            }
                
            else
            {
                UnPausedSound.Play();
                StartCoroutine(SmoothResumeCoroutine());
            }
                
        }


    }

    IEnumerator dayCoroutine()
    {
        
        GameObject sun = GameObject.FindGameObjectWithTag("Sun");

        day = true;
        dayCount++;
        //spawnpoint.GetComponent<SpawnpointScript>().InstanciateWanderer(dayCount);
        sun.GetComponent<Light>().color = new Color(0.9849057f, 0.8108497f, 0.3400711f);
        Debug.Log("Il fait jour.");

        // ← AJOUT : réinitialiser toutes les maisons
        foreach (GameObject house in houses)
        {
            if (house == null) continue;
            HouseScript hs = house.GetComponent<HouseScript>();
            if (hs != null)
            {
                hs.isOccupied = false;
            }
        }

        List<CharacterScript> charactersCopy = new List<CharacterScript>(characters);

        foreach (CharacterScript c in charactersCopy)
        {
            c.isSleeping = false;
            c.sleepiness = false;

            if (c.isJobless)
            {
                c.GoToSchool(c.JobBuilding, c.nextJob);
            }
            else
            {
                if (!c.isWorking)
                {
                    c.GoToWork();
                }
                
            }

        }


        yield return new WaitForSeconds(15f);  // Lenght of a day //////////

        day = false;
        sun.GetComponent<Light>().color = new Color(0.1071307f, 0.09558551f, 0.6754716f);
        Debug.Log("Il fait nuit.");
        ResetBuildingsUsage();
        FinishDay();
        UpdateJoy();
        gameManagerUIScript.happinessReset(Joy/100);
        
    }

    private IEnumerator CharacterListIntegrityCheck()
    {
        while (true)
        {

            CleanCharacterList();
            yield return new WaitForSeconds(3f);
        }
    }
    private IEnumerator SmoothPauseCoroutine()
    {
        isPaused = true;
        float elapsed = 0f;
        float startTimeScale = Time.timeScale;
        float targetTimeScale = 0f;

        while (elapsed < pauseDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(startTimeScale, targetTimeScale, elapsed / pauseDuration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }

        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f; // physique complètement arrêtée
    }

    // Smooth Resume
    private IEnumerator SmoothResumeCoroutine()
    {
        float elapsed = 0f;
        float duration = pauseDuration;
        float startTimeScale = Time.timeScale;
        float targetTimeScale = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(startTimeScale, targetTimeScale, elapsed / duration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        isPaused = false;
    }

    private void ResetBuildingsUsage()
    {
        // Toutes les listes de bâtiments sauf maisons
        List<GameObject>[] buildingLists = new List<GameObject>[] { stones, bushes, forests };

        foreach (var list in buildingLists)
        {
            foreach (GameObject b in list)
            {
                if (b == null) continue;
                Building buildingScript = b.GetComponent<Building>();
                if (buildingScript != null)
                {
                    buildingScript.isUsed = false;
                }
            }
        }
    }

    public void UpdateJoy()
    {
        bool AllDead = true;

        List<CharacterScript> charactersCopy = new List<CharacterScript>(characters);
        foreach (CharacterScript character in charactersCopy)
        {
            if (character != null)
            {
                Joy += character.isHappy;
                AllDead = false;
            }
                
        }

        if (AllDead)
        {
            SceneManager.LoadScene("EndScene_loose");
        }

        if (Joy >= 100)
        {
            SceneManager.LoadScene("EndScene_win");
        }
        
    }
    private void CleanCharacterList()
    {
        // Liste des personnages réellement présents dans la scène
        CharacterScript[] existingCharacters = FindObjectsOfType<CharacterScript>();

        // 1. Retire les NULL
        characters.RemoveAll(c => c == null);

        // 2. Retire les doublons
        HashSet<CharacterScript> unique = new HashSet<CharacterScript>(characters);
        characters = new List<CharacterScript>(unique);

        // 3. Ajoute les personnages manquants dans la liste
        foreach (var character in existingCharacters)
        {
            if (!characters.Contains(character))
            {
                characters.Add(character);
            }
        }

        // 4. Réassigner des IDs propres (optionnel mais propre)
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].ID = i;
        }
    }


    public string GetRandomName()
    {
        int index = Random.Range(0, names.Count);
        
        return names[index];
    }

    // Create chara
    public void RegisterCharacter(CharacterScript character)
    {
        // If the character already has a valid ID
        if (character.ID >= 0 && character.ID < characters.Count)
        {
            
            characters[character.ID] = character;
        }
        else
        {
            // Otherwise, add to the end of the list and assign a new ID
            character.ID = characters.Count;
            /*
            if (!(names.Contains(character.name)))
            {
                character.name = GetRandomName();
                Debug.Log("CA CHANGE");
            }
            */
            character.name = GetRandomName();
            
            characters.Add(character);
            
        }
    }

    public void UnregisterCharacter(CharacterScript character)
    {
        if (characters.Contains(character))
        {
            characters.Remove(character);
           
        }
    }

    //Create buildings
    public void RegisterStructure(GameObject structure)
    {
        switch (structure.tag)
        {
            case "House": houses.Add(structure); break;
            case "Farm": farms.Add(structure); break;
            case "Library": libraries.Add(structure); break;
            case "Museum": museums.Add(structure); break;
            case "Stone": stones.Add(structure); break;
            case "Bush": bushes.Add(structure); break;
            case "Forest": forests.Add(structure); break;
            default:
                
                break;
        }
    }

    public void UnregisterStructure(GameObject structure)
    {
        switch (structure.tag)
        {
            case "House": houses.Remove(structure); break;
            case "Farm": farms.Remove(structure); break;
            case "Library": libraries.Remove(structure); break;
            case "Museum": museums.Remove(structure); break;
            case "Stone": stones.Remove(structure); break;
            case "Bush": bushes.Remove(structure); break;
            case "Forest": forests.Remove(structure); break;
        }
    }


    public bool ConsumeFood(int amount)
    {
        if (nFood < amount)
            return false;

        nFood -= amount;
        return true;
    }

    private void StartDayCicle()
    {
        StartCoroutine(dayCoroutine());
    }

    private void FinishDay()
    {
        List<CharacterScript> charactersCopy = new List<CharacterScript>(characters); 
        foreach (CharacterScript character in charactersCopy) 
        { 
            if (character != null) 
                character.EndOfTheDay(); 
        }

        // Ensuite on attend que la condition soit vraie
        StartCoroutine(WaitForSleepingOrHousesFull());
    }


    private IEnumerator WaitForSleepingOrHousesFull()
    {
        while (true)
        {
            bool everyoneSleeping = true;

            List<CharacterScript> charactersCopy = new List<CharacterScript>(characters);
            foreach (CharacterScript character in charactersCopy)

                foreach (CharacterScript c in charactersCopy)
                {
                    if (c.isSleeping == false)
                    {
                        
                        everyoneSleeping = false;
                        break;
                    }
                }
            
            bool allHousesOccupied = AreAllHousesOccupied();

            // Condition OK ?
            if (everyoneSleeping || allHousesOccupied)
            {
                Debug.Log("✔ Conditions remplies : passage au jour suivant.");
                

                yield return new WaitForSeconds(4f);

                

                StartCoroutine(dayCoroutine());
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private bool AreAllHousesOccupied()
    {
        foreach (GameObject house in houses)
        {
            if (house == null) continue;

            HouseScript h = house.GetComponent<HouseScript>();
            if (h == null)
            {
                h = house.GetComponentInChildren<HouseScript>();
            }

            if (h == null)
            {
                Debug.LogWarning("Maison sans HouseScript détectée : " + house.name);
                return false;
            }

            if (!h.isOccupied)
            {
                return false;
            }
        }

        return true;
    }

}
