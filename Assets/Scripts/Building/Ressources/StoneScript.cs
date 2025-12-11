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


    private IEnumerator CheckMiningState()
    {
        while (isMining)
        {
            // Chara exists ?
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
                currentCharacter.animator.ResetTrigger("Jump");
                currentCharacter.animator.SetTrigger("ToolUse");
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
