using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint; 

    public Material enemyRedMaterial;
    public Material enemyGreenMaterial;
    public Material enemyBlueMaterial;
    public Material enemyYellowMaterial;
    private ObjectPool aracnoBotPool;

    public GameObject player1;
    public GameObject player2;
    // Use this for initialization
    public void Init () {
        mng.eventManager.StartListening(EventManager.EventType.PLAYER_DIED, PlayerDied);
        mng.eventManager.StartListening(EventManager.EventType.COLOR_CHANGED, ColorChanged);

        aracnoBotPool = mng.poolManager.aracnoBotPool;      
    }

    void Start()
    {
        player1 = Instantiate(player1Prefab, player1SpawnPoint.position, Quaternion.identity) as GameObject;
        //Test
        InvokeRepeating("SpawnEnemies", 2f, 3f);
    }

    //Test function
    void ColorChanged(EventInfo eventInfo)
    {
        switch (((ColorEventInfo)eventInfo).newColor)
        {
            case ChromaColor.RED: RenderSettings.skybox.SetColor("_Tint", Color.red); break;
            case ChromaColor.GREEN: RenderSettings.skybox.SetColor("_Tint", Color.green); break;
            case ChromaColor.BLUE: RenderSettings.skybox.SetColor("_Tint", Color.blue); break;
            case ChromaColor.YELLOW: RenderSettings.skybox.SetColor("_Tint", Color.yellow); break;
        }
        
    }

    void SpawnEnemies()
    {
        //Get enemy from pool
        GameObject enemy = aracnoBotPool.GetObject();
        //Set objective goal
        EnemyMovement eMov = enemy.GetComponent<EnemyMovement>();
        eMov.goal = player1.transform;

        //Set spawnpoint
        enemy.transform.position = spawnPoint1.transform.position;
        
        ChromaColor randColor = (ChromaColor)Random.Range((int)ChromaColorInfo.First, (int)ChromaColorInfo.Last + 1);
        enemy.GetComponent<EnemyHealth>().color = randColor;
        Material mat = enemyRedMaterial;
        switch(randColor)
        {
            case ChromaColor.RED: mat = enemyRedMaterial; break;
            case ChromaColor.GREEN: mat = enemyGreenMaterial; break;
            case ChromaColor.BLUE: mat = enemyBlueMaterial; break;
            case ChromaColor.YELLOW: mat = enemyYellowMaterial; break;
        }
        //enemy.GetComponent<Renderer>().material = mat;
        enemy.GetComponentInChildren<Renderer>().material = mat;

        //Activate enemy
        enemy.SetActive(true);
    }

    public void PlayerDied(EventInfo eventInfo)
    {
        //gameOver.gameObject.SetActive(true);
        StartCoroutine("GoToMainMenu");
    }

    public void PlayerWin()
    {
        //win.gameObject.SetActive(true);
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
