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

            mng.eventManager.TriggerEvent(EventManager.EventType.COLOR_CHANGED, new ColorChangedEventInfo { newColor = currentColor });
        }
	}
}
