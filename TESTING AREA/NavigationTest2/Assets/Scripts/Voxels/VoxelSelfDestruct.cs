using UnityEngine;
using System.Collections;

public class VoxelSelfDestruct : MonoBehaviour {

    public float minDuration;
    public float maxDuration;
    public float duration = 0;

    private float realDuration;

    void OnEnable()
    {
        if (duration != 0)
            realDuration = duration;
        else
            realDuration = Random.Range(minDuration, maxDuration);
    }

    // Update is called once per frame
    void Update () {
        realDuration -= Time.deltaTime;
        if (realDuration <= 0.0f)
            mng.poolManager.voxelPool.AddObject(gameObject);
    }
}
