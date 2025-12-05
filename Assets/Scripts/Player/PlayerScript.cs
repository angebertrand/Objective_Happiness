using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool isBuilding = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();

        if (isBuilding && !GetComponentInChildren<BuildMode>().enabled)
        {
            GetComponentInChildren<BuildMode>().enabled = true; 
        }
    }
    private void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Input pressed!");
            isBuilding = true;
        }
    }
}
