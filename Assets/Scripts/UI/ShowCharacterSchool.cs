using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public class ShowCharacterSchool : MonoBehaviour
{
    public GameManagerScript gameManager;
    public GameObject characterItemPrefab;
    public Transform container;

    public SchoolScript SchoolScript;

    void Start()
    {
        RefreshList();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }
    private void OnEnable()
    {
        Invoke("RefreshList", 1f);
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    public void RefreshList()
    {
        // Clean container
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // List in gamemanager
        foreach (CharacterScript character in gameManager.characters)
        {
            if (character != null)
            {
                GameObject item = Instantiate(characterItemPrefab, container);
                // element
                Image icon = item.transform.Find("Icon").GetComponent<Image>();
                TMP_Text nameText = item.transform.Find("Name").GetComponent<TMP_Text>();
                Button btnFarm = item.transform.Find("ButtonFarm").GetComponent<Button>();
                Button btnMiner = item.transform.Find("ButtonMiner").GetComponent<Button>();
                Button btnWoods = item.transform.Find("ButtonWoods").GetComponent<Button>();
                Button btnMason = item.transform.Find("ButtonMason").GetComponent<Button>();

                // Assign value
                icon.sprite = character.icon;        
                nameText.text = character.name;

                btnFarm.onClick.AddListener(() => Clicking("Farmer", character));
                btnMiner.onClick.AddListener(() => Clicking("Miner", character));
                btnWoods.onClick.AddListener(() => Clicking("Woods", character));
                btnMason.onClick.AddListener(() => Clicking("Mason", character));
            }
                      
        }

        Invoke("RefreshList", 1f);
    }

    public void Clicking(string job, CharacterScript character)
    {
        if (gameManager.day)
        {
            if (job != character.currentJob && character.isLearning == false)
            {
                Debug.Log("YAHOUU");
                SchoolScript.goodSFX.Play();
                character.GoToSchool(this.SchoolScript.gameObject, job);
                SchoolScript.isSomeoneLearning = true;
                SchoolScript.HideSchoolCanva();
            }
            else
            {
                SchoolScript.wrongSFX.Play();
            }
        }

        
    }
}
