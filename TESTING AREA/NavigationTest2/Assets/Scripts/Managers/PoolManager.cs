using UnityEngine;
using System.Collections;

//Just a container for the different pools we have in the game
public class PoolManager : MonoBehaviour
{   
    public ObjectPool shotPool;
    public ObjectPool aracnoBotPool;

    public ScriptObjectPoolDefiner voxelControllerDefiner;
    [HideInInspector]
    public ScriptObjectPool<VoxelController> voxelControllerPool;
    public ObjectPool voxelColliderPool;

    public void Init()
    {
        voxelControllerPool = new ScriptObjectPool<VoxelController>();
        if (voxelControllerDefiner != null)
        {
            voxelControllerPool.objectsParent = voxelControllerDefiner.gameObject;
            voxelControllerPool.poolSize = voxelControllerDefiner.poolSize;
            voxelControllerPool.objectWhereScriptIs = voxelControllerDefiner.objectWhereScriptIs;
            voxelControllerPool.Init();
        }
        Debug.Log("Pool manager init");
    }

    void Awake()
    {
        Debug.Log("Pool manager awake");
    }

    void Start()
    {
        Debug.Log("Pool manager start");
    }
}
