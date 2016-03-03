using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Text healthTxt;
    public Text win;
    public Text gameOver;

    // Use this for initialization
    void Start () {
        mng.eventManager.StartListening(EventManager.EventType.PLAYER_SPAWNED, PlayerSpawned);
        mng.eventManager.StartListening(EventManager.EventType.PLAYER_DAMAGED, PlayerDamaged);
        mng.eventManager.StartListening(EventManager.EventType.PLAYER_DIED, PlayerDied);
    }

    void PlayerSpawned(EventInfo eventInfo)
    {
        PlayerDamagedEventInfo info = (PlayerDamagedEventInfo)eventInfo;
        healthTxt.text = "Life: " + info.currentHealth;
    }

    void PlayerDamaged(EventInfo eventInfo)
    {
        PlayerDamagedEventInfo info = (PlayerDamagedEventInfo)eventInfo;
        healthTxt.text = "Life: " + info.currentHealth;
    }

    void PlayerDied(EventInfo eventInfo)
    {
        gameOver.gameObject.SetActive(true);
        StartCoroutine("GoToMainMenu");
    }

    void PlayerWin()
    {
        win.gameObject.SetActive(true);
        StartCoroutine("GoToCredits");
    }

    IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator GoToCredits()
    {
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("GameCredits");
    }
}
