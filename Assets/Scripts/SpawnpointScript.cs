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
    public bool firstSpawn = false;
    // Start is called before the first frame update
    void Awake()
    {
        Instantiate(Farmer, transform.position, Farmer.transform.rotation);
        Instantiate(Miner, transform.position, Miner.transform.rotation);
        Instantiate(Woodsman, transform.position, Woodsman.transform.rotation);
        Instantiate(Mason, transform.position, Mason.transform.rotation);
        firstSpawn = true;
    }

    // Update is called once per frame
    public void InstanciateWanderer(int dayCount)
    {
        if (dayCount > 1)
        {
            Instantiate(Wanderer, this.transform.position, Wanderer.transform.rotation);
            
        }
    }
}
