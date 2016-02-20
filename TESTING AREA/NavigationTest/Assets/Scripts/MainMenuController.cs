using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
    {
        //if(Input.GetButtonDown("joystickbutton7"))
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            SceneManager.LoadScene("MainScene");
        }
	}
}
