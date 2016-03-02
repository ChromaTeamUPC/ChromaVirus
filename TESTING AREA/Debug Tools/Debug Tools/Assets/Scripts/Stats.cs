using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {

    int linePosition;

    string textFPS;
    string textTriangles;

    bool statsVisible = false;
    public bool viewWireFrame = false;

    void OnGUI()
    {
        Event e = Event.current;
        if (e.functionKey && e.type == EventType.keyUp)
            if (e.keyCode == KeyCode.F2) statsVisible = !statsVisible;
            else if (e.keyCode == KeyCode.F3) viewWireFrame = !viewWireFrame;

        if (statsVisible)
        {
            linePosition = 0;

            textFPS = "Fps: " + getCurrentFPS();
            textTriangles = "Triangles in scene: " + getCurrentTriangleCount();

            showStat(ref textFPS);
            showStat(ref textTriangles);
        }
    }

    void showStat(ref string text)
    {
        int w = Screen.width;
        int h = Screen.height;
        GUIStyle textStyle = new GUIStyle();

        Rect rect = new Rect(10, linePosition * 20, w, h * 2 / 100);

        textStyle.alignment = TextAnchor.UpperLeft;
        textStyle.fontSize = h * 2 / 100;
        textStyle.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        GUI.Label(rect, text, textStyle);
        linePosition++;

    }

    int getCurrentFPS()
    {
        return (int)(1.0f / Time.smoothDeltaTime);
    }

    int getCurrentTriangleCount()
    {
        int triangleCount = 0;

        object[] allGameObjects = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (GameObject singleObject in allGameObjects)
        {
            Component[] filters = singleObject.GetComponents(typeof(MeshFilter));
            foreach (MeshFilter f in filters)
            {
                triangleCount += f.sharedMesh.triangles.Length / 3;
            }
        }

        return triangleCount;
    }
}
