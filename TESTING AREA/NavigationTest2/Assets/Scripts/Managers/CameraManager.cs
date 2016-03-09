using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public GameObject mainCameraObj;
    public GameObject godCameraObj;
    public GameObject staticCamera1Obj;
    public GameObject staticCamera2Obj;
    public GameObject staticCamera3Obj;

    public DebugKeys keys;
	
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
    }

    void ChangeCamera(int cameraIndex)
    {
        //Disable all cameras
        mainCameraObj.SetActive(false);
        godCameraObj.SetActive(false);
        staticCamera1Obj.SetActive(false);
        staticCamera2Obj.SetActive(false);
        staticCamera3Obj.SetActive(false);

        GameObject camera = mainCameraObj;
        //Find selected camera
        switch (cameraIndex)
        {
            case 1:
                camera = godCameraObj;
                break;
            case 2:
                camera = staticCamera1Obj;
                break;
            case 3:
                camera = staticCamera2Obj;
                break;
            case 4:
                camera = staticCamera3Obj;
                break;
        }

        //Enable it and send event
        camera.SetActive(true);
        mng.eventManager.TriggerEvent(EventManager.EventType.CAMERA_CHANGED, new CameraEventInfo { newCamera = camera });
    }
}
