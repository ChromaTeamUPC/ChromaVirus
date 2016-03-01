using UnityEngine;
using System.Collections;

public enum ChromaColor
{
    RED,
    GREEN,
    BLUE,
    YELLOW
}

public class ColorManager : MonoBehaviour {
    public int changeInterval;

    private int[] colorCount = new int[] { 1, 1, 1, 1 };
    private ChromaColor currentColor;
    private int elapsedTime = 0;

    // Use this for initialization
    void Start () {
        currentColor = ChromaColor.RED;
    }
	
	void FixedUpdate () {
        elapsedTime += (int)(Time.deltaTime * 1000);

        if(elapsedTime >= changeInterval)
        {
            elapsedTime -= changeInterval;
            if (currentColor != ChromaColor.YELLOW)
                currentColor++;
            else
                currentColor = ChromaColor.RED;

            ColorChangedEventInfo info = new ColorChangedEventInfo();
            info.newColor = currentColor;
            mng.eventManager.TriggerEvent(EventManager.EventType.COLOR_CHANGED, info);
            /*switch (currentColor)
            {
                case ChromaColor.RED:
                    mng.eventManager.TriggerEvent(EventManager.EventType.COLOR_RED);
                    break;
                case ChromaColor.GREEN:
                    mng.eventManager.TriggerEvent(EventManager.EventType.COLOR_GREEN);
                    break;
                case ChromaColor.BLUE:
                    mng.eventManager.TriggerEvent(EventManager.EventType.COLOR_BLUE);
                    break;
                case ChromaColor.YELLOW:
                    mng.eventManager.TriggerEvent(EventManager.EventType.COLOR_YELLOW);
                    break;
            }*/
        }
	}
}
