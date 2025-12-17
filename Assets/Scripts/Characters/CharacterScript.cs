using System.Collections;
using TMPro;
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
    public int age = 0;
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

    public Coroutine wanderRoutine;

    public BuildingManager buildingManager;
    public bool wanderingCoroutineRunning = false;

    GameObject newCharacter;

    protected GameObject currentTargetObject = null;
    protected Vector3 currentTargetPosition = Vector3.zero;
    protected bool hasTargetPosition = false;
    protected float positionTolerance = 0.5f;

    protected Vector3 wanderingTarget;
    protected bool wanderingDestinationSet = false;

    public bool isBuildingSomething = false;

    public Vector3 currentWanderTarget;
    public bool hasWanderTarget = false;
    public float wanderRadius = 30f;
    public float arriveThreshold = 0.5f;

    public Animator animator;

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


    public void StopMovement()
    {

        currentTargetObject = null;
        hasTargetPosition = false;


        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }


    public void MoveTo(GameObject target)
    {
        if (agent == null)
            return;


        if (!agent.enabled)
            return;

        currentTargetObject = null;
        hasTargetPosition = true;
        currentTargetPosition = target.transform.position;

        agent.isStopped = false;
        agent.ResetPath();

        agent.SetDestination(currentTargetPosition);
        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            animator.SetTrigger("Jump");
        }
    }

    // --- Central method for sending the agent to a position ---
    public void MoveTo(Vector3 pos)
    {
        if (agent == null) return;

        Debug.Log($"[MoveTo] {name} requested MoveTo position {pos}");

        // If we already have a very close target → ignore
        if (hasTargetPosition && Vector3.Distance(currentTargetPosition, pos) <= positionTolerance)
            return;

        // Cancel invokes that could redirect movement
        CancelAllMovementInvokes();

        // Find a valid NavMesh position
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 3.0f, NavMesh.AllAreas))
        {
            Vector3 validPos = hit.position;

            currentTargetObject = null;
            hasTargetPosition = true;
            currentTargetPosition = validPos;

            agent.isStopped = false;
            agent.ResetPath();

            bool ok = agent.SetDestination(validPos);
            if (agent.velocity.sqrMagnitude > 0.01f)
            {
                animator.SetTrigger("Jump");
            }
            Debug.Log($"[MoveTo] {name} -> validated position: {validPos}. SetDestination returned: {ok}");
        }
        else
        {
            Debug.LogWarning($"[MoveTo] {name}: could not find NavMesh point for pos {pos}");
        }
    }


    private void CancelAllMovementInvokes()
    {
        // Cancels internal invokes known to influence movement (add more if needed)
        CancelInvoke(nameof(TryGoToWorkLater));
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
        // If the agent is still calculating a path → consider moving
        if (agent.pathPending)
            return true;

        // If speed > 0 → walking
        if (agent.velocity.sqrMagnitude > 0.01f)
            return true;

        // If remaining distance small → not walking
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

            // Retry later (via non-spammy Invoke)
            Invoke(nameof(TryGoToWorkLater), 1f);
            return;
        }

        // Important: cancel any invoke that could redirect elsewhere
        CancelInvoke(nameof(TryGoToWorkLater));

        isWandering = false;
        b.isUsed = true;
        JobBuilding = b.gameObject;

        isWorking = true;
        NextBuilding = b.gameObject;
        MoveTo(b.gameObject);

        Debug.Log(name + "OKEYYY");
    }

    private void TryGoToWorkLater()
    {
        if (!isLearning && manager != null && manager.day)
            GoToWork();
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
            manager.UnregisterCharacter(oldChara);
            manager.RegisterCharacter(newChara);
        }

        // Destroy old job
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

        // Make the name look at the camera:
        nameText.gameObject.transform.LookAt(transform.position + cameraMain.transform.rotation * Vector3.forward, cameraMain.transform.rotation * Vector3.up);
    }

    public void StopBeingHover()
    {
        nameText.gameObject.SetActive(false);
    }


    public void StartWandering()
    {
        if (wanderRoutine != null)
            StopCoroutine(wanderRoutine);
        animator.SetTrigger("Jump");
        wanderRoutine = StartCoroutine(WanderingLoop());
    }

    public void StopWandering()
    {
        isWandering = false;

        if (wanderRoutine != null)
            StopCoroutine(wanderRoutine);

        wanderRoutine = null;
    }



    private IEnumerator WanderingLoop()
    {
        isWandering = true;
        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            animator.SetTrigger("Jump");
        }

        while (isWandering)
        {
            Vector3 target = RandomNavmeshLocation(30f);
            agent.SetDestination(target);

            // Wait until the character has really arrived
            while (isWandering && !HasArrived(target))
                yield return null;

            // Small pause
            yield return new WaitForSeconds(0.3f);
        }
    }

    private bool HasArrived(Vector3 target)
    {
        if (!agent.hasPath) return false; // Avoid false positives
        if (agent.pathPending) return false;
        if (agent.remainingDistance > agent.stoppingDistance + 0.2f) return false;

        return true;
    }


    public void BeSleepy()
    {
        sleepiness = true;
    }

    public void EndOfTheDay()
    {
        age += 1;
        if (age == 7)
        {
            Die();
        }

        sleepiness = true;
        Eat();

        if (currentJob != "Wander")
        {
            Building house = buildingManager.GetFreeBuilding("House");

            if (house == null)
            {
                agent.isStopped = false;
                agent.ResetPath();

                StartWandering();
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
