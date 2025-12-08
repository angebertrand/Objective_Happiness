using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StoneScript : MonoBehaviour
{
    public GameManagerScript gameManagerScript;
    public bool isSomeoneWorkingOnIt = false;
    public float percentUntilStone = 0f;
    public float percentOnStoneRate = 20f;
    public CharacterScript currentCharacter;
    public bool isMining = false;
    public ProgressBarScript progressBar;
    public GameObject canvasProgress;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        gameManagerScript = FindObjectOfType<GameManagerScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMining && currentCharacter != null)
        {
            canvasProgress.SetActive(true);
            // Increase learning progress over time
            percentUntilStone += percentOnStoneRate * Time.deltaTime;
            progressBar.UpdateProgress(percentUntilStone / 100);
            //Debug.Log("Progress: " + learningProgress + "%");

            // Check if learning is complete
            if (percentUntilStone >= 100f)
            {
                
                percentUntilStone = 0f;
                gameManagerScript.nStone += 1;
                progressBar.UpdateProgress(percentUntilStone / 100);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character") && !isMining)
        {
            Debug.Log("AA");
            currentCharacter = other.GetComponent<CharacterScript>();
            
            if (currentCharacter.NextBuilding == this.gameObject)
            {
                isMining = true;
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
                

        }

    }



}
