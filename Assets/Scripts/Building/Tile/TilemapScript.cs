using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapScript : MonoBehaviour
{
    public int xGridSize = 0;
    public int yGridSize = 0;
    public GameObject[] gameObjects;
    public GameObject Tile;
    private GameObject currentTile;
    // Start is called before the first frame update
    void Awake()
    {
        for (int x = 0; x < xGridSize; x++)
        {
            for (int y = 0; y < yGridSize; y++)
            {
                currentTile = Instantiate(Tile, Vector3.zero, Tile.transform.rotation, this.transform);
                currentTile.GetComponent<TileSettings>().xPos = x;
                currentTile.GetComponent<TileSettings>().yPos = y;
                currentTile.GetComponent<TileSettings>().enabled = true;
            }
        }
        gameObjects = GameObject.FindGameObjectsWithTag("Tile");
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
