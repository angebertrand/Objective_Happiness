using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool isBuilding = false;
    public GameObject buildInterface;
    public DefaultMode defaultMode;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        defaultMode = GetComponent<DefaultMode>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();

        if (isBuilding && !GetComponentInChildren<BuildMode>().enabled)
        {
            buildInterface.SetActive(true);
        }
    }
    private void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuilding = true;
            defaultMode.enabled = false;
        }
    }
}
