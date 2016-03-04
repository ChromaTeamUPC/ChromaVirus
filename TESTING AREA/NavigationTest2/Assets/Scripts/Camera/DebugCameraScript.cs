using UnityEngine;
using System.Collections;

public class DebugCameraScript : MonoBehaviour {

    public DebugKeys keys;

    private bool viewWireFrame = false;

    void Update()
    {
        if (Input.GetKeyDown(keys.toggleWireframeKey))
            viewWireFrame = !viewWireFrame;
    }

    void OnPreRender()
    {
        if (viewWireFrame)
            GL.wireframe = true;
    }

    void OnPostRender()
    {
        GL.wireframe = false;
    }
}
