using UnityEngine;
using System.Collections;

public struct Man
{
    public static VoxelObjectPool voxelPool;
}

public class Managers : MonoBehaviour {

    public VoxelObjectPool voxelPool;

	// Use this for initialization
	void Awake () {
        Man.voxelPool = voxelPool;
	}
}
