using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    public GameObject player1;
    private PlayerHealth player1Health;
    private PlayerShoot player1Shot;

    public Text player1HealthTxt;
    public Text player1EnergyTxt;

    public Text win;
    public Text gameOver;

    public Material enemyRedMaterial;
    public Material enemyGreenMaterial;
    public Material enemyBlueMaterial;
    public Material enemyYellowMaterial;
    private ObjectPool aracnoBotPool;


    // Use this for initialization
    void Start () {
        mng.eventManager.StartListening(EventManager.EventType.PLAYER_SPAWNED, PlayerSpawned);
        mng.eventManager.StartListening(EventManager.EventType.PLAYER_DAMAGED, PlayerDamaged);
        mng.eventManager.StartListening(EventManager.EventType.PLAYER_DIED, PlayerDied);

        player1Health = player1.GetComponent<PlayerHealth>();
        player1Shot = player1.GetComponent<PlayerShoot>();

        aracnoBotPool = mng.poolManager.aracnoBotPool;

        //Test
        InvokeRepeating("SpawnEnemies", 2f, 3f);
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
        enemy.GetComponent<Renderer>().material = mat;

        //Activate enemy
        enemy.SetActive(true);
    }

    void Update()
    {
        player1EnergyTxt.text = "Energy: " + (int)player1Shot.currentEnergy;

    }


    void PlayerSpawned(EventInfo eventInfo)
    {
        PlayerDamagedEventInfo info = (PlayerDamagedEventInfo)eventInfo;
        player1HealthTxt.text = "Life: " + info.currentHealth;
    }

    void PlayerDamaged(EventInfo eventInfo)
    {
        PlayerDamagedEventInfo info = (PlayerDamagedEventInfo)eventInfo;
        player1HealthTxt.text = "Life: " + info.currentHealth;
    }

    public void PlayerDied(EventInfo eventInfo)
    {
        gameOver.gameObject.SetActive(true);
        StartCoroutine("GoToMainMenu");
    }

    public void PlayerWin()
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
