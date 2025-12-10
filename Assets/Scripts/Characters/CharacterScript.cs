
using JetBrains.Annotations;
using TMPro;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class CharacterScript : MonoBehaviour
{
    // DEBUG //

    //private Renderer Renderer;
    //public Color Color = Color.white;

    ///////////////

    public Sprite icon;

    public bool sleepiness;
    public int age;
    public bool hasEaten;
    public bool hasAJob;
    public bool enoughFood;
    public bool isWandering;
    public bool isWorking;
    public int ID = -1;
    public bool canLearn;
    public string nextJob;
    public bool isSleeping = false;
    public bool isJobless = false;

    public int isHappy;

    public string currentJob;
    public bool isLearning = false;
    public bool isBeingHover = false;

    public Camera cameraMain;
    public TMP_Text nameText;

    public NavMeshAgent agent;
    public GameManagerScript manager;

    public GameObject Farmer;
    public GameObject Wanderer;
    public GameObject Miner;
    public GameObject Mason;
    public GameObject Woodsman;
    public GameObject NextBuilding;
    public GameObject JobBuilding;

    public BuildingManager buildingManager;
    

    GameObject newCharacter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Register()
    {
        if (this.ID == -1)
        {
            manager = FindObjectOfType<GameManagerScript>();
            if (manager != null)
            {
                manager.RegisterCharacter(this);
            }
        }

    }

    // Returns TRUE if the agent is currently moving
    public bool IsWalking()
    {
        
        // If the agent is still calculating a path, consider it as walking
        if (agent.pathPending)
            return true;

        // If the speed is more than zero → walking
        if (agent.velocity.sqrMagnitude > 0.01f)
            return true;

        // If the remaining distance is very small → not walking
        if (agent.remainingDistance <= agent.stoppingDistance)
            return false;

        
        return false;
    }

    public void Die()
    {
        manager.UnregisterCharacter(this);
        Destroy(gameObject);
    }

    public void GoToWork()
    {
        
        if (isLearning || manager.day == false) return;

        Building b = buildingManager.GetBuildingForJob(currentJob, this);


        if (b == null)
        {
            
            isWorking = false;
            isWandering = true;
            
            Invoke("GoToWork", 1f);

            return;
        }
        isWandering = false;
        b.isUsed = true;
        JobBuilding = b.gameObject;
        
        agent.isStopped = false;
        agent.ResetPath();

        agent.SetDestination(b.transform.position);
        isWorking = true;
        NextBuilding = b.gameObject;
    }

    public void FinishWork()
    {
        isWorking = false;
        buildingManager.ReleaseBuilding(JobBuilding.GetComponent<Building>());
    }


    public void ChangeCharacter(GameObject prefab)
    {
        
        CharacterScript oldChara = this;

        
        GameObject newObj = Instantiate(prefab);

        
        CharacterScript newChara = newObj.GetComponent<CharacterScript>();


        // Copy data
        newChara.sleepiness = oldChara.sleepiness;
        newChara.age = oldChara.age;
        newChara.hasEaten = oldChara.hasEaten;
        newChara.hasAJob = oldChara.hasAJob;
        newChara.enoughFood = oldChara.enoughFood;
        newChara.isWandering = oldChara.isWandering;
        newChara.isWorking = oldChara.isWorking;
        newChara.ID = oldChara.ID;
        newChara.transform.position = oldChara.transform.position;
        newChara.transform.rotation = oldChara.transform.rotation;
        newChara.isHappy = oldChara.isHappy;
        newChara.canLearn = false;
        newChara.name = oldChara.name;


        newChara.GoToWork();

        // Replace old character in the manager
        if (manager != null)
        {
            manager.UnregisterCharacter(oldChara);   // Remove old character
            manager.RegisterCharacter(newChara);    // Register new character with same ID
        }

        // Destroy old "job"
        CancelInvoke();
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public Vector3 RandomNavmeshLocation(float radius) // Random point on the NavMesh
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void Wandering()
    {
        agent.SetDestination(RandomNavmeshLocation(30f));
    }

    public void GoToBuilding(GameObject Building)
    {
        NextBuilding = Building;
        agent.SetDestination(Building.transform.position);
    }

    public void GoToSchool(GameObject Building, string NextJob)
    {
        
        if (NextJob != currentJob)
        {
            isJobless = true;
            if (isWorking)
            {
                FinishWork();
            }
            NextBuilding = Building;
            JobBuilding = Building;
            this.canLearn = true;
            this.nextJob = NextJob;
            this.isLearning = true;
            agent.SetDestination(Building.transform.position);
            
        } 
        
    }

    public void BeingHover()
    {
        nameText.text = this.name;
        nameText.gameObject.SetActive(true);
        // faire que le texte regarde la camera :
        nameText.gameObject.transform.LookAt(transform.position + cameraMain.transform.rotation * Vector3.forward, cameraMain.transform.rotation * Vector3.up);

    }

    public void StopBeingHover()
    {
        nameText.gameObject.SetActive(false);
    }

    public void BeSleepy()
    {
        sleepiness = true;
    }

    public void EndOfTheDay()
    {
        
        sleepiness = true;
        Eat();

        

        if (currentJob != "Wander")
        {
            
            Building house = buildingManager.GetFreeBuilding("House");

            if (house == null)
            { 
                isWandering = true;
                isHappy = -1;
                return;
            }

            agent.isStopped = false;     
            agent.ResetPath();
            HouseScript houseScript = house.gameObject.GetComponent<HouseScript>();
            if (houseScript == null) return;
            houseScript.currentCharacter = this;
            
            
            houseScript.currentCharacter = this;
            agent.SetDestination(house.transform.position);
            isWorking = false;
            isWandering = false;
            isLearning = false;
            NextBuilding = house.gameObject;
            isHappy = 1;
                
            

        }
        else
        {
            isHappy = 0;
            isSleeping = true;
        }

    }

    public void Eat()
    {  
        if (!manager.ConsumeFood(1))
        {
            Die();
        }
    }
}
