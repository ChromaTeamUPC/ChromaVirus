using UnityEngine;
using System.Collections;

public class VoxelCollider : MonoBehaviour {

    public int framesToStay = 1;

    private int currentFrames;

    void OnEnable()
    {
        currentFrames = framesToStay;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    if(currentFrames == 0)
        {
            mng.poolManager.voxelColliderPool.AddObject(gameObject);
        }
        --currentFrames;
	}
}
