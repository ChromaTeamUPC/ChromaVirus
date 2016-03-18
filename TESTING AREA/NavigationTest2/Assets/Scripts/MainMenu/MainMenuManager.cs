using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

    public GameObject help;
    private bool helpOpen = false;
    private AsyncOperation loadLevel;

    void Start()
    {
        loadLevel = SceneManager.LoadSceneAsync("MainScene");
        loadLevel.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (helpOpen && Input.GetButtonDown("Back"))
        {
            help.SetActive(false);
            helpOpen = false;
        }
    }

    public void OnClickStart()
    {
        //SceneManager.LoadScene("MainScene");
        loadLevel.allowSceneActivation = true;
    }

    public void OnClickHelp()
    {
        help.SetActive(true);
        helpOpen = true;
    }

    public void OnClickCredits()
    {
        SceneManager.LoadScene("GameCredits");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
