using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstructionScript : MonoBehaviour
{
    
    public bool buildIsFinished = false;
    public GameObject futureBuilding;
    private Vector3 position;
    public MasonScript masonScript;
    private bool isBuilding = false;
    public ProgressBarScript progressBar;
    public GameObject canvasProgress;
    public float percentUntilConstr = 0f;
    public float percentOnConstrRate = 20f;
    private GameManagerScript gameManager;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<MeshFilter>().mesh = futureBuilding.GetComponent<MeshFilter>().sharedMesh;
        if (futureBuilding.GetComponent<Building>().bigBuilding)
        {
            transform.position += new Vector3(0, 0, 10.90681f);
        }
        this.transform.rotation = futureBuilding.transform.rotation;
        position = this.transform.position;
        this.transform.localScale = futureBuilding.transform.localScale;
    }

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();

        MasonScript availableMason = GetAvailableMason();

        if (availableMason != null)
        {
            // Il part vers le chantier
            availableMason.StartBuilding(this.gameObject);
            availableMason.isWorking = true;

            masonScript = availableMason;

            Debug.Log("Un maçon a été assigné à " + this.name + ". Il arrive !");
        }
        else
        {
            Debug.Log("⚠ Aucun maçon disponible !");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isBuilding && masonScript != null)
        {
            canvasProgress.SetActive(true);
            // Increase learning progress over time
            percentUntilConstr += percentOnConstrRate* Time.deltaTime;
            progressBar.UpdateProgress(percentUntilConstr / 100);
            //Debug.Log("Progress: " + learningProgress + "%");

            // Check if learning is complete
            if (percentUntilConstr >= 100f)
            {
                masonScript.StartWandering();
                masonScript.isWorking = false;
                percentUntilConstr = 0f;
                masonScript.isBuildingSomething = false;
                progressBar.UpdateProgress(percentUntilConstr / 100);
                Instantiate(futureBuilding, position, this.transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }

    private MasonScript GetAvailableMason()
    {
        foreach (CharacterScript c in gameManager.characters)
        {
            MasonScript mason = c as MasonScript;

            if (mason != null)
            {
                if (!mason.isWorking && !mason.isLearning && mason.isJobless == false && mason.isBuildingSomething == false)
                {
                    return mason;
                }
            }
        }

        return null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mason"))
        {
            masonScript = other.GetComponent<MasonScript>();

            if (masonScript != null && masonScript.JobBuilding == this.gameObject)
            {
                isBuilding = true;
                masonScript.isWorking = true;
                masonScript.isWandering = false;
                masonScript.agent.isStopped = false;
                masonScript.agent.ResetPath();
            }
        }
    }
}
