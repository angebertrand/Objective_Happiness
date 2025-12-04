
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class CharacterScript : MonoBehaviour
{
    // DEBUG //
    
    //private Renderer Renderer;
    //public Color Color = Color.white;
    
    ///////////////
    


    public int sleepiness;
    public int age;
    public bool hasEaten;
    public bool hasAJob;
    public bool enoughFood;
    public bool isWandering;
    public bool isWorking;

    public NavMeshAgent agent;

    public GameObject Farmer;
    public GameObject Wanderer;
    public GameObject Miner;
    public GameObject Mason;
    public GameObject Woodsman;

    GameObject newCharacter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        

    }

    // Update is called once per frame
    void Update()
    {

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

    }

    public void GoToWork()
    {

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
        newChara.transform.position = oldChara.transform.position;
        newChara.transform.rotation = oldChara.transform.rotation;


        // Destroy old "job"
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

}
