using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public Camera mainCamera;
    public KeyCode mainCameraActivationKey;
    public Camera godCamera;
    public KeyCode godCameraActivationKey;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(mainCameraActivationKey))
            ChangeCamera(0);
        else if (Input.GetKeyDown(godCameraActivationKey))
            ChangeCamera(1);
    }

    void ChangeCamera(int cameraIndex)
    {
        //Disable all cameras
        mainCamera.enabled = false;
        godCamera.enabled = false;

        //Enable selected camera
        switch(cameraIndex)
        {
            case 0:
                mainCamera.enabled = true;
                break;
            case 1:
                godCamera.enabled = true;
                break;
        }
    }
}
