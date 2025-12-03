using System;
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

    [Header("Réglage de la tile")]
    public int xPos;
    public int yPos;

    private float hexRadius;

    void Start()
    {
        // Calcule automatiquement le rayon du mesh attaché
        hexRadius = DetectHexRadius();

        transform.position = PlacerHexaDansWorld(xPos, yPos, DetectHexRadius());
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
}
