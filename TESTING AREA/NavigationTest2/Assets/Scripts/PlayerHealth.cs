using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public int health = 100;
    public Text healthTxt;
    public Text win;
    public Text gameOver;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        healthTxt.text = "Life: " + health;

        if(health <= 0)
        {
            gameOver.gameObject.SetActive(true);
            StartCoroutine("GoToMainMenu");
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DeathZone")
        {
            health = 0;
        }
        else if (other.tag == "WinZone")
        {
            win.gameObject.SetActive(true);
            StartCoroutine("GoToMainMenu");
        }
    }

    IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene(0);
    }
}
