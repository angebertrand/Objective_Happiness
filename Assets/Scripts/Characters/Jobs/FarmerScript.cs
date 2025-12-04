using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerScript : CharacterScript
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isWandering)
        {

            if (!IsWalking())
            {
                Wandering();
            }

        }

    }
}
