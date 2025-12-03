using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : MonoBehaviour
{
    public PlayerScript PlayerScript;
    public GameObject PrevisualisationBuild;
    public string futureBuildType = "farm";
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Mouse_Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (PlayerScript.isBuilding == true)
        {
            PrevisualisationBuild.transform.position = new Vector3 (Mouse_Position.x, 2, Mouse_Position.z);
        }
    }
}
