using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningMessagesScript : MonoBehaviour
{
    public GameObject NoAvailableMason;
    public GameObject NotEnoughRessources;
    // Start is called before the first frame update
    void Awake()
    {
        NoAvailableMason = GameObject.Find("NoAvailableMasonWarning");
        NotEnoughRessources = GameObject.Find("NotEnoughRessourcesWarning");
    }

    public void NoMasonWarning()
    {

    }

    IEnumerator waitCoroutine(GameObject Warning)
    {
        Warning.SetActive(true);
        yield return new WaitForSeconds(3f);
        Warning.SetActive(false);
    }
}
