using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

    private enum MainMenuState
    {
        FadingIn,
        Idle,
        ShowHelp,
        FadingOut
    }
    private MainMenuState currentState;

    public FadeSceneScript fadeScript;

    public Button playBtn;
    public Button helpBtn;
    public Button creditsBtn;
    public Button exitBtn; 

    public GameObject help;
    private bool helpOpen = false;
    private AsyncOperation loadLevel;

    void Start()
    {
        DisableButtons();
        loadLevel = SceneManager.LoadSceneAsync("Level01");
        loadLevel.allowSceneActivation = false;
        currentState = MainMenuState.FadingIn;
        fadeScript.StartFadingToClear();
    }

    // Update is called once per frame
    void Update()
    {
        if (helpOpen && Input.GetButtonDown("Back"))
        {
            help.SetActive(false);
            helpOpen = false;
        }

        switch (currentState)
        {
            case MainMenuState.FadingIn:
                if (!fadeScript.FadingToClear)
                {
                    EnableButtons();
                    currentState = MainMenuState.Idle;
                }
                break;
        }
    }

    private void EnableButtons()
    {
        playBtn.interactable = true;
        helpBtn.interactable = true;
        creditsBtn.interactable = true;
        exitBtn.interactable = true;
        playBtn.Select();
    }

    private void DisableButtons()
    {
        playBtn.interactable = false;
        helpBtn.interactable = false;
        creditsBtn.interactable = false;
        exitBtn.interactable = false;
    }

    public void OnClickStart()
    {
        //TODO: Select players
        loadLevel.allowSceneActivation = true;
    }

    public void OnClickHelp()
    {
        help.SetActive(true);
        helpOpen = true;
    }

    public void OnClickCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
