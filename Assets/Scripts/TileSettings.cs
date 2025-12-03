using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSettings : MonoBehaviour
{
    public bool isBuildableOn = false;
    public string tileType;
    private int ressourceAmount = 5;
    public int xPos;
    public int yPos;

    // Update is called once per frame
    void Awake()
    {
    }

    public void ressourceIsExtracted()
    {
        ressourceAmount--;
    }
}
