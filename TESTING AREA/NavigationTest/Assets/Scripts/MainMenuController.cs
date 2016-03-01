using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour {

    public GameObject help;
    private bool helpOpen = false;
    
	// Update is called once per frame
	void Update ()
    {
        //if(Input.GetButtonDown("joystickbutton7"))
        if (helpOpen && Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            help.SetActive(false);
            helpOpen = false;
        }
	}

    public void OnClickStart()
    {
        SceneManager.LoadScene("MainScene");
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
