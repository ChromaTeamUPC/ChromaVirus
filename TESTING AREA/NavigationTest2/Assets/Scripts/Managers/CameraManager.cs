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
        //mainCamera.enabled = false;
        //godCamera.enabled = false;
        mainCameraObj.SetActive(false);
        godCameraObj.SetActive(false);
        staticCamera1Obj.SetActive(false);
        staticCamera2Obj.SetActive(false);
        staticCamera3Obj.SetActive(false);

        //Enable selected camera
        switch (cameraIndex)
        {
            case 0:
                mainCameraObj.SetActive(true);
                break;
            case 1:
                godCameraObj.SetActive(true);
                break;
            case 2:
                staticCamera1Obj.SetActive(true);
                break;
            case 3:
                staticCamera2Obj.SetActive(true);
                break;
            case 4:
                staticCamera3Obj.SetActive(true);
                break;
        }
    }
}
