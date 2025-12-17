using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerUIScript : MonoBehaviour
{
    public TMP_Text popText;
    public TMP_Text woodText;
    public TMP_Text foodText;
    public TMP_Text stoneText;
    public TMP_Text happinessText;
    public Image timeImage;
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
        happinessText.text = "Happiness : " + gameManagerScript.Joy.ToString() + " / 100";

        if (gameManagerScript.day)
        {
            float currentZ = timeImage.rectTransform.localEulerAngles.z;
            if (currentZ == 0f)
            {
                timeImage.rectTransform.localEulerAngles = new Vector3(0, 0, 90f);
            }
            if (currentZ < 270f)
            {
                timeImage.rectTransform.Rotate(0, 0, 180f / 65f * Time.deltaTime);
            }
        }
        else
        {
            timeImage.rectTransform.localEulerAngles = new Vector3(0, 0,0);

        }
    }

    public void happinessReset(float newHappiness)
    {
        happinessBar.UpdateProgress(newHappiness);
    }
}
