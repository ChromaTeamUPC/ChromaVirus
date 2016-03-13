using UnityEngine;
using System;
using System.Collections;

//Helper class to know the first and last colors defined in the enumerator, so we can check for .First or .Last and always will return the proper color even if we reorder the enum
public class ChromaColorInfo
{
    public static ChromaColor First = (ChromaColor)Enum.GetValues(typeof(ChromaColor)).GetValue(0);
    public static ChromaColor Last = (ChromaColor)Enum.GetValues(typeof(ChromaColor)).GetValue(Enum.GetValues(typeof(ChromaColor)).Length - 1);
}

public enum ChromaColor
{
    RED,
    GREEN,
    BLUE,
    YELLOW
}

public class ColorManager : MonoBehaviour
{
    public int changeInterval;

    private int[] colorCount = new int[] { 0, 0, 0, 0 };
    private ChromaColor currentColor;
    private int elapsedTime = 0;

    public void Init()
    {
        currentColor = ChromaColor.RED;
        mng.eventManager.StartListening(EventManager.EventType.ENEMY_SPAWNED, EnemySpawned);
        mng.eventManager.StartListening(EventManager.EventType.ENEMY_DIED, EnemyDied);
    }

    // Use this for initialization
    void Start () {
        
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
        if (colorCount[(int)info.newColor] < 0) colorCount[(int)info.newColor] = 0;
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
