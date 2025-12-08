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
        if (futureBuilding.GetComponent<Building>().bigBuilding)
        {
            transform.position += new Vector3(0, 0, 10.90681f);
        }
        this.transform.rotation = futureBuilding.transform.rotation;
        position = this.transform.position;
        this.transform.localScale = futureBuilding.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Temporary autobuild system
        buildingTimerTemp += 1f * Time.deltaTime;
        if (buildingTimerTemp >= 5f)
        {
            Instantiate(futureBuilding,position,this.transform.rotation);
            Destroy(this.gameObject);
        }
        
        if (buildIsFinished)
        {

        }
    }
}
