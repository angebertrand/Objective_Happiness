using System;
using UnityEngine;
using UnityEngine.UI;

public class SchoolScript : MonoBehaviour
{
    public CharacterScript currentCharacter;
    public GameObject NextJob;
    public float learningProgress = 0f; // Progress from 0 to 100
    public float learningRate = 20f;    // % per second

    public AudioSource wrongSFX;
    public AudioSource goodSFX;

    public ProgressBarScript progressBar;
    public GameObject canvasProgress;
    public GameObject canvasUI;
    public bool isSomeoneLearning;

    private bool isLearning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            Debug.Log("AA");
            currentCharacter = other.GetComponent<CharacterScript>();
            Debug.Log(currentCharacter.NextBuilding);
            if (currentCharacter.canLearn && currentCharacter.NextBuilding == this.gameObject)
            {
                
                isLearning = true;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            
            isLearning = false;
            canvasProgress.SetActive(false);
        }
    }

    private void Update()
    {
        if (isLearning && currentCharacter != null)
        {
            canvasProgress.SetActive(true);
            // Increase learning progress over time
            learningProgress += learningRate * Time.deltaTime;
            progressBar.UpdateProgress(learningProgress/100);
            //Debug.Log("Progress: " + learningProgress + "%");

            // Check if learning is complete
            if (learningProgress >= 100f)
            {
                FinishLearning();
                learningProgress = 0f;
                isLearning = false;
                progressBar.UpdateProgress(learningProgress/100);
            }
        }
    }

    private void FinishLearning()
    {
        isSomeoneLearning = false;
        canvasProgress.SetActive(false);
        Debug.Log(currentCharacter.name + " has finished learning!");
        // Example: change character's job
        switch (currentCharacter.nextJob)
        {
            case "Farmer":
                currentCharacter.ChangeCharacter(currentCharacter.Farmer);
                break;
            case "Miner":
                currentCharacter.ChangeCharacter(currentCharacter.Miner);
                break;
            case "Woods":
                currentCharacter.ChangeCharacter(currentCharacter.Woodsman);
                break;
            case "Mason":
                currentCharacter.ChangeCharacter(currentCharacter.Mason);
                break;
        }
        
    }
    
    public void ShowSchoolCanva()
    {
        canvasUI.SetActive(true);
    }
    public void HideSchoolCanva()
    {
        canvasUI.SetActive(false);
    }
}
