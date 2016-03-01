using UnityEngine;
using System.Collections;

public class ColorChangeTest : MonoBehaviour {

    public Material colorRed;
    public Material colorGreen;
    public Material colorBlue;
    public Material colorYellow;

    Renderer thisRenderer;

    // Use this for initialization
    void Start () {
        thisRenderer = GetComponent<Renderer>();

        EventManager evMan = mng.eventManager;

        //evMan.StartListening(EventManager.EventType.COLOR_RED, ColorRed);
        //evMan.StartListening(EventManager.EventType.COLOR_GREEN, ColorGreen);
        //evMan.StartListening(EventManager.EventType.COLOR_BLUE, ColorBlue);
        //evMan.StartListening(EventManager.EventType.COLOR_YELLOW, ColorYellow);
        evMan.StartListening(EventManager.EventType.COLOR_CHANGED, ColorChanged);
    }

    void ColorChanged(EventInfo eventInfo)
    {
        ColorChangedEventInfo info = (ColorChangedEventInfo)eventInfo;

        switch (info.newColor)
        {
            case ChromaColor.RED:
                thisRenderer.material = colorRed;
                break;
            case ChromaColor.GREEN:
                thisRenderer.material = colorGreen;
                break;
            case ChromaColor.BLUE:
                thisRenderer.material = colorBlue;
                break;
            case ChromaColor.YELLOW:
                thisRenderer.material = colorYellow;
                break;
        }
    }

    void ColorRed()
    {
        thisRenderer.material = colorRed;
    }

    void ColorGreen()
    {
        thisRenderer.material = colorGreen;
    }

    void ColorBlue()
    {
        thisRenderer.material = colorBlue;
    }

    void ColorYellow()
    {
        thisRenderer.material = colorYellow;
    }
}
