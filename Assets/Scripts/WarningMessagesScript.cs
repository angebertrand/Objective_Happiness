using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningMessagesScript : MonoBehaviour
{
    public GameObject NoAvailableMason;
    public GameObject NotEnoughRessources;
    public GameObject NoSpace;
    // Start is called before the first frame update
    void Awake()
    {
        NoAvailableMason = GameObject.Find("NoAvailableMasonWarning");
        NotEnoughRessources = GameObject.Find("NotEnoughRessourcesWarning");
        NoSpace = GameObject.Find("NoSpaceWarning");
        NoAvailableMason.SetActive(false);
        NotEnoughRessources.SetActive(false);
        NoSpace.SetActive(false);
    }

    public IEnumerator warningCoroutine(GameObject Warning)
    {
        Warning.SetActive(true);
        yield return new WaitForSeconds(3f);
        Warning.SetActive(false);
        yield break;
    }
}
