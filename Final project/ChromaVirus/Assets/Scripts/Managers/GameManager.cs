using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    private AsyncOperation loadLevel;

    private bool paused = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetButtonDown("Ok"))
        {
            paused = !paused;

            if (paused)
                Time.timeScale = 0.000000000001f;
            else
                Time.timeScale = 1f;
        }
	}

    public void StartPreloadingFirstLevel()
    {
        if (loadLevel == null)
        {
            loadLevel = SceneManager.LoadSceneAsync("Level01");
            loadLevel.allowSceneActivation = false;
        }
    }


    public void StartNewGame(int numPlayers)
    {
        InitPlayers(numPlayers);

        loadLevel.allowSceneActivation = true;
    }

    public void InitPlayers(int numPlayers)
    {
        rsc.gameInfo.numberOfPlayers = numPlayers;
        rsc.gameInfo.player1Controller.ResetPlayer();
        rsc.gameInfo.player1Controller.Active = true;
        rsc.gameInfo.player2Controller.ResetPlayer();
        if (numPlayers == 2)
        {
            rsc.gameInfo.player2Controller.Active = true;
            rsc.gameInfo.player2.SetActive(true);
        }
        else {
            rsc.gameInfo.player2Controller.Active = false;
            rsc.gameInfo.player2.SetActive(false);
        }
    }
}
