using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //Value to change transition parameter
    public float transitionQuality = 0.05f;
    public float transitionSpeed = 0.5f;
    private float nombreDeRepetition;

    //Object
    private GameObject forground;
    private Image forgroundImage;
    private Color color;
    // Start is called before the first frame update
    void Start()
    {
        nombreDeRepetition = transitionSpeed / transitionQuality;
        forground = GameObject.Find("ForgroundLoading");
        forgroundImage = forground.GetComponent<Image>();
        forground.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        StartCoroutine(Forgroundfade());
    }
    private IEnumerator Forgroundfade()
    {
       forground.SetActive(true);
        for (int i = 0; i < nombreDeRepetition; i++)
        {
            color.a = color.a + 1/nombreDeRepetition;
            forgroundImage.color = color;
            yield return new WaitForSeconds(transitionSpeed/nombreDeRepetition);
        }
        SceneManager.LoadScene("GameScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
