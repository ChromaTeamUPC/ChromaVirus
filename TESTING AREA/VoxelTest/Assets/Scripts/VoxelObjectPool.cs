using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelObjectPool : MonoBehaviour {
     
    public GameObject voxel;
    public int poolSize = 1000;

    private Queue<GameObject> pool;
    private GameObject aux;

	// Use this for initialization
	void Start () {
        pool = new Queue<GameObject>(poolSize);
        for (int i = 0; i < poolSize; ++i)
        {
            aux = Instantiate(voxel) as GameObject;
            aux.SetActive(false);
            pool.Enqueue(aux);
        }
	}
	
    public GameObject GetVoxel()
    {
        if (pool.Count > 0)
            return pool.Dequeue();
        else
            return null;
    }

    public void AddVoxel(GameObject voxel)
    {
        voxel.SetActive(false);
        pool.Enqueue(voxel);
    }

    public int GetPoolCount()
    {
        return pool.Count;
    }
}
