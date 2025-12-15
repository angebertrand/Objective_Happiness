using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarScript : MonoBehaviour
{
    public Image progressBar;
    private void Start()
    {
        transform.rotation = Quaternion.identity;
    }
    public void UpdateProgress(float number)
    {
        progressBar.fillAmount = number;
    }
}
