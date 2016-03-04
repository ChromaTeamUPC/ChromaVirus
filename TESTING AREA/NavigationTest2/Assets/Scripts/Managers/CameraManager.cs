using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public GameObject mainCameraObj;
    public GameObject godCameraObj;

    public DebugKeys keys;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(keys.mainCameraActivationKey))
            ChangeCamera(0);
        else if (Input.GetKeyDown(keys.godCameraActivationKey))
            ChangeCamera(1);
    }

    void ChangeCamera(int cameraIndex)
    {
        //Disable all cameras
        //mainCamera.enabled = false;
        //godCamera.enabled = false;
        mainCameraObj.SetActive(false);
        godCameraObj.SetActive(false);

        //Enable selected camera
        switch (cameraIndex)
        {
            case 0:
                //mainCamera.enabled = true;
                mainCameraObj.SetActive(true);
                break;
            case 1:
                //godCamera.enabled = true;
                godCameraObj.SetActive(true);
                break;
        }
    }
}
