using UnityEngine;
using System.Collections;

public class EntryCameraScript : MonoBehaviour {

    public Camera entryCamera;
    public Camera mainCamera;

	void ChangeToMainCamera()
    {
        mainCamera.transform.position = entryCamera.transform.position;
        mainCamera.transform.rotation = entryCamera.transform.rotation;
        entryCamera.enabled = false;
    }
}
