using UnityEngine;
using System.Collections;

public class ShotManager : MonoBehaviour {

    public Material redMaterial;
    public Material greenMaterial;
    public Material blueMaterial;
    public Material yellowMaterial;

    private ObjectPool shotObjectPool;
    private ChromaColor currentColor;
    private Material currentMaterial;

    public void Init()
    {
        shotObjectPool = mng.poolManager.shotPool;
        mng.eventManager.StartListening(EventManager.EventType.COLOR_CHANGED, ColorChanged);
    }

    void ColorChanged(EventInfo eventInfo)
    {
        ColorEventInfo info = (ColorEventInfo)eventInfo;
        currentColor = info.newColor;

        switch (currentColor)
        {
            case ChromaColor.RED:
                currentMaterial = redMaterial;
                break;
            case ChromaColor.GREEN:
                currentMaterial = greenMaterial;
                break;
            case ChromaColor.BLUE:
                currentMaterial = blueMaterial;
                break;
            case ChromaColor.YELLOW:
                currentMaterial = yellowMaterial;
                break;
        }
    }

    public GameObject GetShot()
    {
        //Get a shot from pool
        GameObject shot = shotObjectPool.GetObject();

        if (shot != null)
        {
            shot.GetComponent<ShotMover>().color = currentColor;
            shot.GetComponent<Renderer>().material = currentMaterial;
        }

        return shot;
    }
}
