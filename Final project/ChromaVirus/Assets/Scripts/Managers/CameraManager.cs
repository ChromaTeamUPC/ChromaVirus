using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    [HideInInspector]
    public Camera currentCamera;

    public GameObject mainCameraObj;
    public GameObject godCameraObj;
    public GameObject staticCamera1Obj;
    public GameObject staticCamera2Obj;
    public GameObject staticCamera3Obj;

    public DebugKeys keys;

    public void Init()
    {
        //Set main camera
        currentCamera = mainCameraObj.GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(keys.mainCameraActivationKey))
            ChangeCamera(0);
        else if (Input.GetKeyDown(keys.godCameraActivationKey))
            ChangeCamera(1);
        else if (Input.GetKeyDown(keys.staticCamera1ActivationKey))
            ChangeCamera(2);
        else if (Input.GetKeyDown(keys.staticCamera2ActivationKey))
            ChangeCamera(3);
        else if (Input.GetKeyDown(keys.staticCamera3ActivationKey))
            ChangeCamera(4);
        else if (Input.GetKeyDown(keys.mainCameraFollowPlayersKey))
            ToggleCameraFollowPlayers();
    }

    void ChangeCamera(int cameraIndex)
    {
        //Disable all cameras
        mainCameraObj.SetActive(false);
        godCameraObj.SetActive(false);
        staticCamera1Obj.SetActive(false);
        staticCamera2Obj.SetActive(false);
        staticCamera3Obj.SetActive(false);

        GameObject cameraObj = mainCameraObj;
        //Find selected camera
        switch (cameraIndex)
        {
            case 1:
                cameraObj = godCameraObj;
                break;
            case 2:
                cameraObj = staticCamera1Obj;
                break;
            case 3:
                cameraObj = staticCamera2Obj;
                break;
            case 4:
                cameraObj = staticCamera3Obj;
                break;
        }

        //Enable it and send event
        cameraObj.SetActive(true);
        currentCamera = cameraObj.GetComponent<Camera>();
        mng.eventManager.TriggerEvent(EventManager.EventType.CAMERA_CHANGED, new CameraEventInfo { newCamera = currentCamera });
    }

    void ToggleCameraFollowPlayers()
    {
        MainCameraScript script = mainCameraObj.GetComponent<MainCameraScript>();
        script.enabled = !script.enabled;
    }
}
