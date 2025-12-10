using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningMessagesScript : MonoBehaviour
{
    public List<GameObject> warningsMessages = new List<GameObject>();
    private bool coroutineOn = false;
    // Start is called before the first frame update
    void Awake()
    {
        warningsMessages.Add(GameObject.Find("NoAvailableMasonWarning"));
        warningsMessages.Add(GameObject.Find("NotEnoughRessourcesWarning"));
        warningsMessages.Add(GameObject.Find("NoSpaceWarning"));
        warningsMessages[0].SetActive(false);
        warningsMessages[1].SetActive(false);
        warningsMessages[2].SetActive(false);
    }

    public IEnumerator warningCoroutine(int indexOfMessage)
    {
        if (coroutineOn)
        {
            foreach (GameObject message in warningsMessages)
            {
                if (warningsMessages.IndexOf(message) != indexOfMessage)
                {
                    StopCoroutine(warningCoroutine(warningsMessages.IndexOf(message)));
                }
            }
        }
        coroutineOn = true;
        warningsMessages[indexOfMessage].SetActive(true);
        yield return new WaitForSeconds(1f);
        warningsMessages[indexOfMessage].SetActive(false);
        coroutineOn = false;
    }
}
