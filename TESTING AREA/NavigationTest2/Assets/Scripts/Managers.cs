using UnityEngine;
using System.Collections;

public struct mng
{
    public static GameManager gameManager;
    public static ColorManager colorManager;
    public static CameraManager cameraManager;
    public static PoolManager poolManager;
    public static EventManager eventManager;
}


public class Managers : MonoBehaviour {

    public GameManager gameManager;
    public ColorManager colorManager;
    public CameraManager cameraManager;
    public PoolManager poolManager;
    public EventManager eventManager;

    // Use this for initialization
    void Awake () {
        mng.gameManager = gameManager;
        mng.colorManager = colorManager;
        mng.cameraManager = cameraManager;
        mng.poolManager = poolManager;
        mng.eventManager = eventManager;
	}
}
