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

        if (mr == null)
        {
            Debug.LogError("Aucun MeshRenderer trouvé sur l'hexagone !");
            return 1f;
        }

        // taille réelle du mesh rendu
        Vector3 size = mr.bounds.size;

        // Pour un hex pointy-top :
        // largeur = radius * √3
        // radius = largeur / √3
        float width = size.x;

        float radius = width / Mathf.Sqrt(3f);

        return radius;
    }

    Vector3 PlacerHexaDansWorld(int x, int y, float size)
    {
        float worldX = size * (1.5f * x); 
        float worldZ = size * (Mathf.Sqrt(3f) * (y + x * 0.5f)); 

        return new Vector3(worldX, 0f, worldZ); 
    }

    public void ressourceIsExtracted()
    {
        ressourceAmount--;
    }
}
