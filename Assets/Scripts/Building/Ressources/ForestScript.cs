using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ForestScript : MonoBehaviour
{
    public GameManagerScript gameManagerScript;
    public bool isSomeoneWorkingOnIt = false;
    public float percentUntilWood = 0f;
    public float percentOnWoodRate = 20f;
    public CharacterScript currentCharacter;
    public bool isMining = false;
    public ProgressBarScript progressBar;
    public GameObject canvasProgress;

    public Building buildingScript;

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
            percentUntilWood += percentOnWoodRate* Time.deltaTime;
            progressBar.UpdateProgress(percentUntilWood / 100);
            //Debug.Log("Progress: " + learningProgress + "%");

            // Check if learning is complete
            if (percentUntilWood >= 100f)
            {
                
                percentUntilWood = 0f;
                gameManagerScript.nWood += 1;
                progressBar.UpdateProgress(percentUntilWood / 100);
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
            && other.gameObject.GetComponent<CharacterScript>() == currentCharacter
            )
        {

            isMining = false;
            canvasProgress.SetActive(false);
            buildingScript.isUsed = false;

        }
    }



}
