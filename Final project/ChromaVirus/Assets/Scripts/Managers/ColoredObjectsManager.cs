using UnityEngine;
using System.Collections;

public class ColoredObjectsManager : MonoBehaviour
{
    //Player
    public Material player1Red;
    public Material player1Green;
    public Material player1Blue;
    public Material player1Yellow;

    private Material currentPlayer1;

    //Player Shot Materials
    public Material playerShotRed;
    public Material playerShotGreen;
    public Material playerShotBlue;
    public Material playerShotYellow;

    private Material currentPlayerShot;

    //Player Shot Light Color
    public Color playerShotRedLight;
    public Color playerShotGreenLight;
    public Color playerShotBlueLight;
    public Color playerShotYellowLight;

    private Color currentPlayerShotLight;

    //Voxel Materials
    public Material voxelRed;
    public Material voxelBlue;
    public Material voxelGreen;
    public Material voxelYellow;

    private Material currentVoxel;

    //Spider Materials
    public Material spiderRed;
    public Material spiderGreen;
    public Material spiderBlue;
    public Material spiderYellow;

    private Material currentSpider;

    private ObjectPool playerShotPool;
    private ObjectPool spiderPool;
    private ScriptObjectPool<VoxelController> voxelPool;

    private ChromaColor currentColor;

	void Start ()
    {
        Debug.Log("Colored Objects Manager created");

        playerShotPool = rsc.poolMng.playerShotPool;
        spiderPool = rsc.poolMng.spiderPool;
        voxelPool = rsc.poolMng.voxelPool;
        rsc.eventMng.StartListening(EventManager.EventType.COLOR_CHANGED, ColorChanged);
        currentColor = rsc.colorMng.CurrentColor;
        SetCurrentMaterials();
    }

    void OnDestroy()
    {
        if(rsc.eventMng != null)
            rsc.eventMng.StopListening(EventManager.EventType.COLOR_CHANGED, ColorChanged);
        Debug.Log("Colored Objects Manager destroyed");
    }

    void ColorChanged(EventInfo eventInfo)
    {
        ColorEventInfo info = (ColorEventInfo)eventInfo;
        currentColor = info.newColor;

        SetCurrentMaterials();
    }

    void SetCurrentMaterials()
    {
        switch (currentColor)
        {
            case ChromaColor.RED:
                currentPlayer1 = player1Red;
                currentPlayerShot = playerShotRed;
                currentPlayerShotLight = playerShotRedLight;
                currentVoxel = voxelRed;
                currentSpider = spiderRed;
                break;
            case ChromaColor.GREEN:
                currentPlayer1 = player1Green;
                currentPlayerShot = playerShotGreen;
                currentPlayerShotLight = playerShotGreenLight;
                currentVoxel = voxelGreen;
                currentSpider = spiderGreen;
                break;
            case ChromaColor.BLUE:
                currentPlayer1 = player1Blue;
                currentPlayerShot = playerShotBlue;
                currentPlayerShotLight = playerShotBlueLight;
                currentVoxel = voxelBlue;
                currentSpider = spiderBlue;
                break;
            case ChromaColor.YELLOW:
                currentPlayer1 = player1Yellow;
                currentPlayerShot = playerShotYellow;
                currentPlayerShotLight = playerShotYellowLight;
                currentVoxel = voxelYellow;
                currentSpider = spiderYellow;
                break;
        }
    }

    public Material GetPlayer1Material(ChromaColor color)
    {
        switch (color)
        {
            case ChromaColor.RED:
                return player1Red;
            case ChromaColor.GREEN:
                return player1Green;
            case ChromaColor.BLUE:
                return player1Blue;
            case ChromaColor.YELLOW:
                return player1Yellow;
        }

        return null; //Should not reach here
    }

    //Player Shot methods
    public GameObject GetPlayerShot()
    {
        //Get a shot from pool
        GameObject shot = playerShotPool.GetObject();

        if (shot != null)
        {
            shot.GetComponent<PlayerShotController>().color = currentColor;
            shot.GetComponent<Renderer>().material = currentPlayerShot;
        }

        return shot;
    }

    public GameObject GetPlayerShot(ChromaColor color)
    {
        //Get a shot from pool
        GameObject shot = playerShotPool.GetObject();

        if (shot != null)
        {
            switch (color)
            {
                case ChromaColor.RED:
                    shot.GetComponent<Renderer>().material = playerShotRed;
                    break;
                case ChromaColor.GREEN:
                    shot.GetComponent<Renderer>().material = playerShotGreen;
                    break;
                case ChromaColor.BLUE:
                    shot.GetComponent<Renderer>().material = playerShotBlue;
                    break;
                case ChromaColor.YELLOW:
                    shot.GetComponent<Renderer>().material = playerShotYellow;
                    break;
            }
            shot.GetComponent<PlayerShotController>().color = color;
        }

        return shot;
    }

