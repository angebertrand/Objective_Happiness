using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapScript : MonoBehaviour
{
    public GameObject[] gameObjects;
    // Start is called before the first frame update
    void Awake()
    {
        gameObjects = GameObject.FindGameObjectsWithTag("Tile");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
