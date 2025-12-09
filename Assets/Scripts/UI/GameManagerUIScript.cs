using TMPro;
using UnityEngine;

public class GameManagerUIScript : MonoBehaviour
{
    public TMP_Text popText;
    public TMP_Text woodText;
    public TMP_Text foodText;
    public TMP_Text stoneText;
    public GameManagerScript gameManagerScript;
    public ProgressBarScript happinessBar;


    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        popText.text = gameManagerScript.nPopulation.ToString();
        woodText.text = gameManagerScript.nWood.ToString();
        foodText.text = gameManagerScript.nFood.ToString();
        stoneText.text = gameManagerScript.nStone.ToString();
    }

    public void happinessReset(float newHappiness)
    {
        happinessBar.UpdateProgress(newHappiness);
    }
}
