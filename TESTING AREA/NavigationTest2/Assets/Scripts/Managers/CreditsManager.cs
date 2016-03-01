using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsManager : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
