using UnityEngine;
using System.Collections;

public class WireFrame : MonoBehaviour
{
    public Stats stats;
    void OnPreRender()
    {
        if (stats.viewWireFrame)
            GL.wireframe = true;
    }
    void OnPostRender()
    {
        GL.wireframe = false;
    }
}