using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapScript : MonoBehaviour
{
    public int xGridSize = 0;
    public int yGridSize = 0;
    public GameObject[] gameObjects;
    public GameObject Tile;
    public GameObject spawnpoint;
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
                if (x == xGridSize/2 && y == yGridSize/2)
                {
                    currentTile.GetComponent<TileSettings>().tileType = 0;
                    currentTile.GetComponent<TileSettings>().isBuildableOn = false;
                    currentTile.name = "spawnpointTile";
                }
                else
                {
                    int seed = Random.Range(0, 101);
                    if (seed < 10)
                    {
                        currentTile.GetComponent<TileSettings>().tileType = 1;
                    }
                    else if (seed >= 10 && seed < 20)
                    {
                        currentTile.GetComponent<TileSettings>().tileType = 2;
                    }
                    else if (seed >= 20 && seed < 30)
                    {
                        currentTile.GetComponent<TileSettings>().tileType = 3;
                    }
                }
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
