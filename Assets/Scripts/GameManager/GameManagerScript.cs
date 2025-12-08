using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public List<CharacterScript> characters = new List<CharacterScript>();

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
        StartCoroutine(CharacterListIntegrityCheck());
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
    private IEnumerator CharacterListIntegrityCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            CleanCharacterList();
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

        Debug.Log("✔ Character list cleaned. Count = " + characters.Count);
    }


    public string GetRandomName()
    {
        int index = Random.Range(0, names.Count);
        Debug.Log(names[index]);
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
                Debug.LogWarning("Structure with unrecognized tag: " + structure.name);
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
}
