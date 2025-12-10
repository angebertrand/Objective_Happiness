using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayCountScript : MonoBehaviour
{
    // Update is called once per frame
    public void UpdateDisplay(int dayCount)
    {
        GetComponent<TMP_Text>().text = dayCount.ToString();
    }
}
