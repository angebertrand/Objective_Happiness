using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public class ShowCharacterSchool : MonoBehaviour
{
    public GameManagerScript gameManager;
    public GameObject characterItemPrefab;
    public Transform container;
    public AudioSource wrongSFX;
    public AudioSource goodSFX;

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
                character.wrongSFX = wrongSFX;
                character.goodSFX = goodSFX;
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
            
                btnFarm.onClick.AddListener(() => character.GoToSchool(GameObject.FindGameObjectWithTag("School"), "Farmer"));
                btnMiner.onClick.AddListener(() => character.GoToSchool(GameObject.FindGameObjectWithTag("School"), "Miner"));
                btnWoods.onClick.AddListener(() => character.GoToSchool(GameObject.FindGameObjectWithTag("School"), "Woods"));
                btnMason.onClick.AddListener(() => character.GoToSchool(GameObject.FindGameObjectWithTag("School"),"Mason"));
            }
                      
        }

        Invoke("RefreshList", 0.8f);
    }


}