    public Material GetPlayerShotMaterial(ChromaColor color)
    {
        switch (color)
        {
            case ChromaColor.RED:
                return playerShotRed;
            case ChromaColor.GREEN:
                return playerShotGreen;
            case ChromaColor.BLUE:
                return playerShotBlue;
            case ChromaColor.YELLOW:
                return playerShotYellow;
        }

        return null; //Should not reach here
    }

    public Color GetPlayerShotLightColor()
    {
        return currentPlayerShotLight;
    }

    //Spider methods
    public SpiderAIBehaviour GetSpider()
    {
        //Get a spider from pool
        GameObject spider = spiderPool.GetObject();       

        if (spider != null)
        {
            SpiderAIBehaviour spiderAI = spider.GetComponent<SpiderAIBehaviour>();
            spiderAI.color = currentColor;
            Material[] mats = spiderAI.spiderRenderer.materials;
            mats[1] = currentSpider;
            spiderAI.spiderRenderer.materials = mats;

            return spiderAI;
        }

        return null;
    }

    public SpiderAIBehaviour GetSpiderRandomColor()
    {
        return GetSpider(ChromaColorInfo.Random);
    }

    public SpiderAIBehaviour GetSpider(int offset)
    {
        int colorIndex = ((int)currentColor + offset) % ChromaColorInfo.Count;

        ChromaColor color = (ChromaColor)colorIndex;

        return GetSpider(color);
    }

    public SpiderAIBehaviour GetSpider(ChromaColor color)
    {
        //Get a spider from pool
        GameObject spider = spiderPool.GetObject();

        if (spider != null)
        {
            SpiderAIBehaviour spiderAI = spider.GetComponent<SpiderAIBehaviour>();
            spiderAI.color = color;
            Material[] mats = spiderAI.spiderRenderer.materials;
            switch (color)
            {
                case ChromaColor.RED:
                    mats[1] = spiderRed;
                    break;
                case ChromaColor.GREEN:
                    mats[1] = spiderGreen;
                    break;
                case ChromaColor.BLUE:
                    mats[1] = spiderBlue;
                    break;
                case ChromaColor.YELLOW:
                    mats[1] = spiderYellow;
                    break;
            }
            spiderAI.spiderRenderer.materials = mats;

            return spiderAI;
        }

        return null;
    }

    public Material GetSpiderMaterial(ChromaColor color)
    {
        switch (color)
        {
            case ChromaColor.RED:
                return spiderRed;
            case ChromaColor.GREEN:
                return spiderGreen;
            case ChromaColor.BLUE:
                return spiderBlue;
            case ChromaColor.YELLOW:
                return spiderYellow;
        }

        return null; //Should not reach here
    }

    //Voxel methods
    public VoxelController GetVoxel()
    {
        //Get a voxel from pool
        VoxelController voxel = voxelPool.GetObject();

        if (voxel != null)
        {
            voxel.GetComponent<Renderer>().material = currentVoxel;
        }

        return voxel;
    }

    public VoxelController GetVoxel(ChromaColor color)
    {
        //Get a voxel from pool
        VoxelController voxel = voxelPool.GetObject();

        if (voxel != null)
        {
            switch (color)
            {
                case ChromaColor.RED:
                    voxel.GetComponent<Renderer>().material = voxelRed;
                    break;
                case ChromaColor.GREEN:
                    voxel.GetComponent<Renderer>().material = voxelGreen;
                    break;
                case ChromaColor.BLUE:
                    voxel.GetComponent<Renderer>().material = voxelBlue;
                    break;
                case ChromaColor.YELLOW:
                    voxel.GetComponent<Renderer>().material = voxelYellow;
                    break;
            }
        }

        return voxel;
    }

    public Material GetVoxelMaterial(ChromaColor color)
    {
        switch (color)
        {
            case ChromaColor.RED:
                return voxelRed;
            case ChromaColor.GREEN:
                return voxelGreen;
            case ChromaColor.BLUE:
                return voxelBlue;
            case ChromaColor.YELLOW:
                return voxelYellow;
        }

        return null; //Should not reach here
    }

    public Material GetVoxelRandomMaterial()
    {
        return GetVoxelMaterial(ChromaColorInfo.Random);
    }
}
