using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileSettings : MonoBehaviour
{
    public bool isBuildableOn = false;
    public string tileType;
    private int ressourceAmount = 5;

    [Header("Réglage de la tile")]
    public int xPos;
    public int yPos;
    public List<GameObject> sisterTiles;
    private Vector3 position;

    private float hexRadius;

    void Awake()
    {
        // Calcule automatiquement le rayon du mesh attaché
        hexRadius = DetectHexRadius();
        position = PlacerHexaDansWorld(xPos, yPos, DetectHexRadius());
        transform.position = position;
    }

    void Start()
    {
        sisterTiles = SetSisterTiles(xPos, yPos);
    }

    float DetectHexRadius()
    {
        MeshRenderer mr = GetComponentInChildren<MeshRenderer>();

        // taille réelle du mesh rendu
        Vector3 size = mr.bounds.size;

        // Pour un hex flat-top :
        // largeur = radius * 2
        // radius = largeur / 2
        float width = size.x;

        float radius = width / 2;

        return radius;
    }

    Vector3 PlacerHexaDansWorld(int x, int y, float size)
    {
        float worldX = size * 3 * x;
        float worldZ = (size * (float)Math.Sqrt(3f))/2 * y;
        if (y % 2 == 1)
        {
            worldX += size * 1.5f;
        }

        return new Vector3(worldX, 0f, -worldZ); 
    }

    public void ressourceIsExtracted()
    {
        ressourceAmount--;
    }
    private List<GameObject> SetSisterTiles(int XPos, int YPos)
    {
        List<GameObject> sisterTiles = new List<GameObject>();
        foreach (GameObject go in GetComponentInParent<TilemapScript>().gameObjects)
        {
            int goXPos = go.GetComponent<TileSettings>().xPos;
            int goYPos = go.GetComponent<TileSettings>().yPos;
            if (goXPos == XPos)
            {
                for (int i = 1; i < 3; i++)
                {
                    if (goYPos == YPos - i)
                    {
                        sisterTiles.Add(go);
                    }
                }
            }
            int xScan = 0;
            switch (goYPos%2)
            {
                case 0:
                    xScan = 1;
                    break;
                case 1:
                    xScan = -1;
                    break;
            }
            if (goXPos == XPos + xScan && goYPos == YPos - 1)
            {
                sisterTiles.Add(go);
            }
        }
        return (sisterTiles);
    }
}