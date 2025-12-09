using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointScript : MonoBehaviour
{
    //Characters
    public GameObject Farmer;
    public GameObject Mason;
    public GameObject Miner;
    public GameObject Woodsman;
    public GameObject Wanderer;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Farmer, transform.position, Farmer.transform.rotation);
        Instantiate(Mason, transform.position, Mason.transform.rotation);
        Instantiate(Miner, transform.position, Miner.transform.rotation);
        Instantiate(Woodsman, transform.position, Woodsman.transform.rotation);
    }

    // Update is called once per frame
    public void InstanciateWanderer()
    {
        Instantiate(Wanderer, transform.position, Wanderer.transform.rotation);
    }
}
