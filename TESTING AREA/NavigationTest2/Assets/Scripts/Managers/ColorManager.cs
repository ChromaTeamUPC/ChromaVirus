using UnityEngine;
using System.Collections;

public class ChromaColorInfo
{
    public static ChromaColor First = ChromaColor.RED;
    public static ChromaColor Last = ChromaColor.YELLOW;
}
public enum ChromaColor
{
    RED,
    GREEN,
    BLUE,
    YELLOW
}

public class ColorManager : MonoBehaviour {
    public int changeInterval;

    private int[] colorCount = new int[] { 1, 0, 1, 0 };
    private ChromaColor currentColor;
    private int elapsedTime = 0;

    // Use this for initialization
    void Start () {
        currentColor = ChromaColor.RED;
        mng.eventManager.StartListening(EventManager.EventType.ENEMY_SPAWNED, EnemySpawned);
        mng.eventManager.StartListening(EventManager.EventType.ENEMY_SPAWNED, EnemyDied);
    }

    public void EnemySpawned(EventInfo eventInfo)
    {
        ColorEventInfo info = (ColorEventInfo)eventInfo;
        ++colorCount[(int)info.newColor];
    }

    public void EnemyDied(EventInfo eventInfo)
    {
        ColorEventInfo info = (ColorEventInfo)eventInfo;
        --colorCount[(int)info.newColor];
    }

    void FixedUpdate () {
        elapsedTime += (int)(Time.fixedDeltaTime * 1000);

        if(elapsedTime >= changeInterval)
        {
            elapsedTime -= changeInterval;
            SetNewColor();        
        }
	}

    private void SetNewColor()
    {
        //If there is no color enemies in the scene, no color change
        if (TotalColorItems() == 0) return;

        //Search first color in loop that has items
        do
        {
            if (currentColor == ChromaColorInfo.Last)
                currentColor = ChromaColorInfo.First;        
            else
                currentColor++;
        }
        while (colorCount[(int)currentColor] <= 0);

        mng.eventManager.TriggerEvent(EventManager.EventType.COLOR_CHANGED, new ColorEventInfo { newColor = currentColor });
    }

    private int TotalColorItems()
    {
        int sum = colorCount[0];

        for (int i = 1; i < colorCount.Length; ++i)
            sum += colorCount[i];

        return sum;
    }
}
