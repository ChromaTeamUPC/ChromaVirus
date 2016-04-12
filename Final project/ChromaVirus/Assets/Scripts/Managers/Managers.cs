using UnityEngine;
using System.Collections;

public struct mng
{
    public static EventManager eventManager;
    public static PoolManager poolManager;   
    public static ColorManager colorManager;
    public static CameraManager cameraManager;
    public static ShotManager shotManager;
    public static UIManager uiManager;

    public static GameManager gameManager;
}


public class Managers : MonoBehaviour
{
    public EventManager eventManager;
    public PoolManager poolManager;
    public ColorManager colorManager;
    public CameraManager cameraManager;
    public ShotManager shotManager;
    public UIManager uiManager;

    public GameManager gameManager;
    
    // Use this for initialization
    void Awake ()
    {
        Debug.Log("Managers awake");
        mng.eventManager = eventManager;
        mng.poolManager = poolManager;
        mng.colorManager = colorManager;
        mng.cameraManager = cameraManager;
        mng.shotManager = shotManager;
        mng.uiManager = uiManager;

        mng.gameManager = gameManager;


        //Init the managers (order matters)
        //EventManager should be the first one, so the others can start listening for events when init
        eventManager.Init();
        poolManager.Init();
        colorManager.Init();
        cameraManager.Init();
        shotManager.Init();
        uiManager.Init();


        gameManager.Init();
        
    }

    void Start()
    {
        Debug.Log("Manager start");
    }
}
