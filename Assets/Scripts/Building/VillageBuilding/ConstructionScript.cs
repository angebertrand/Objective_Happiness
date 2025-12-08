using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstructionScript : MonoBehaviour
{
    private float buildingTimerTemp = 0f;
    public bool buildIsFinished = false;
    public GameObject futureBuilding;
    private Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<MeshFilter>().mesh = futureBuilding.GetComponent<MeshFilter>().sharedMesh;
        position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Temporary autobuild system
        buildingTimerTemp += 1f * Time.deltaTime;
        if (buildingTimerTemp >= 5f)
        {
            Instantiate(futureBuilding,position,GetComponent<Transform>().rotation);
            Destroy(this.gameObject);
        }
        
        if (buildIsFinished)
        {

        }
    }
}
