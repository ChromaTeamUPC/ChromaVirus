using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CreditsManager : MonoBehaviour {

    public Text text;
    public Slider slider;

	// Update is called once per frame
	void Update () {

        text.color = Color.Lerp(Color.black, Color.red, Time.time * 0.1f);

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ChangeColor()
    {
        text.color = Color.Lerp(Color.black, Color.red, Time.time * 0.1f);
    }
}
