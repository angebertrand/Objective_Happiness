using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BushScript : MonoBehaviour
{
    public GameManagerScript gameManagerScript;
    public bool isSomeoneWorkingOnIt = false;
    public float percentUntilFood = 0f;
    public float percentOnFoodRate = 20f;
    public CharacterScript currentCharacter;
    public bool isMining = false;
    public ProgressBarScript progressBar;
    public GameObject canvasProgress;

    public Building buildingScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        gameManagerScript = FindObjectOfType<GameManagerScript>();
        buildingScript = this.gameObject.GetComponent<Building>();
    }

    // Update is called once per frame
    void Update()
    {
  

        if (isMining && currentCharacter != null)
        {
            canvasProgress.SetActive(true);
            // Increase learning progress over time
            percentUntilFood += percentOnFoodRate * Time.deltaTime;
            progressBar.UpdateProgress(percentUntilFood / 100);
            //Debug.Log("Progress: " + learningProgress + "%");

            // Check if learning is complete
            if (percentUntilFood >= 100f)
            {
                
                percentUntilFood = 0f;
                gameManagerScript.nFood += 1;
                progressBar.UpdateProgress(percentUntilFood / 100);
            }
        }
    }

    private IEnumerator CheckMiningState()
    {
        while (isMining)
        {
            // Le character existe toujours ?
            if (currentCharacter == null || currentCharacter.sleepiness)
            {
                currentCharacter = null;
                isMining = false;
                buildingScript.isUsed = false;
                canvasProgress.SetActive(false);
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character") && !isMining)
        {
            
            currentCharacter = other.GetComponent<CharacterScript>();
            
            if (currentCharacter.NextBuilding == this.gameObject)
            {
                isMining = true;
                StartCoroutine(CheckMiningState());
            }     

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentCharacter != null
            && other.CompareTag("Character")
            && other.gameObject == currentCharacter.gameObject
            )
        {

            isMining = false;
            canvasProgress.SetActive(false);
            buildingScript.isUsed = false;
        }
    }



}
